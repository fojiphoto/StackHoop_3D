using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingUp : StateGameplay
{
    private Ring ringReady = null;
    private RingStack ringStackReady = null;

    public StateGameplayRingUp(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        RingStack ringStackStart = InputMgr.Instance.ringStackStart;
        Ring ringMove = InputMgr.Instance.ringMove;
        ringMove.isMoving = true;

        float newRingYPos = ringStackStart.transform.position.y +
            ringStackStart.boxCol.size.y / 2 +
            ringMove.boxCol.size.z / 2 + .5f;


        Sequence ringUpSeq = DOTween.Sequence();

        ringUpSeq.Append(ringMove.transform.DOMoveY(newRingYPos, (newRingYPos - ringMove.transform.position.y) / gameplayMgr.ringUpSpeed).SetEase(Ease.Linear));
        ringUpSeq.AppendCallback(
               () =>
               {    
                    //ringMove.transform.GetComponent<Animator>().enabled=false;
                    gameplayMgr.CloseRingAnimator(ringMove);
                   ringMove.transform.GetChild(1).gameObject.SetActive(true);
               }
               );



        if (ringReady != null)
        {
            Sequence ringDownSeq = DOTween.Sequence();
            ringReady.isMoving = false;
            float newY = -1.123066f + ringStackReady.boxCol.size.z / 2 + ringReady.boxCol.size.z / 2 + ringReady.boxCol.size.z * (ringStackReady.ringStack.Count - 1);
            ringDownSeq.Append(
                ringReady.transform.DOMoveY(newY, (ringMove.transform.position.y - newY) / gameplayMgr.ringDownSpeed).SetEase(Ease.Linear)
                );

            ringDownSeq.AppendCallback(
                () => SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.DROP], false)
                );

            Vector3 newPos = new Vector3(ringReady.transform.position.x, newY, ringReady.transform.position.z);
            ringDownSeq.Append(
                ringReady.transform.DOJump(newPos, gameplayMgr.ringJumpPower, 2, gameplayMgr.ringJumpTime)
                );
        }
        ChangeToNextState();
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

        ringReady = null;
        ringStackReady = null;
    }

    public void ChangeToNextState()
    {
        stateMachine.StateChange(gameplayMgr.stateGameplayRingReady);

    }

    public void GetRingReady(Ring ring, RingStack ringStackReady)
    {
        this.ringReady = ring;
        this.ringStackReady = ringStackReady;
    }
}
