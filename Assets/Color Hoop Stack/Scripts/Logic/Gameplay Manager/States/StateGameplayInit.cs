using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateGameplayInit : StateGameplay
{
    public StateGameplayInit(GameplayMgr _gameplayMgr, StateMachine _stateMachine) : base(_gameplayMgr, _stateMachine)
    {
        stateMachine = _stateMachine;
        gameplayMgr = _gameplayMgr;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        EventDispatcher.Instance.PostEvent(EventID.ON_FAILED_LOAD_REWARDED_AD);

        gameplayMgr.LoadAds(0.5f);
        InitLevel();
    }

    public override void OnHandleInput()
    {
        base.OnHandleInput();
    }

    public override void OnLogicUpdate()
    {
        base.OnLogicUpdate();

        stateMachine.StateChange(gameplayMgr.stateGameplayIdle);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public void InitLevel()
    {
        if (gameplayMgr.mapDataStack.Count == 0)
        {
            InitLevelDefault();
        }

        if ((gameplayMgr.currentLevel > 2) /*&& (gameplayMgr.currentLevel != 9)*/)
        {
            if (gameplayMgr.enabledTutorial)
            {
                gameplayMgr.enabledTutorial = false;
            }
        }
        else
        {
            if (!gameplayMgr.enabledTutorial)
            {
                gameplayMgr.enabledTutorial = true;
            }
        }

        if (gameplayMgr.enabledTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.ON_ENABLED_TUTORIAL);
            EventDispatcher.Instance.PostEvent(EventID.ON_RESET_TUTORIAL_TEXT);
            if (gameplayMgr.currentLevel == 1)
            {
                EventDispatcher.Instance.PostEvent(EventID.ON_CHANGE_TUTORIAL_TEXT);
            }
        }
        else
        {
            EventDispatcher.Instance.PostEvent(EventID.ON_DISABLED_TUTORIAL);
        }

        EventDispatcher.Instance.PostEvent(EventID.ON_INIT_LEVEL);

        FileHandler fileHandler = new FileHandler();
        fileHandler.SaveLevelData();
    }

    public void InitLevelDefault()
    {
        int ringStackNumber = gameplayMgr.levelListConfig.levelList[gameplayMgr.currentLevel].ringStackList.Count;
        int ringStackPerRow = gameplayMgr.stackRowListConfig.stackRowList[ringStackNumber].maxStackInRow;
        gameplayMgr.stackCompleteNumber = 0;
        gameplayMgr.GetRingTypeNumber(gameplayMgr.currentLevel);
        

        for (int i = 0; i < ringStackNumber; i++)
        {
            GameObject newRingStack = PoolerMgr.Instance.ringStackPooler.GetNextRingStack();
            RingStack newRingStackComp = newRingStack.GetComponent<RingStack>();
            gameplayMgr.ringStackList.Add(newRingStackComp);
            newRingStackComp.number = i;
        }

        gameplayMgr.SetUpRingStacksPosition(ringStackPerRow, ringStackNumber);

        for (int i = 0; i < gameplayMgr.ringStackList.Count; i++)
        {
            RingStackList ringStackList = gameplayMgr.levelListConfig.levelList[gameplayMgr.currentLevel].ringStackList[i];
            RingStack ringStack = gameplayMgr.ringStackList[i];
            for (int j = 0; j < ringStackList.ringList.Count; j++)
            {
                if (!(ringStackList.ringList[j] == RingType.NONE))
                {
                    GameObject newRing = PoolerMgr.Instance.ringPooler.GetNextRing(ringStackList.ringList[j]);
                    Ring newRingComp = newRing.GetComponent<Ring>();
                    newRing.transform.position = new Vector3(
                        ringStack.transform.position.x,
                        -1.123066f + ringStack.boxCol.size.z/2 + newRingComp.boxCol.size.z/2 + newRingComp.boxCol.size.z * j,
                        ringStack.transform.position.z
                        );

                    ringStack.AddNewRing(newRingComp);
                }
            }
        }

        //level 1 setup for tutorial
        if (gameplayMgr.currentLevel == 0)
        {
            gameplayMgr.ringStackList[1].canControl = false;
        }
    }
}
