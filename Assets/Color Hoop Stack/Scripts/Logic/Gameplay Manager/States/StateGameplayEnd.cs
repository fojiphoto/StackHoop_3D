using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayEnd : StateGameplay
{
    public StateGameplayEnd(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        foreach (RingStack ringStack in gameplayMgr.ringStackList)
        {
            while (ringStack.ringStack.Count > 0)
            {
                Ring ring = ringStack.ringStack.Pop();
                PoolerMgr.Instance.ringPooler.ReturnPooledObject(ring.gameObject);
            }
            ringStack.ringStack.Clear();
            PoolerMgr.Instance.ringStackPooler.ReturnPooledObject(ringStack.gameObject);
        }

        gameplayMgr.ringStackList.Clear();
        gameplayMgr.mapDataStack.Clear();

        EventDispatcher.Instance.PostEvent(EventID.ON_DISABLED_TUTORIAL);
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
        //GoogleAdMobController.Instance.DisableBannerAdWarpper();
        //GoogleAdMobController.Instance.DestroyBannerAd();
        CASAds.instance.ShowInterstitial();
    }
}
