using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplay : State
{
    protected GameplayMgr gameplayMgr;

    public StateGameplay(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base (_stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }


}
