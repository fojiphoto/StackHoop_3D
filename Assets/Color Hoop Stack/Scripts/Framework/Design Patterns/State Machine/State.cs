using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected StateMachine stateMachine;

    public State(StateMachine _stateMachine)
    {
        stateMachine = _stateMachine;
    }

    public virtual void OnEnter()
    {
        //DisplayOnUI(UIManager.Alignment.Left);
        Utils.Common.Log("enter state: " + GetType().Name);
          GameplayMgr.Instance.DeactivateCapForAllRingStacks();
    }

    public virtual void OnHandleInput()
    {

    }

    public virtual void OnLogicUpdate()
    {

    }

    public virtual void OnPhysicsUpdate()
    {

    }

    public virtual void OnExit()
    {
        Utils.Common.Log("exit state: " + GetType().Name);
          GameplayMgr.Instance.DeactivateCapForAllRingStacks();
    }
}
