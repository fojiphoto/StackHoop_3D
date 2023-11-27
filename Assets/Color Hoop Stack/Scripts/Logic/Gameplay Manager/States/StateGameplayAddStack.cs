using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayAddStack : StateGameplay
{
    public StateGameplayAddStack(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        gameplayMgr.AddRingStack();
        stateMachine.StateChange(gameplayMgr.stateGameplayIdle);
    }

    public override void OnPhysicsUpdate()
    {
        base.OnPhysicsUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
