using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingDown : StateGameplay
{
    public StateGameplayRingDown(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Ring ringMove = InputMgr.Instance.ringMove;
        RingStack ringStackEnd = InputMgr.Instance.ringStackEnd;
        Utils.Common.Log("place order = " + (ringStackEnd.ringStack.Count - 1));
        float newY = -1.123066f + ringStackEnd.boxCol.size.z / 2 + ringMove.boxCol.size.z / 2 + ringMove.boxCol.size.z * (ringStackEnd.ringStack.Count - 1);
        Sequence seqRingDown = DOTween.Sequence();
        seqRingDown.Append(
            ringMove.transform.DOMoveY(newY, (ringMove.transform.position.y - newY) / gameplayMgr.ringDownSpeed).SetEase(Ease.Linear)
            );
        seqRingDown.AppendCallback(
            () => SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.DROP], false)
            );
             seqRingDown.AppendCallback(
            () => ringMove.transform.GetComponent<Animator>().enabled=false
                
            );
        Vector3 newPos = new Vector3(ringMove.transform.position.x, newY, ringMove.transform.position.z);
        seqRingDown.Append(
            ringMove.transform.DOJump(newPos, gameplayMgr.ringJumpPower, 2, gameplayMgr.ringJumpTime)
            );
        ChangeToNextState();
    }

    private void ChangeToNextState()
    {
        stateMachine.StateChange(gameplayMgr.stateGameplayIdle);
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

        if (InputMgr.Instance.isUndoMove)
        {
            InputMgr.Instance.isUndoMove = false;
            return;
        }
        else
        {
            if (InputMgr.Instance.ringStackEnd.IsStackFullSameColor())
            {
                float newRingYPos = InputMgr.Instance.ringStackEnd.transform.position.y +
                    InputMgr.Instance.ringStackEnd.boxCol.size.y / 2 +
                    InputMgr.Instance.ringStackEnd.boxCol.size.z / 2;
                Vector3 newPos = new Vector3(InputMgr.Instance.ringStackEnd.transform.position.x, newRingYPos, InputMgr.Instance.ringStackEnd.transform.position.z);
                SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.FULL_STACK], false);
                GameObject particleGO = PoolerMgr.Instance.VFXCompletePooler.GetNextPS();
                particleGO.transform.position = newPos;
            }
        }
    }
}
