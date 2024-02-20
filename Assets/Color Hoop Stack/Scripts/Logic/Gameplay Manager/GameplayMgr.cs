#if UNITY_EDITOR
using DG.Tweening;
using Sirenix.OdinInspector;
#endif
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayMgr : Singleton<GameplayMgr>
{
    [Header("Game Design Datas")]
    public RingColorConfig ringColorConfig;
    public LevelListConfig levelListConfig;
    public StackRowListConfig stackRowListConfig;
    public int stackNumberMax = 4;

    [Header("Ring Speed")]
    public float ringUpSpeed = 0.002f;
    public float ringMoveSpeed = 0.5f;
    public float ringDownSpeed = 0.2f;
    public float ringJumpTime = 0.05f;
    public float ringJumpPower = 0.05f;
    public float waitTime = 0.05f;

    [Header("Positions")]
    public Vector2 ringStackDistance;

    [Header("Debug Infos")]
    public int currentLevel = 0;

    [HideInInspector] public StateGameplay stateGameplayInit;
    [HideInInspector] public StateGameplay stateGameplayIdle;
    [HideInInspector] public StateGameplayRingUp stateGameplayRingUp;
    [HideInInspector] public StateGameplay stateGameplayRingReady;
    [HideInInspector] public StateGameplay stateGameplayRingMove;
    [HideInInspector] public StateGameplay stateGameplayRingDown;
    [HideInInspector] public StateGameplay stateGameplayCompleteLevel;
    [HideInInspector] public StateGameplay stateGameplayEnd;
    [HideInInspector] public StateGameplay stateGameplayAddStack;

    [HideInInspector] public StateMachine stateMachine;

    [HideInInspector] public List<RingStack> ringStackList;
    [HideInInspector] public bool touched = false;
    [HideInInspector] public bool readyToTouch = false;
    [HideInInspector] public bool firstLoad = true;
    [HideInInspector] public Stack<MapData> mapDataStack;

    public int ringTypeNumber = 0;
    public int stackCompleteNumber = 0;

    [HideInInspector] public int undoTime = 5;
    [HideInInspector] public bool enabledTutorial = true;
    bool isCapActivated;
    bool IsActive = false;
    //public ParticleSystem dustParticle;
    public GameObject simpleLevel;
    public GameObject specialLevel;
    private void Awake()
    {

        mapDataStack = new Stack<MapData>();
        ringStackList = new List<RingStack>();

        stateMachine = new StateMachine();

        stateGameplayInit = new StateGameplayInit(this, stateMachine);
        stateGameplayIdle = new StateGameplayIdle(this, stateMachine);
        stateGameplayRingUp = new StateGameplayRingUp(this, stateMachine);
        stateGameplayRingReady = new StateGameplayRingReady(this, stateMachine);
        stateGameplayRingMove = new StateGameplayRingMove(this, stateMachine);
        stateGameplayRingDown = new StateGameplayRingDown(this, stateMachine);
        stateGameplayCompleteLevel = new StateGameplayCompleteLevel(this, stateMachine);
        stateGameplayEnd = new StateGameplayEnd(this, stateMachine);
        stateGameplayAddStack = new StateGameplayAddStack(this, stateMachine);
        EarnReward();
       
    }
    
    public void Start() {
        currentLevel = PlayerPrefs.GetInt("Levelnumber");
       
        DeactivateCapForAllRingStacks();
        DOTween.Init();
        //  foreach (RingStack ringStack in ringStackList)
        // {

        //         Transform cap= ringStack.transform.GetChild(2);
        //         cap.gameObject.SetActive(false);
        // }
       
    }

    public void Init()
    {
        FileHandler fileHandler = new FileHandler();
        if (!fileHandler.IsFileExist(fileHandler.settingFilePath))
        {
            fileHandler.SaveSettingDataDefault();
        }
        fileHandler.ReadSettingData();
        if (fileHandler.IsFileExist(fileHandler.levelFilePath))
        {
            fileHandler.LoadLevelData();
        }
        else
        {
            fileHandler.SaveLevelDataDefault();
        }

        stateMachine.StateChange(stateGameplayInit);
        
    }

    private void Update()
    {
        //EnableLockINRing();
        stateMachine.StateHandleInput();
        stateMachine.StateLogicUpdate();
        EnableStackChildNail();
        int currentLevel = GameplayMgr.Instance.currentLevel;
        if (currentLevel!=0 && currentLevel%5==0 && currentLevel<52)
        {
            simpleLevel.SetActive(false);
            specialLevel.SetActive(true);
        }
        else
        {
            simpleLevel.SetActive(true);
            specialLevel.SetActive(false);
        }

    }
    public bool isLock;
    public void EnableLockINRing()
    {
        foreach (RingStack ringStack in ringStackList)
        {
            int ringCount = ringStack.ringStack.Count;

            if (ringCount > 0)
            {
                Debug.Log("Ring Stack count is.... " + ringCount);
                Ring topRing = ringStack.ringStack.Peek();
                foreach (Ring ring in ringStack.ringStack)
                {
                    if (ring == topRing)
                    {
                        // Skip enabling Child 3 for the top ring
                        continue;
                    }

                    Transform childObject3 = ring.transform.GetChild(3);
                    Transform child0 = ring.transform.GetChild(0);
                    childObject3.gameObject.SetActive(true);
                    isLock = true;
                    child0.gameObject.SetActive(false);

                }

            }
            else
            {
                Debug.Log($"Ring Stack {ringStack.number} is empty.");
                Debug.Log("Ring Stack count is "+ ringCount);
                
            }
            
        }

    }



    public void EnableStackChildNail()
    {

        int currentLevel = PlayerPrefs.GetInt("Levelnumber");
        if (currentLevel != 0 && currentLevel % 5 == 0 && currentLevel<52)
        {
            // Activate the child object in each ring stack
            foreach (RingStack ringStack in ringStackList)
            {
                //int ringCount = ringStack.ringStack.Count;
                //Debug.Log($"Ring Stack {ringStack.number} has {ringCount} rings.");
                Transform childObject = ringStack.transform.GetChild(13);
                childObject.gameObject.SetActive(true);
                Transform childObject1 = ringStack.transform.GetChild(14);
                childObject1.gameObject.SetActive(true);
            }

            // Set maxStackInRow to 8
            stackNumberMax = 8;
            
        }
        else
        {
            // Otherwise, use the original calculation based on the level
           int ringStackPerRow = stackRowListConfig.stackRowList[ringStackList.Count].maxStackInRow;
            foreach (RingStack ringStack in ringStackList)
            {
                Transform childObject = ringStack.transform.GetChild(13);
                childObject.gameObject.SetActive(false);
                Transform childObject1 = ringStack.transform.GetChild(14);
                childObject1.gameObject.SetActive(false);
            }
            stackNumberMax = 4;
        }
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void SetUpRingStacksPosition(int ringStackPerRow, int ringStackNumber)
    {
        Vector2 ringStackDistance = this.ringStackDistance;
        if ((ringStackNumber == 2) || (ringStackPerRow == 2))
        {
            ringStackDistance.x *= 2f;
        }
        else if (ringStackNumber == 3)
        {
            ringStackDistance.x *= 1.5f;
        }
        int ringStackPerColumn = (int)Mathf.Ceil((float)ringStackNumber/(float)ringStackPerRow);
        int stackLeft = ringStackNumber;
        Vector3 startPos = new Vector3(
            - (float)(ringStackPerRow - 1) / 2 * ringStackDistance.x, 
            0f, 
            (float)(ringStackPerColumn - 1) / 2 * ringStackDistance.y);

        int ringStackListCurrent = 0;

        for (int i = 0; i < ringStackPerColumn; i++)
        {
            if (stackLeft < ringStackPerRow)
                startPos.x = -(float)(stackLeft - 1) / 2 * ringStackDistance.x;
            for (int j = 0; j < ringStackPerRow; j++)
            {
                ringStackList[ringStackListCurrent].transform.position = startPos + new Vector3(j * ringStackDistance.x, 0f, i * -ringStackDistance.y);
                ringStackListCurrent++;
                stackLeft--;
                if (ringStackListCurrent >= ringStackList.Count)
                    break;
            }
            if (ringStackListCurrent >= ringStackList.Count)
                break;
            //float stackSpacing = 1.2f;
            //startPos.x -= stackSpacing;
        }

        EventDispatcher.Instance.PostEvent(EventID.ON_RING_STACK_NUMBER_CHANGE);
        Debug.Log("ringstack rows  "+ ringStackPerColumn);
        Debug.Log("ringstack perrows  "+ ringStackPerRow);
        Debug.Log("ringstack number  "+ ringStackNumber);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void AddRingStack()
    {
        
        if (ringStackList.Count < 20)
        {
            GameObject newRingStack = PoolerMgr.Instance.ringStackPooler.GetNextRingStack();
            RingStack newRingStackComp = newRingStack.GetComponent<RingStack>();
            ringStackList.Add(newRingStackComp);
            newRingStackComp.number = ringStackList.Count - 1;
            int ringStackNumber = ringStackList.Count;
            int ringStackPerRow = stackRowListConfig.stackRowList[ringStackList.Count].maxStackInRow;
            SetUpRingStacksPosition(ringStackPerRow, ringStackNumber);
        }
        else
        {
            Utils.Common.Log("Reached ring stack number limit!");
           
        }
        if (ringStackList.Count == 20)
        {
            MoreStackButton.Instance.riseDisableButtonFlag = true;
        }
    }

    public void ReArrangeStack()
    {
       
        int ringStackNumber = ringStackList.Count;
        int ringStackPerRow = stackRowListConfig.stackRowList[ringStackList.Count].maxStackInRow;
        SetUpRingStacksPosition(ringStackPerRow, ringStackNumber);

    }

    public void EarnReward()
    {
        //Abdul Rehman
        if (CASAds.instance.rewardedTypeAd == CASAds.RewardType.RING_STACK)
        {
            stateMachine.StateChange(stateGameplayAddStack);
        }
        else if (CASAds.instance.rewardedTypeAd == CASAds.RewardType.UNDO)
        {
            UndoLevel();
            undoTime--;
        }
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void GoToLevel(int level)
    {
         DeactivateCapForAllRingStacks();
        currentLevel = level;
       
        stateMachine.StateChange(stateGameplayEnd);
        stateMachine.StateChange(stateGameplayInit);
        PlayerPrefs.SetInt("Levelnumber",currentLevel);
    }
    public void LevelGo(){
         DeactivateCapForAllRingStacks();
        GoToLevel( PlayerPrefs.GetInt("Levelnumber"));
    }
    public void GoToLevel(int level, float goAfterSeconds)
    {
         DeactivateCapForAllRingStacks();
        StartCoroutine(GoToLevelAfter(level, goAfterSeconds));
    }

    private IEnumerator GoToLevelAfter(int level, float goAfterSeconds)
    {
        yield return new WaitForSeconds(goAfterSeconds);
        GoToLevel(level);
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void GoNextLevel()
    {
      isCapActivated=false;
        currentLevel++;
        stateMachine.StateChange(stateGameplayEnd);
        stateMachine.StateChange(stateGameplayInit);
         Debug.Log("Interstitial Ad is runnig ");
       
       
    }

#if UNITY_EDITOR
    [Button]
#endif
    public void GoPreviousLevel()
    {
        currentLevel--;
        stateMachine.StateChange(stateGameplayEnd);
        stateMachine.StateChange(stateGameplayInit);
    }

     public void DeactivateCapForAllRingStacks()
    {
        PlayerPrefs.SetInt("Cap",0);
        if (isCapActivated)  // Check if the cap is activated before deactivating
        {
            foreach (RingStack ringStack in ringStackList)
            {
                Transform cap = ringStack.transform.GetChild(2);
                cap.gameObject.SetActive(false);
                isCapActivated=false;
                 PlayerPrefs.SetInt("Cap",0);
            }
        }
    }
    public void disabledCap(){
        
    }
    public void CapEnabled(RingStack ringStack){
       
        float targetPos=0.58f;
         Transform cap= ringStack.transform.GetChild(2);
         cap.gameObject.SetActive(true);
        //abdulrehman
        cap.DOMoveY(targetPos, .2f).SetEase(Ease.InCirc);
        
        PlayDustParticle(ringStack);
         PlayerPrefs.SetInt("Cap",1);
         Debug.Log("Cap bool is :"+ PlayerPrefs.GetInt("Cap"));

    }
    public void PlayDustParticle(RingStack ringStack){
          Transform particle= ringStack.transform.GetChild(11).GetChild(0);
          particle.gameObject.GetComponent<ParticleSystem>().Play();
       // dustParticle.Play();
    }
    
    public void TriggerCompleteLevelEffect()
    {
        float effectYPos = ringStackList[0].transform.position.y +
            ringStackList[0].boxCol.size.y / 2 +
            0.15f;
        foreach (RingStack ringStack in ringStackList)
        {
            if (ringStack.ringStack.Count == stackNumberMax)
            {
                // Transform cap= ringStack.transform.GetChild(2);
                // cap.gameObject.SetActive(true);
                //CapEnabled(ringStack);
                float targetPos=0.58f;
                //capenable
                //Transform cap= ringStack.transform.GetChild(2);
                //        cap.gameObject.SetActive(true);
                //abdulrehman
                //cap.DOMoveY(targetPos,0.2f).SetEase(Ease.InCirc);
                isCapActivated=true;
                GameObject particleGO = PoolerMgr.Instance.VFXCompletePooler.GetNextPS();
                particleGO.transform.position = new Vector3(ringStack.transform.position.x, effectYPos, ringStack.transform.position.z);
            }
        }
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.FULL_ALL], false);
         
    }
    public void CloseRingAnimator(Ring ring){
      StartCoroutine(CloseAnimatorWait(ring));
    }
    public IEnumerator CloseAnimatorWait(Ring ring){
        yield return new WaitForSeconds(0.02f);
        ring.GetComponent<Animator>().enabled=false;
       // ring.transform.rotation=Quaternion.Euler(90,0,0);
    }
    public void PlayConfetti(RingStack ringStack){
       StartCoroutine(ConfettiWait(ringStack));
    }
    public IEnumerator ConfettiWait(RingStack ringStack){
       yield return new WaitForSeconds(0.5f);
        float newRingYPos = ringStack.transform.position.y + ringStack.boxCol.size.y / 2 + ringStack.boxCol.size.z / 2;
                Vector3 newPos = new Vector3(ringStack.transform.position.x, newRingYPos, ringStack.transform.position.z);
        GameObject particleGO = PoolerMgr.Instance.VFXCompletePooler.GetNextPS();
         particleGO.transform.position = newPos;
        
    }

    public void TriggerCompleteLevelEffect(float afterSeconds)
    {
        StartCoroutine(TriggerCompleteLevelEffectAfter(afterSeconds));
    }

    private IEnumerator TriggerCompleteLevelEffectAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        TriggerCompleteLevelEffect();
    }

    public void GetRingTypeNumber(int level)
    {
        LevelConfig levelConfig = levelListConfig.levelList[level];
        List<RingType> ringTypeList = new List<RingType>();

        foreach(RingStackList ringStackList in levelConfig.ringStackList)
        {
            foreach(RingType ringType in ringStackList.ringList)
            {
                bool isDuplicated = false;
                if (ringType == RingType.NONE)
                {
                    continue;
                }
                if (ringTypeList.Count == 0)
                {
                    ringTypeList.Add(ringType);
                    continue;
                }
                foreach(RingType currentRingType in ringTypeList)
                {
                    if (currentRingType == ringType)
                    {
                        isDuplicated = true;
                        break;
                    }
                }
                if (!isDuplicated)
                {
                    ringTypeList.Add(ringType);
                }
            }
        }

        ringTypeNumber = ringTypeList.Count;
    }

    public void LoadAds(float seconds)
    {
        StartCoroutine(LoadAdsAfter(seconds));
    }

    public IEnumerator LoadAdsAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //nadeem
        //abdul Rehman
        CASAds.instance.ShowBanner(CAS.AdPosition.BottomCenter);
        //GoogleAdMobController.Instance.RequestBannerAd();
        //GoogleAdMobController.Instance.RequestAndLoadInterstitialAd();
        //GoogleAdMobController.Instance.RequestAndLoadRewardedAd();
    }

    public void UndoLevel()
    {
        if (mapDataStack.Count > 0)
        {
            LoadLevelMapData(mapDataStack.Pop());
            stateMachine.StateChange(stateGameplayIdle);

            if (undoTime > 0)
            {
                undoTime--;
                EventDispatcher.Instance.PostEvent(EventID.ON_CHANGED_UNDO_TIME);
                return;
            }
            else
            {
                undoTime = 5;
                UndoButton.instance.watchAd.SetActive(false);
                UndoButton.instance.UndoImage.SetActive(true);
                EventDispatcher.Instance.PostEvent(EventID.ON_CHANGED_UNDO_TIME);
                return;
            }
        }
        else
        {
            Utils.Common.Log("Level map data stack don't archive any data!");
        }
    }
    
    public void LoadLevelMapData(MapData mapData)
    {
        foreach (RingStack ringStack in ringStackList)
        {
            while (ringStack.ringStack.Count > 0)
            {
                Ring ring = ringStack.ringStack.Pop();
                PoolerMgr.Instance.ringPooler.ReturnPooledObject(ring.gameObject);
            }
            ringStack.ringStack.Clear();
            PoolerMgr.Instance.ringStackPooler.ReturnPooledObject(ringStack.gameObject);
        }

        ringStackList.Clear();

        stackCompleteNumber = mapData.StackCompleteNumber;
        int ringStackNumber = mapData.RingStackNumber;
        int ringStackPerRow = stackRowListConfig.stackRowList[ringStackNumber].maxStackInRow;

        for (int i = 0; i < ringStackNumber; i++)
        {
            GameObject newRingStack = PoolerMgr.Instance.ringStackPooler.GetNextRingStack();
            RingStack newRingStackComp = newRingStack.GetComponent<RingStack>();
            ringStackList.Add(newRingStackComp);
        }
        



        SetUpRingStacksPosition(ringStackPerRow, ringStackNumber);

        for (int i = 0; i < ringStackList.Count; i++)
        {
            RingStack ringStack = ringStackList[i];
            ringStack.number = mapData.ListRingStack[i].number;
            ringStack.canControl = mapData.ListRingStack[i].canControl;

            for (int j = mapData.ListRingStack[i].ringList.Count - 1; j >= 0; j--)
            {
                if (!(mapData.ListRingStack[i].ringList[j] == RingType.NONE))
                {
                    GameObject newRing = PoolerMgr.Instance.ringPooler.GetNextRing(mapData.ListRingStack[i].ringList[j]);
                    Ring newRingComp = newRing.GetComponent<Ring>();
                    newRing.transform.position = new Vector3(
                        ringStack.transform.position.x,
                        -1.123066f + ringStack.boxCol.size.z / 2 + newRingComp.boxCol.size.z / 2 + newRingComp.boxCol.size.z * (mapData.ListRingStack[i].ringList.Count - 1 - j),
                        ringStack.transform.position.z
                        );

                    ringStack.AddNewRing(newRingComp);
                    
                }
            }
        }
    }

    public void PushMapLevel()
    {
        mapDataStack.Push(new MapData(ringStackList, stackCompleteNumber, ringStackList.Count));
    }

    private void OnApplicationQuit()
    {
        FileHandler fileHandler = new FileHandler();
        fileHandler.SaveLevelData();
    }

    public bool CheckWinState()
    {
        if (stackCompleteNumber == ringTypeNumber)
        {
            return true;
        }

        return false;
    }
    public void RearrangeActiveRingStacks()
    {
        List<RingStack> activeRingStacks = ringStackList.FindAll(stack => stack.gameObject.activeSelf);
        for (int i = ringStackList.Count - 1; i >= 0; i--)
        {
            if (!ringStackList[i].gameObject.activeSelf)
            {
                ringStackList.RemoveAt(i);
            }
        }

        int ringStackPerRow = stackRowListConfig.stackRowList[activeRingStacks.Count].maxStackInRow;
        Utils.Common.Log("RingStack pr row before "+ringStackPerRow);
        int ringStackPerColumn = (int)Mathf.Ceil((float)activeRingStacks.Count / (float)ringStackPerRow);
        SetUpRingStacksPosition(ringStackPerRow, activeRingStacks.Count);
        Utils.Common.Log("RingStack pr row after " + ringStackPerRow);
        Utils.Common.Log("This is working");
    }
}
