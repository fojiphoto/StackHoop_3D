using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private State currentState;

    public State CurrentState { get => currentState; set => currentState = value; }

    public void StateHandleInput()
    {
        if (currentState != null)
            currentState.OnHandleInput();
        else
            Utils.Common.Log("Don't have any state in state machine to handle input!");
    }

    public void StateLogicUpdate()
    {
        if (currentState != null)
            currentState.OnLogicUpdate();
        else
            Utils.Common.Log("Don't have any state in state machine to update logic!");
    }

    public void StatePhysicsUpdate()
    {
        if (currentState != null)
            currentState.OnPhysicsUpdate();
        else
            Utils.Common.Log("Don't have any state in state machine to update physics!");
    }

    public void StateChange(State state)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter();
        }
    }
}
