using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingMove : StateGameplay
{
    private Ring ringMove;

    public StateGameplayRingMove(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        gameplayMgr.PushMapLevel();

        MoveRings(InputMgr.Instance.ringStackStart, InputMgr.Instance.ringStackEnd);
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void MoveRings(RingStack ringStackStart, RingStack ringStackEnd)
    {
        ringStackEnd.canControl = false;
        int blankSlots = gameplayMgr.stackNumberMax - ringStackEnd.ringStack.Count;
        int ringNumber = 0;
        RingType moveRingType = InputMgr.Instance.ringMove.ringType;

        while(ringStackStart.ringStack.Count > 0)
        {
            ringMove = ringStackStart.ringStack.Pop();
            ringMove.transform.SetParent(ringStackEnd.transform);
            ringStackEnd.ringStack.Push(ringMove);

            float newRingYPos = ringStackStart.transform.position.y +
                ringStackStart.boxCol.size.y / 2 +
                ringMove.boxCol.size.z / 2;

            Vector3 newPos = new Vector3(ringStackEnd.transform.position.x, newRingYPos, ringStackEnd.transform.position.z);
            float distance = Vector3.Distance(ringMove.transform.position, newPos);

            Sequence ringMoveSeq = DOTween.Sequence();
            ringMoveSeq.PrependInterval(gameplayMgr.waitTime * (ringNumber));

            float newY = -1.123066f + ringStackEnd.boxCol.size.z / 2 + ringMove.boxCol.size.z / 2 + ringMove.boxCol.size.z * (ringStackEnd.ringStack.Count - 1);

            //move up
            ringMoveSeq.Append(
                ringMove.transform.DOMoveY(newRingYPos, (newRingYPos - ringMove.transform.position.y) / gameplayMgr.ringUpSpeed)
                .SetEase(Ease.Linear)
                );
             ringMoveSeq.AppendCallback(
                () => ringMove.transform.GetComponent<Animator>().enabled=true
            );

            //move to another stack
            ringMoveSeq.Append(
                ringMove.transform.DOMove(newPos, distance / gameplayMgr.ringMoveSpeed).SetEase(Ease.Linear)
                );

            //move down
            ringMoveSeq.Append(
                ringMove.transform.DOMoveY(newY, (newRingYPos - newY) / gameplayMgr.ringDownSpeed).SetEase(Ease.Linear)
                );
            ringMoveSeq.AppendCallback(
                () => ringMove.transform.GetComponent<Animator>().enabled=false
            );

            ringMoveSeq.AppendCallback(() => SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.DROP], false));

            //jump
            ringMoveSeq.Append(
                ringMove.transform.DOJump(new Vector3(newPos.x, newY, newPos.z), gameplayMgr.ringJumpPower, 2, gameplayMgr.ringJumpTime)
                );

            ringNumber++;
            if (blankSlots <= ringNumber)
            {
                ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                break;
            }
            if (ringStackStart.ringStack.Count == 0)
            {
                ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                break;
            }
            if (ringStackStart.ringStack.Peek().ringType != moveRingType)
            {
                ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                break;
            }
        }

        stateMachine.StateChange(gameplayMgr.stateGameplayIdle);
    }

    public void ActiveControlStack(RingStack ringStack)
    {
        if (ringStack.IsStackFullSameColor())
        {
            gameplayMgr.stackCompleteNumber++;
            if (gameplayMgr.CheckWinState())
            {
                gameplayMgr.TriggerCompleteLevelEffect(0f);
                                
                stateMachine.StateChange(gameplayMgr.stateGameplayCompleteLevel);

            }
            else
            {
                float newRingYPos = ringStack.transform.position.y + ringStack.boxCol.size.y / 2 + ringStack.boxCol.size.z / 2;
                Vector3 newPos = new Vector3(ringStack.transform.position.x, newRingYPos, ringStack.transform.position.z);
                SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.FULL_STACK], false);
                GameObject particleGO = PoolerMgr.Instance.VFXCompletePooler.GetNextPS();
                particleGO.transform.position = newPos;
                 Transform cap= ringStack.transform.GetChild(2);
                cap.gameObject.SetActive(true);
            }
        }
        else
        {
            ringStack.canControl = true;
            Transform cap= ringStack.transform.GetChild(2);
                cap.gameObject.SetActive(false);
        }
    }
}
