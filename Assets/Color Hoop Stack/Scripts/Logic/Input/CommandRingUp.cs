using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CommandRingUp : Command
{
    public RingStack ringStackStart;
    public Ring ring;
    public Ring ringReady = null;
    public RingStack ringStackReady = null;

    public CommandRingUp(RingStack ringStackStart, Ring ring, Ring ringReady, RingStack ringStackReady)
    {
        this.ring = ring;
        this.ringStackStart = ringStackStart;
        this.ringReady = ringReady;
        this.ringStackReady = ringStackReady;
        this.ring.GetComponent<Animator>().enabled=true;
      
        Vibration.Vibrate(50);
    }

    public override void Execute()
    {
        base.Execute();
        GameplayMgr.Instance.stateGameplayRingUp.GetRingReady(ringReady, ringStackReady);
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingUp);
        ringReady = null;
        ringStackReady = null;
       
        if (GameplayMgr.Instance.currentLevel == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.ON_MOVE_TUTORIAL_CURSOR);
        }
        else if (GameplayMgr.Instance.currentLevel == 1)
        {
            EventDispatcher.Instance.PostEvent(EventID.ON_ENABLED_CORRECTOR, ringStackStart);
        }
       // CheckAndMoveUp(ringStackStart, ring);
       
    }

    public override void Undo()
    {
        base.Undo();
        InputMgr.Instance.ringStackEnd = ringStackStart;
        InputMgr.Instance.ringMove = ring;
        GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayRingDown);
    }
    

    //private void CheckAndMoveUp(RingStack ringStack, Ring currentRing)
    //{
    //    // Check if there's a ring below with the same color and move it up if needed
    //    List<Ring> ringList = new List<Ring>(ringStack.ringStack);
    //    if (ringList.Count > 1)
    //    {
    //        Ring ringBelow = ringList[ringList.Count - 2];  // Get the ring below the clicked ring
    //        if (ringBelow.ringType == currentRing.ringType)
    //        {
    //            float newY = currentRing.transform.position.y + currentRing.boxCol.size.z;  // Adjust as needed
    //            ringBelow.transform.DOMoveY(newY, (newY - ringBelow.transform.position.y) / GameplayMgr.Instance.ringUpSpeed)
    //                .SetEase(Ease.Linear)
    //                .OnComplete(() => CheckAndMoveUp(ringStack, ringBelow));  // Recursive call to check the next ring below
    //        }
    //    }
    //}
}
