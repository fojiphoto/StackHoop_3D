using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingReady : StateGameplay
{
    public StateGameplayRingReady(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        gameplayMgr.readyToTouch = true;
        if (gameplayMgr.currentLevel == 0)
        {
            gameplayMgr.ringStackList[1].canControl = true;
        }

        if (gameplayMgr.currentLevel == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.ON_CHANGE_TUTORIAL_TEXT);
        }
        
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

        gameplayMgr.readyToTouch = false;
    }
}
