using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayCompleteLevel : StateGameplay
{
    public StateGameplayCompleteLevel(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Utils.Common.Log("win game!");
         GameplayMgr.Instance.DeactivateCapForAllRingStacks();
        EventDispatcher.Instance.PostEvent(EventID.ON_COMPLETE_LEVEL);
        EventDispatcher.Instance.PostEvent(EventID.ON_DISABLED_TUTORIAL);
        gameplayMgr.currentLevel++;
        gameplayMgr.mapDataStack.Clear();
        FileHandler fileHandler = new FileHandler();
        fileHandler.SaveLevelData();
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
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
