using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRingMove : Command
{
    public RingStack ringStackStart;
    public RingStack ringStackEnd;
    public int ringMoveNumber = 0;

    public CommandRingMove(RingStack ringStackStart, RingStack ringStackEnd)
    {
        this.ringStackStart = ringStackStart;
        this.ringStackEnd = ringStackEnd;
    }
    public override void Execute()
    {
        base.Execute();
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingMove);

        if (GameplayMgr.Instance.currentLevel == 1)
        {
            EventDispatcher.Instance.PostEvent(EventID.ON_DISABLED_CORRECTOR);
        }
    }

    public override void Undo()
    {
        base.Undo();
        if (!InputMgr.Instance.ringStackEnd.canControl)
        {
            InputMgr.Instance.ringStackEnd.canControl = true;
            GameplayMgr.Instance.stackCompleteNumber--;
        }
        InputMgr.Instance.ringStackEnd = ringStackStart;
        InputMgr.Instance.ringStackStart = ringStackEnd;
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingMove);
    }
}
