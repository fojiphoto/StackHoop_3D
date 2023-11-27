using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayIdle : StateGameplay
{
    public StateGameplayIdle(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        gameplayMgr.readyToTouch = true;

        FileHandler fileHandler = new FileHandler();
        fileHandler.SaveLevelData();
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
