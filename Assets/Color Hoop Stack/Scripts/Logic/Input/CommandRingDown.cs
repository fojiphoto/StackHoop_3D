using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRingDown : Command
{
    public RingStack ringStackEnd;
    public Ring ring;

    public CommandRingDown(RingStack ringStackEnd, Ring ringMove)
    {
        this.ringStackEnd = ringStackEnd;
        this.ring = ringMove;
    }

    public override void Execute()
    {
        base.Execute();
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingDown);

        if (GameplayMgr.Instance.currentLevel == 1)
        {
            EventDispatcher.Instance.PostEvent(EventID.ON_DISABLED_CORRECTOR);
        }
    }

    public override void Undo()
    {
        base.Undo();

        InputMgr.Instance.ringStackStart = ringStackEnd;
        InputMgr.Instance.ringMove = ring;
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingUp);
    }
}
