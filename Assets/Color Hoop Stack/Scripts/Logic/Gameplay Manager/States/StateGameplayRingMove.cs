using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayRingMove : StateGameplay 
{
    private Ring ringMove;


    public List<RingStack> ringStackList;
    public StackRowListConfig stackRowListConfig;

    public StateGameplayRingMove(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
        
        
    }

    
    public override void OnEnter()
    {
        base.OnEnter();
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.MOVE], false);
        gameplayMgr.PushMapLevel();
        
            MoveRings(InputMgr.Instance.ringStackStart, InputMgr.Instance.ringStackEnd);
        
        
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();
        Debug.Log("ring stack    >>>>>>");
    }

    public override void OnExit()
    {
        base.OnExit();
    }
    public void MoveRings1(RingStack ringStackStart, RingStack ringStackEnd)
    {
        //ringStackEnd.canControl = false;
        int blankSlots = gameplayMgr.stackNumberMax - ringStackEnd.ringStack.Count;
        int ringNumber = 0;
        RingType moveRingType = InputMgr.Instance.ringMove.ringType;
        if (ringStackStart.ringStack.Count > 0)
        {
            ringMove = ringStackStart.ringStack.Pop();
            if (ringMove.transform.GetChild(3).gameObject.activeSelf)
            {
                ringMove.transform.SetParent(ringStackEnd.transform);
                ringStackEnd.ringStack.Push(ringMove);

                float newRingYPos = ringStackStart.transform.position.y +
                                    ringStackStart.boxCol.size.y / 2 +
                                    ringMove.boxCol.size.z / 2 + .5f;

                Vector3 newPos = new Vector3(ringStackEnd.transform.position.x, newRingYPos, ringStackEnd.transform.position.z);
                float distance = Vector3.Distance(ringMove.transform.position, newPos);

                Sequence ringMoveSeq = DOTween.Sequence();
                ringMoveSeq.PrependInterval(gameplayMgr.waitTime * (ringNumber));

                float newY = -1.123066f + ringStackEnd.boxCol.size.z / 2 + ringMove.boxCol.size.z / 2 + ringMove.boxCol.size.z * (ringStackEnd.ringStack.Count - 1);

                // Your existing movement code here...
                //move up
                ringMoveSeq.Append(
                    ringMove.transform.DOMoveY(newRingYPos, (newRingYPos - ringMove.transform.position.y) / gameplayMgr.ringUpSpeed)
                    .SetEase(Ease.Linear)
                    );
                ringMoveSeq.AppendCallback(
                   () => ringMove.transform.GetComponent<Animator>().enabled = true

               );

                //move to another stack
                //ringMoveSeq.Append(
                //    ringMove.transform.DOMove(newPos, distance / gameplayMgr.ringMoveSpeed).SetEase(Ease.Linear)
                //    );

                ringMoveSeq.Append(
                ringMove.transform.DOJump(newPos, 1, 1, distance / gameplayMgr.ringMoveSpeed, false).SetEase(Ease.Linear)
                );

                //move down
                ringMoveSeq.Append(
                    ringMove.transform.DOMoveY(newY, (newRingYPos - newY) / gameplayMgr.ringDownSpeed).SetEase(Ease.Linear)
                    
                    );
                ringMoveSeq.AppendCallback(
                    () =>
                    {
                    //ringMove.transform.GetComponent<Animator>().enabled=false;
                    gameplayMgr.CloseRingAnimator(ringMove);
                    }
                );

                ringMoveSeq.AppendCallback(() => SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.MOVE], false));

                //jump
                ringMoveSeq.Append(
                    ringMove.transform.DOJump(new Vector3(newPos.x, newY, newPos.z), gameplayMgr.ringJumpPower, 2, gameplayMgr.ringJumpTime)
                    );

                ringNumber++;
                // Additional logic or code after the ring has been moved

                if (ringStackStart.ringStack.Count > 0)
                {
                    List<Ring> ringList = new List<Ring>(ringStackStart.ringStack);

                    if (ringList.Count > 0)
                    {
                        Ring ringBelowTop = ringList[0];
                        ringBelowTop.transform.GetChild(3).gameObject.SetActive(false);
                        ringBelowTop.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }

                if (blankSlots <= ringNumber)
                {
                    ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                    Debug.Log("blankSlots " + blankSlots);
                    Debug.Log("ringnumber " + ringNumber);
                    Debug.Log("ringstack end count_ " + ringStackEnd.ringStack.Count);
                }
                else if (ringStackStart.ringStack.Count == 0)
                {
                    ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                    Debug.Log("ringstack start count " + ringStackStart.ringStack.Count);
                }
                else if (ringStackStart.ringStack.Peek().ringType != moveRingType)
                {
                    ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                    Debug.Log("ringstackstartpeek type " + ringStackStart.ringStack.Peek().ringType);
                    Debug.Log("ringstack end peektype " + ringStackEnd.ringStack.Peek().ringType);
                }
            }
           
            
        }

        stateMachine.StateChange(gameplayMgr.stateGameplayIdle);
    }
    public void MoveRings(RingStack ringStackStart, RingStack ringStackEnd)
    {
        ringStackEnd.canControl = false;
        int blankSlots = gameplayMgr.stackNumberMax - ringStackEnd.ringStack.Count;
        int ringNumber = 0;
        RingType moveRingType = InputMgr.Instance.ringMove.ringType;

        while (ringStackStart.ringStack.Count > 0)
        {
            ringMove = ringStackStart.ringStack.Pop();
            
                ringMove.transform.SetParent(ringStackEnd.transform);
                ringStackEnd.ringStack.Push(ringMove);

                float newRingYPos = ringStackStart.transform.position.y +
                    ringStackStart.boxCol.size.y / 2 +
                    ringMove.boxCol.size.z / 2 + .5f;

                Vector3 newPos = new Vector3(ringStackEnd.transform.position.x, newRingYPos, ringStackEnd.transform.position.z);
                float distance = Vector3.Distance(ringMove.transform.position, newPos);

                Sequence ringMoveSeq = DOTween.Sequence();
                ringMoveSeq.PrependInterval(gameplayMgr.waitTime * (ringNumber));

                float newY = -1.123066f + ringStackEnd.boxCol.size.z / 2 + ringMove.boxCol.size.z / 2 + ringMove.boxCol.size.z * (ringStackEnd.ringStack.Count - 1);

                //move up
                ringMoveSeq.Append(
                    ringMove.transform.DOMoveY(newRingYPos, (newRingYPos - ringMove.transform.position.y) / gameplayMgr.ringUpSpeed)
                    .SetEase(Ease.Linear)
                    );
                ringMoveSeq.AppendCallback(
                   () => ringMove.transform.GetComponent<Animator>().enabled = true

               );

                //move to another stack
                //ringMoveSeq.Append(
                //    ringMove.transform.DOMove(newPos, distance / gameplayMgr.ringMoveSpeed).SetEase(Ease.Linear)
                //    );

                ringMoveSeq.Append(
                ringMove.transform.DOJump(newPos, 1, 1, distance / gameplayMgr.ringMoveSpeed, false).SetEase(Ease.Linear)
                );

                //move down
                ringMoveSeq.Append(
                    ringMove.transform.DOMoveY(newY, (newRingYPos - newY) / gameplayMgr.ringDownSpeed).SetEase(Ease.Linear)
                    );
                ringMoveSeq.AppendCallback(
                    () =>
                    {
                    //ringMove.transform.GetComponent<Animator>().enabled=false;
                    gameplayMgr.CloseRingAnimator(ringMove);
                    }
                );

                ringMoveSeq.AppendCallback(() => SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.DROP], false));

                //jump
                ringMoveSeq.Append(
                    ringMove.transform.DOJump(new Vector3(newPos.x, newY, newPos.z), gameplayMgr.ringJumpPower, 2, gameplayMgr.ringJumpTime)
                    );

                ringNumber++;

                if (ringStackStart.ringStack.Count > 0)
                {
                    //Ring topRing = ringStackStart.ringStack.Peek();

                    // Convert the stack to a list
                    List<Ring> ringList = new List<Ring>(ringStackStart.ringStack);

                    // Check if there is a ring below the top ring
                    if (ringList.Count > 0)
                    {
                        Ring ringBelowTop = ringList[0]; // Get the ring below the top ring
                        ringBelowTop.transform.GetChild(3).gameObject.SetActive(false); // Disable Child 3 for the ring below the top ring
                        ringBelowTop.transform.GetChild(0).gameObject.SetActive(true);
                        //gameplayMgr.isLock = false;
                    }
                }

                if (blankSlots <= ringNumber)
                {
                    ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                    Debug.Log("blankSlots " + blankSlots);
                    Debug.Log("ringnumber " + ringNumber);
                    Debug.Log("ringstack end count_ " + ringStackEnd.ringStack.Count);

                    break;
                }
                if (ringStackStart.ringStack.Count == 0)
                {
                    ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                    Debug.Log("ringstack start count " + ringStackStart.ringStack.Count);
                    break;
                }
                if (ringStackStart.ringStack.Peek().ringType != moveRingType)
                {
                    ringMoveSeq.AppendCallback(() => ActiveControlStack(ringStackEnd));
                    Debug.Log("ringstackstartpeek type " + ringStackStart.ringStack.Peek().ringType);
                    Debug.Log("ringstack end peektype " + ringStackEnd.ringStack.Peek().ringType);
                    break;
                }
            
            



        }

        stateMachine.StateChange(gameplayMgr.stateGameplayIdle);
    }

    public void ActiveControlStack(RingStack ringStack)
    {
        Debug.LogError("Check Ring stack");
        if (ringStack.IsStackFullSameColor())
        {
            Debug.LogError("Check  stack full "+ringStack.IsStackFullSameColor());
            RingType stackRingType = ringStack.ringStack.Peek().ringType;
           
            gameplayMgr.StartCoroutine(RemoveRingsWithDelay(ringStack, stackRingType));
            //gameplayMgr.stackCompleteNumber++;
            //if (gameplayMgr.CheckWinState())
            //{
            //    gameplayMgr.TriggerCompleteLevelEffect(0f);

            //    stateMachine.StateChange(gameplayMgr.stateGameplayCompleteLevel);

            //}
            //else
            //{
            //    float newRingYPos = ringStack.transform.position.y + ringStack.boxCol.size.y / 2 + ringStack.boxCol.size.z / 2;
            //    Vector3 newPos = new Vector3(ringStack.transform.position.x, newRingYPos, ringStack.transform.position.z);
            //    SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.FULL_STACK], false);


            //    //     GameObject particleGO = PoolerMgr.Instance.VFXCompletePooler.GetNextPS();
            //    //    particleGO.transform.position = newPos;
            //    //capenabled
            //    //gameplayMgr.CapEnabled(ringStack);
            //    //gameplayMgr.PlayDustParticle(ringStack);
            //    gameplayMgr.PlayConfetti(ringStack);
            //    //  Transform cap= ringStack.transform.GetChild(2);
            //    // cap.gameObject.SetActive(true);
            //}
        }
        else
        {
            ringStack.canControl = true;
            Transform cap= ringStack.transform.GetChild(2);
                cap.gameObject.SetActive(false);
        }
        if (!ringStack.gameObject.activeSelf)
        {
            // Rearrange the positions of the remaining active ring stacks
            gameplayMgr.RearrangeActiveRingStacks();
        }

    }

    private IEnumerator RemoveRingsWithDelay(RingStack ringStack, RingType stackRingType)
    {
        List<Ring> ringsToRemove = new List<Ring>(ringStack.ringStack);
        ringsToRemove.Reverse();  // Reverse the order to start removing from the bottom

        int ringCount = ringsToRemove.Count;

        //ringStackList = new List<RingStack>();
        for (int i = 0; i < ringCount; i++)
        {
            Ring removedRing = ringsToRemove[i];
            ringStack.ringStack.Pop();
            DeactivateRing(removedRing);
            // Move the rings above the removed ring down to fill the gap
            for (int j = i + 1; j < ringCount; j++)
            {
                Ring ringAbove = ringsToRemove[j];
                Transform particle = ringAbove.transform.GetChild(2).GetChild(0);
                particle.gameObject.GetComponent<ParticleSystem>().Play();
                float newY = ringAbove.transform.position.y - ringAbove.boxCol.size.z; // Adjust as needed
                Vector3 newPosition = new Vector3(ringAbove.transform.position.x, newY, ringAbove.transform.position.z);
                ringAbove.transform.DOMoveY(newY, 0.15f);  // Adjust the duration as needed
            }
            
            yield return new WaitForSeconds(0.15f); // Adjust the delay as needed
            ringStack.canControl = true;
            

        }
       
       
       

        gameplayMgr.stackCompleteNumber++;
        if (gameplayMgr.CheckWinState())
        {
            gameplayMgr.TriggerCompleteLevelEffect(0f);
            stateMachine.StateChange(gameplayMgr.stateGameplayCompleteLevel);
        }
        else
        {
            
            float newRingYPos = ringStack.transform.position.y + ringStack.boxCol.size.y / 2 + ringStack.boxCol.size.z / 2;
            Vector3 newPos = new Vector3(ringStack.transform.position.x, newRingYPos, ringStack.transform.position.z);
            SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.FULL_STACK], false);
            gameplayMgr.PlayConfetti(ringStack);
            ringStack.transform.gameObject.SetActive(false);

            gameplayMgr.RearrangeActiveRingStacks();


        }

    }





    private void DeactivateRing(Ring ring)
    {
        // Deactivate or set the ring to inactive state
        Transform particle = ring.transform.GetChild(2).GetChild(0);
        particle.gameObject.GetComponent<ParticleSystem>().Play();
        ring.gameObject.SetActive(false);
        
        //ring.transform.SetParent(null);
        
    }
            
}
