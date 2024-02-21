#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMgr : Singleton<InputMgr>
{
    [HideInInspector] public Ring ringMove;
    [HideInInspector] public RingStack ringStackStart;
    [HideInInspector] public RingStack ringStackEnd;
    [HideInInspector] public bool isUndoMove = false;

    private void Awake()
    {
        Instance = this;
    }
    public void HandleTap(RingStack ringStackTap)
    {

        if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayIdle)
        {
            ringStackStart = ringStackTap;
            if (ringStackStart.ringStack.Count > 0 )
            {   
                ringMove = ringStackStart.ringStack.Peek();
                //command up
                Command newMove = new CommandRingUp(ringStackStart, ringMove, null, null);
                newMove.Execute();
                return;
            }
        }
        else if (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayRingReady)
        {
            ringStackEnd = ringStackTap;
            Debug.Log("ringStackStart.number  " + ringStackStart.number);
            Debug.Log("ringStackEnd.number  "+ ringStackEnd.number);
            if (ringStackEnd.number == ringStackStart.number)
            {
                //command down
                if (GameplayMgr.Instance.currentLevel != 0)
                {
                    Command newMove = new CommandRingDown(ringStackEnd, ringMove);
                    newMove.Execute();
                }
            }
            else
            {
                if (ringStackEnd.ringStack.Count > 0)
                {
                    if (ringStackEnd.canControl)
                    {
                        if (ringStackEnd.ringStack.Peek().ringType != ringMove.ringType)
                        {
                            //Command up
                            RingStack ringStackReady = ringStackStart;
                            Ring ringReady = ringStackReady.ringStack.Peek();
                            ringStackStart = ringStackEnd;
                            ringMove = ringStackStart.ringStack.Peek();

                            Command newMove = new CommandRingUp(ringStackStart, ringMove, ringReady, ringStackReady);

                            newMove.Execute();
                            return;
                        }
                        else if (ringStackEnd.ringStack.Count == GameplayMgr.Instance.stackNumberMax)
                        {
                            //Command up
                            RingStack ringStackReady = ringStackStart;
                            Ring ringReady = ringStackReady.ringStack.Peek();
                           
                            ringStackStart = ringStackEnd;
                            ringMove = ringStackStart.ringStack.Peek();

                            Command newMove = new CommandRingUp(ringStackStart, ringMove, ringReady, ringStackReady);
                            newMove.Execute();

                            return;
                        }
                        else if (ringStackEnd.ringStack.Peek().ringType == ringMove.ringType)
                        {
                            //Command move
                            Command newMove = new CommandRingMove(ringStackStart, ringStackEnd);
                            newMove.Execute();
                            return;
                        }
                    }
                }
                else
                {
                    //Command move
                    Command newMove = new CommandRingMove(ringStackStart, ringStackEnd);
                    newMove.Execute();
                    return;
                }
            }

            return;
        }

    }

}
