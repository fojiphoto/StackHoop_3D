using System;
using System.Collections;
using UnityEngine;
using System.Net;
//using Firebase.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using SolarEngine;


public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    private const string MaxSdkKey = "TecxGz9HeSi_iQ5CGuTRWRFTDHtMM_TKk19s1icUkcuQLyS1FNVZ8Kp_McP2XmsoOIqrW7KHyl1Q5itk7RJVnT";//FS

    [SerializeField] public string InterstitialAdUnitIdGeneric = "e8e8ff6ef4a25289";
    [SerializeField] private string RewardedAdUnitIdGeneric = "0e85edf4221cc795";
    [SerializeField] private string BannerAdUnitId = "db18ff90af1f4bdb";
    [SerializeField] private string AppOpenAdUnitId = "7673f85ee8b3b89f";
    [SerializeField] private string RecID = "bb079c8a4a059190";

    public bool isBannerShowing = false;
    public bool isMRecShowing = false;
    public bool _isAdLoaded;
    private bool _isSdkInitialized;
    private bool _isAdRemoved;

    float lastTime = 0;
    public float rVShownTime = 0;



      public enum RewardType { NONE, RING_STACK, UNDO };


      [HideInInspector]
     public RewardType rewardedTypeAd = RewardType.NONE;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);  
        
    }

    private MaxSdkBase.SdkConfiguration _sdkConfiguration;
    private bool gdprChecking = false;

    void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            _sdkConfiguration = sdkConfiguration;
            if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies || Application.isEditor)
            {
                MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
                Debug.Log("CountryCode"+sdkConfiguration.CountryCode);
                // Show user consent dialog
                print("FS Max show gdpr dialog");
                print("its a gdpr zone");
                

#if UNITY_IO
                if (Preferences.Instance.Gdpr_accepted == 1)
                {
                    ShowPopUp();
                } 
                else
                {
                    Preferences.Instance.Gdpr_accepted = 1;
                    PlayerPrefs.Save();
                    ShowPopUp();
                    
                }         
#else
                if (Gdpr_accepted == 1)
                {
                    ShowPopUp();
                } 
                else
                {
                    ShowPopUp();
                    //gdprChecking = true;
                    //var gdprPrefab = Resources.Load("GDPR/GDPRCanvasSmart") as GameObject;
                    //Instantiate(gdprPrefab);
                    //Time.timeScale = 0;
                }
#endif
            }
            else if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.DoesNotApply)
            {
                // No need to show consent dialog, proceed with initialization
                print("FS Max gdpr concent does not apply, Loading next scene");
                Gdpr_accepted = 1;
                PlayerPrefs.Save();
                ShowPopUp();
            }
            else
            {
                // Consent dialog state is unknown. Proceed with initialization, but check if the consent
                // dialog should be shown on the next application initialization
                print("GDPR Consent dialog state is unknown, but will check later , Loading next scene");
                ShowPopUp();
            }
        };

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            Debug.Log("MAX SDK Initialized");
            _isSdkInitialized = true; 
            //MaxSdk.ShowMediationDebugger();
        };
        
        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        
    }

    


    public void GDPRPopupAccepted()
    {
        Time.timeScale = 1;
        Gdpr_accepted = 1;
        PlayerPrefs.Save();
        gdprChecking = false;
        ShowPopUp();
    }



    private void ShowPopUp()
    {
#if UNITY_IOS
        AppTrackingTransparency.OnAuthorizationRequestDone += OnAuthorizationRequestDone;

        var currentStatus = AppTrackingTransparency.TrackingAuthorizationStatus;
        Debug.Log($"Current authorization status: {currentStatus.ToString()}");
        if (currentStatus != AppTrackingTransparency.AuthorizationStatus.AUTHORIZED)
        {
            Debug.Log("Requesting authorization...");
            AppTrackingTransparency.RequestTrackingAuthorization();
        }
#endif

        if (RemoveAds == 1)
        {
            _isAdRemoved = true;
        }
        
        MaxSdk.SetHasUserConsent(true);
        //MaxSdk.SetIsAgeRestrictedUser(false);
        MaxSdk.SetDoNotSell(false);
        
        InitializeInterstitialAds();
        InitializeRewardedAds();
       InitializeAppOpenAds();
       InitializeBannerAds();
        InitializePaidEvents();
        InitializeClickedEvents();
       InitializeMRecAds();
        LoadApOpen();

        Debug.Log("StatLoading");
        //SceneManager.LoadScene("Scenes/singletonScene");
        
    }
#if UNITY_IOS
    private void OnAuthorizationRequestDone(AppTrackingTransparency.AuthorizationStatus status)
    {
        switch(status)
        {
            case AppTrackingTransparency.AuthorizationStatus.NOT_DETERMINED:
                Debug.Log("AuthorizationStatus: NOT_DETERMINED");
                break;
            case AppTrackingTransparency.AuthorizationStatus.RESTRICTED:
                Debug.Log("AuthorizationStatus: RESTRICTED");
                break;
            case AppTrackingTransparency.AuthorizationStatus.DENIED:
                Debug.Log("AuthorizationStatus: DENIED");
                Preferences.Instance.Interstitial_frequency = 15;

                AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(false);

                break;
            case AppTrackingTransparency.AuthorizationStatus.AUTHORIZED:
                Debug.Log("AuthorizationStatus: AUTHORIZED");
                AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
                break;
        }
    }
#endif

    #region MREC Ad Methods

    private void InitializeMRecAds()
    {
        // Attach Callbacks
        MaxSdkCallbacks.MRec.OnAdLoadedEvent += OnMRecAdLoadedEvent;
        MaxSdkCallbacks.MRec.OnAdLoadFailedEvent += OnMRecAdFailedEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnMRecAdRevenuePaidEvent;

        // MRECs are automatically sized to 300x250.
        MaxSdk.CreateMRec(RecID, MaxSdkBase.AdViewPosition.TopCenter);
    }

    public void ShowMRec()
    {
        if (!isMRecShowing)
        {
            MaxSdk.ShowMRec(RecID);
            isMRecShowing = true; // MREC is now showing
        }
    }
    public void HideMRec()
    {
        if (isMRecShowing)
        {
            MaxSdk.HideMRec(RecID);
            isMRecShowing = false; // MREC is now hidden
        }
    }
    private void ToggleMRecVisibility()
    {
        //if (!isMRecShowing)
        //{
        //    MaxSdk.ShowMRec(RecID);
        //}
        //else
        //{
        //    MaxSdk.HideMRec(RecID);
        //}

        //isMRecShowing = !isMRecShowing;
    }

    private void OnMRecAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // MRec ad is ready to be shown.
        // If you have already called MaxSdk.ShowMRec(MRecAdUnitId) it will automatically be shown on the next MRec refresh.
        Debug.Log("MRec ad loaded");

        try
        {
            //FirebaseInitialize.instance.LogEvent1("MREC_Loaded");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }


    private void OnMRecAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // MRec ad failed to load. MAX will automatically try loading a new ad internally.
        Debug.Log("MRec ad failed to load with error code: " + errorInfo.Code);
    }

    //private void OnMRecAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    //{
    //    Debug.Log("MRec ad clicked");

    //    try
    //    {
    //       // FirebaseInitialize.instance.LogEvent1("MREC_Clicked");
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.Log(ex.Message);
    //    }
    //}

    private void OnMRecAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        Debug.Log("OnAdRevenuePaidEvent");
        double revenue = impressionData.Revenue;

        //AppImpressionAttributes impressionAttributes = new AppImpressionAttributes();
        //impressionAttributes.ad_platform = impressionData.NetworkName;
        //impressionAttributes.ad_id = adUnitId;
        //impressionAttributes.ad_type = 3;
        //impressionAttributes.ad_ecpm = impressionData.Revenue * 1000.00;
        //impressionAttributes.currency_type = "USD";
        //impressionAttributes.mediation_platform = "MAX";//Please input the mediation platform you're using.
        //impressionAttributes.is_rendered = true;
        //SolarEngine.Analytics.trackIAI(impressionAttributes);
    }

    #endregion

    // public void CheckButtonsStatus()
    // {
    //
    //     if (_isAdLoaded)
    //     {
    //         for (var index = 0; index < allRewardButtons.Count; index++)
    //         {
    //             allRewardButtons[index].EnableMe();
    //         }
    //     }
    //     else
    //     {
    //         for (var index = 0; index < allRewardButtons.Count; index++)
    //         {
    //             allRewardButtons[index].DisableMe();
    //         }
    //     }
    // }


    #region Max Clicked Events

    void InitializeClickedEvents() 
    {
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnAdClickedEvent;
        MaxSdkCallbacks.AppOpen.OnAdClickedEvent += OnAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnAdClickedEvent;
        MaxSdkCallbacks.MRec.OnAdClickedEvent += OnAdClickedEvent;
       
    }
    #endregion

    #region MaxPaid Event

    void InitializePaidEvents()
    {
        Debug.Log("InitializePaidEvents");
        // Attach callbacks based on the ad format(s) you are using
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
       MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
    }

    private void OnAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo impressionData)
    {
        Debug.Log("OnAdRevenuePaidEvent");
        //Rewarded ad revenue paid. Use this callback to track user revenue.
       //AppImpressionAttributes impressionAttributes = new AppImpressionAttributes();
       // impressionAttributes.ad_platform = impressionData.NetworkName;
       // impressionAttributes.ad_id = adUnitId;
       // impressionAttributes.ad_type = 1;
       // impressionAttributes.ad_ecpm = impressionData.Revenue * 1000.00;
       // impressionAttributes.currency_type = "USD";
       // impressionAttributes.mediation_platform = "MAX";//Please input the mediation platform you're using.
       // impressionAttributes.is_rendered = true;
       // SolarEngine.Analytics.trackIAI(impressionAttributes);

        //TrackAdRevenue(adInfo);
    }
    private void OnAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //Debug.Log("Rewarded ad clicked");
        //OppenApp_Not_Shown = true;

        //AdClickAttributes AdClickAttributes = new AdClickAttributes();
        //AdClickAttributes.ad_platform = adInfo.NetworkName; // Monetization Platform Name
        //AdClickAttributes.mediation_platform = "MAX";// Mediation Platform that provides the ad 
        //AdClickAttributes.ad_id = adUnitId; // Monetization Platform ad placement ID
        //AdClickAttributes.ad_type = 1;

        //SolarEngine.Analytics.trackAdClick(AdClickAttributes);

    }

    #endregion

    #region AppOpen Ad Methods

    [SerializeField] private float appOpenTimeDelay = 30f;
    public bool isAppOpenShowing;


    public bool IsAllowToShowAppOpen()
    {
        Debug.Log("IsAllowToShowAppOpen " + (Time.time - lastTime));
        return Time.time - lastTime > 7f;
    }

    public void OnAppOpenDismissed()
    {
        isAppOpenShowing = false;
        if (Time.time - lastTime > appOpenTimeDelay)
        {
            lastTime = Time.time - (60 - appOpenTimeDelay);
        }
    }

    private void InitializeAppOpenAds()
    {
        //Attach callbacks
        MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnApOpenLoadedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += OnApOpenFailedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += ApOpenFailedToDisplayEvent;
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += OnApOpenDisplayedEvent;
        //Load the first interstitial
        StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));
    }

    private IEnumerator PriorityApOpenAdLoaded(string adUnit)
    {
        Debug.Log("ApOpen ad requesting to load  " + adUnit);
        yield return new WaitForSeconds(1f);
        LoadApOpen();
    }

    void LoadApOpen()
    {
        if (!MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
            MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
    }

    private static bool isBannerLastShowing = false;
    private static bool isMRECLastShowing = false;

    public void HideBannersAds() 
    {
        if (isBannerShowing)
        {
            HideBanner();
            isBannerLastShowing = true;
        }

        if (isMRECLastShowing)
        {
            HideMRec();
            isMRECLastShowing = true;
        }
    }

    public void ShowHiddenBannersAd() 
    {
        Invoke(nameof(ShowDely), 0.3f);
    }

    void ShowDely() 
    {
        if (isBannerLastShowing)
        {
            ShowBanner();
            isBannerLastShowing = false;
        }
        if (isMRECLastShowing)
        {
            ShowMRec();
            isMRECLastShowing = false;
        }
    }
    public Action AppOpenInternalReward;
    public void ShowAppOpenAdIfReady(Action AppOpenReward)
    {
        AppOpenInternalReward = AppOpenReward;

        if (isAppOpenShowing || _isAdRemoved || isAdShowing)
            return;

        if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
        {
            HideBannersAds();
            Invoke(nameof(ShowingAppOpen),0.3f);
            Debug.Log("ShowingAppOpen");
        }
        else
        {
            Debug.Log("Loading appopen");
            AppOpenInternalReward?.Invoke();
            MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
        }
    }

    void ShowingAppOpen() 
    {
        MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
    }

    //private void OnApplicationPause(bool pauseStatus)
    //{
    //    if (!pauseStatus)
    //    {
    //        ShowAppOpenAdIfReady();
    //    }
    //}

    public bool isApOpenLoaded;
    private void OnApOpenLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        //Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
        Debug.Log("ApOpen loaded with adInfo" + adInfo);
        isApOpenLoaded = true;
        try
        {
            //FirebaseInitialize.instance.LogEvent1("MREC_Loaded");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void OnApOpenFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo adInfo)
    {
        Debug.Log("ApOpen failed to load with error code: " + adInfo + " " + adUnitId);

        StartCoroutine(PriorityApOpenAdLoaded(AppOpenAdUnitId));

    }

    private void ApOpenFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("_AppOpen failed to display with error code: " + errorInfo + "  " + adInfo);
        AppOpenInternalReward?.Invoke();
        StartCoroutine(PriorityApOpenAdLoaded(AppOpenAdUnitId));


    }

    private void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppOpenInternalReward?.Invoke();
        isApOpenLoaded = false;

        // AppOpen ad is dismissed. Re-show Banner and MREC ads if they were showing before
        ShowHiddenBannersAd();

        // Pre-load the next ad
        Debug.Log("AppOpenDismissed " + adInfo);
        OnAppOpenDismissed();
        StartCoroutine(PriorityApOpenAdLoaded(AppOpenAdUnitId));
    }

    public string interstitialPlacement = "level_up";
    private void OnApOpenDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AppOpenInternalReward?.Invoke();
        try
        {
            // FirebaseInitialize.instance.LogEvent1("App_Shown");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        isAppOpenShowing = true;

        // Hide Banner and MREC when AppOpen ad is shown
        HideBannersAds();
    }

    #endregion

    #region Interstitial Ad Methods

    private void InitializeInterstitialAds()
    {
        // Attach callbacks
       //MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += Interstitial_OnAdLoadedEvent;
        //MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += Interstitial_OnAdLoadFailedEvent;
        //MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += Interstitial_OnAdDisplayFailedEvent;
        //MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += Interstitial_OnAdHiddenEvent;
        //MaxSdkCallbacks.OnInterstitialDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += Interstitial_OnAdDisplayedEvent;
        // Load the first interstitial
        StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));
    }

    private void Interstitial_OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        isAdShowing = true;
        AudioListener.pause = true;
        lastTime = Time.time;

        // Hide Banner and MREC when Interstitial ad is shown
        HideBannersAds();

        Debug.Log("_Interstitial Ads Is Displayed: " );
    }

    private void Interstitial_OnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        lastTime = Time.time;
        AudioListener.pause = false;
        CancelInvoke(nameof(DelayInterstitialShow));
        Invoke(nameof(DelayInterstitialShow), 5f);
        isInterstitialLoaded = false;
        InterInternalReward?.Invoke();

        // Interstitial ad is hidden. Pre-load the next ad
        Debug.Log("_Interstitial dismissed ");

        // Re-show Banner and MREC ads if they were showing before
        ShowHiddenBannersAd();

        StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));
    }

    private void Interstitial_OnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        // Interstitial ad failed to display. We recommend loading the next ad
        Debug.Log("_Interstitial failed to display with error code: ");
        InterInternalReward?.Invoke();
        StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));
    }

    private void Interstitial_OnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        Debug.Log("_Interstitial failed to load with error code: ");

        StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));
    }

    private void Interstitial_OnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        Debug.Log("Interstitial loaded " );
        isInterstitialLoaded = true;
        try
        {
            //FirebaseInitialize.instance.LogEvent1("Interstital_Loaded");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private IEnumerator PriorityInterstitialAdLoaded(string adUnit)
    {
        Debug.Log("Interstitial ad requesting to load  " + adUnit);
        yield return new WaitForSeconds(1.5f);
        LoadInterstitial(adUnit);
    }
    void LoadInterstitial(string adUnit)
    {
        if (!MaxSdk.IsInterstitialReady(adUnit))
            MaxSdk.LoadInterstitial(adUnit);
    }

    public Action InterInternalReward;
    public void ShowInterstitialWithoutConditions(string placement,Action InterReward)
    {
        InterInternalReward = InterReward;
        if (!_isAdRemoved)
        {
            if (RemoveAds == 1)
            {
                _isAdRemoved = true;
                return;
            }
        }
        Debug.Log("Showing...Inter "+placement);

        if (MaxSdk.IsInterstitialReady(InterstitialAdUnitIdGeneric))
        {
            HideBannersAds();
            Invoke(nameof(ShowingINter),0.5f);
        }
        else 
        {
            InterInternalReward?.Invoke();
        }

    }

    void ShowingINter() 
    {
        MaxSdk.ShowInterstitial(InterstitialAdUnitIdGeneric);
    }


    // public void ShowInterstitialWithConditions(string placement)
    // {
    //     if (!_isAdRemoved)
    //     {
    //         if (DataManager.RemoveAds == 1)
    //         {
    //             _isAdRemoved = true;
    //             return;
    //         }
    //     }
    //
    //     if (Time.time - lastTime > Preferences.Instance.Interstitial_frequency)
    //     {
    //         //  interstitialPlacement = placement;
    //         interstitialConditions();
    //         //  Debug.Log("Int frequency true");
    //     }
    //     else
    //     {
    //         // Debug.Log("int frequency false");
    //
    //     }
    //
    // }

    // void interstitialConditions()
    // {
    //     if (Time.realtimeSinceStartup > Preferences.Instance.Interstitial_first_delay
    //         && GameManagerNew.Instance.currentLevel >= Preferences.Instance.Interstitial_first_level
    //     )
    //     {
    //         if (Time.time - rVShownTime > Preferences.Instance.Inter_pause_after_RV)
    //         {
    //             if (!_isAdRemoved)
    //             {
    //                 if (DataManager.RemoveAds == 1)
    //                 {
    //                     _isAdRemoved = true;
    //                     return;
    //                 }
    //             }
    //
    //            
    //             if (MaxSdk.IsInterstitialReady(InterstitialAdUnitIdGeneric))
    //             {
    //                 MaxSdk.ShowInterstitial(InterstitialAdUnitIdGeneric);
    //             }
    //
    //         }
    //         else
    //         {
    //            // Debug.Log("Inter after RV pause time false");
    //         }
    //
    //     }
    //     else
    //     {
    //        // Debug.Log("You dont meet server data requirement for interstitial");
    //     }
    // }




    public bool isInterstitialLoaded;
    //private void OnInterstitialLoadedEvent(string adUnitId)
    //{
    //    // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
    //    Debug.Log("Interstitial loaded " + adUnitId);
    //    isInterstitialLoaded = true;
    //    try
    //    {
    //        //FirebaseInitialize.instance.LogEvent1("Interstital_Loaded");
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.Log(ex.Message);
    //    }
    //}

    //private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    //{
    //    Debug.Log("_Interstitial failed to load with error code: " + errorCode + " " + adUnitId);
       
    //        StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));

    //}

    //private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    //{
    //    // Interstitial ad failed to display. We recommend loading the next ad
    //    Debug.Log("_Interstitial failed to display with error code: " + errorCode + "  " + adUnitId);
    //    InterInternalReward?.Invoke();
    //    StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));
            
    //}

    //private void OnInterstitialDismissedEvent(string adUnitId)
    //{
    //    lastTime = Time.time;
    //    AudioListener.pause = false;
    //    CancelInvoke(nameof(DelayInterstitialShow));
    //    Invoke(nameof(DelayInterstitialShow), 5f);
    //    isInterstitialLoaded = false;
    //    InterInternalReward?.Invoke();

    //    // Interstitial ad is hidden. Pre-load the next ad
    //    Debug.Log("_Interstitial dismissed " + adUnitId);

    //    // Re-show Banner and MREC ads if they were showing before
    //    ShowHiddenBannersAd();

    //    StartCoroutine(PriorityInterstitialAdLoaded(InterstitialAdUnitIdGeneric));
    //}


    void DelayInterstitialShow()
    {
        isAdShowing = false;
    }
    
    public bool isAdShowing;
    //private void OnInterstitialDisplayedEvent(string adUnitId)
    //{
    //    isAdShowing = true;
    //    AudioListener.pause = true;
    //    lastTime = Time.time;

    //    // Hide Banner and MREC when Interstitial ad is shown
    //    HideBannersAds();

    //    Debug.Log("_Interstitial Ads Is Displayed: " + adUnitId);
    //}

    #endregion

    #region Rewarded Ad Methods

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        //MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += Rewarded_OnAdLoadedEvent;
        //MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += Rewarded_OnAdLoadFailedEvent;
        //MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += Rewarded_OnAdDisplayFailedEvent;
        //MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += Rewarded_OnAdDisplayedEvent;
        //MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        //MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += Rewarded_OnAdHiddenEvent;
        //MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += Rewarded_OnAdReceivedRewardEvent;
        StartCoroutine(PriorityRewardedLoad(RewardedAdUnitIdGeneric));
    }

    private void Rewarded_OnAdReceivedRewardEvent(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
    {
        Debug.Log("Rewarded ad received reward ");
        _isAdLoaded = false;
        //  CheckButtonsStatus();
        //FirebaseAnalytics.LogEvent("ad_rewarded_shown");
        //Here give reward

        rVShownTime = Time.time;

        RewardGiven?.Invoke();
        try
        {
            //FirebaseInitialize.instance.LogEvent1("Rewarded_Received");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void Rewarded_OnAdHiddenEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        AudioListener.pause = false;
        Debug.Log("Rewarded ad dismissed");
        _isAdLoaded = false;

        // Rewarded ad is dismissed. Re-show Banner and MREC ads if they were showing before
        ShowHiddenBannersAd();

        StartCoroutine(PriorityRewardedLoad(RewardedAdUnitIdGeneric));
        CancelInvoke(nameof(DelayInterstitialShow));
        Invoke(nameof(DelayInterstitialShow), 5f);
    }

    private void Rewarded_OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        isAdShowing = true;
        AudioListener.pause = true;
        Debug.Log("Rewarded ad displayed" );

        // Hide Banner and MREC when Rewarded ad is shown
        HideBannersAds();
    }

    private void Rewarded_OnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        Debug.Log("Rewarded ad failed to display with error code: " );
        _isAdLoaded = false;
        // CheckButtonsStatus();
        StartCoroutine(PriorityRewardedLoad(RewardedAdUnitIdGeneric));
    }

    private void Rewarded_OnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        Debug.Log("Rewarded ad failed to load with error code: " );
        _isAdLoaded = false;
        StartCoroutine(PriorityRewardedLoad(RewardedAdUnitIdGeneric));
    }

    private void Rewarded_OnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
        Debug.Log("Rewarded ad loaded" + " " );
        _isAdLoaded = true;
        // Invoke(nameof(CheckButtonsStatus), 1);
        // CheckButtonsStatus();
        try
        {
            //FirebaseInitialize.instance.LogEvent1("Rewarded_Loaded");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private IEnumerator PriorityRewardedLoad(string adUnit)
    {
        yield return new WaitForSeconds(1.5f);
        LoadRewardedAd(adUnit);

    }
    private void LoadRewardedAd(string adUnit)
    {
        Debug.Log("Rewarded ad requesting to load " + adUnit);
        if (!MaxSdk.IsRewardedAdReady(adUnit))
            MaxSdk.LoadRewardedAd(adUnit);
    }

    Action RewardGiven;
    public void ShowRewardedAd(Action reward)
    {
        RewardGiven = reward;
        HideBannersAds();

        if (MaxSdk.IsRewardedAdReady(RewardedAdUnitIdGeneric))
        {
            isAdShowing = true;
            MaxSdk.ShowRewardedAd(RewardedAdUnitIdGeneric);
        }

        print("New Rewarded Not Ready ");
    }


    private bool _isAdFillAvailable;

    //private void OnRewardedAdLoadedEvent(string adUnitId)
    //{
    //    // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
    //    Debug.Log("Rewarded ad loaded" + " " + adUnitId);
    //    _isAdLoaded = true;
    //    // Invoke(nameof(CheckButtonsStatus), 1);
    //    // CheckButtonsStatus();
    //    try
    //    {
    //        //FirebaseInitialize.instance.LogEvent1("Rewarded_Loaded");
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.Log(ex.Message);
    //    }
    //}

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        Debug.Log("Rewarded ad failed to load with error code: " + errorCode + " " + adUnitId);
        _isAdLoaded = false;
            StartCoroutine(PriorityRewardedLoad(RewardedAdUnitIdGeneric));

       // CheckButtonsStatus();
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        Debug.Log("Rewarded ad failed to display with error code: " + errorCode + " " + adUnitId);
        _isAdLoaded = false;
       // CheckButtonsStatus();
        StartCoroutine(PriorityRewardedLoad(RewardedAdUnitIdGeneric));
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId)
    {
        isAdShowing = true;
        AudioListener.pause = true;
        Debug.Log("Rewarded ad displayed" + adUnitId);

        // Hide Banner and MREC when Rewarded ad is shown
        HideBannersAds();   
    }

    private void OnRewardedAdClickedEvent(string adUnitId)
    {
        Debug.Log("Rewarded ad clicked" + " " + adUnitId);
    }

private void OnRewardedAdDismissedEvent(string adUnitId)
{
    AudioListener.pause = false;
    Debug.Log("Rewarded ad dismissed" + " " + adUnitId);
    _isAdLoaded = false;

    // Rewarded ad is dismissed. Re-show Banner and MREC ads if they were showing before
    ShowHiddenBannersAd();

    StartCoroutine(PriorityRewardedLoad(RewardedAdUnitIdGeneric));
    CancelInvoke(nameof(DelayInterstitialShow));
    Invoke(nameof(DelayInterstitialShow), 5f);
}

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)  // give reward
    {
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward " + " " + adUnitId);
        _isAdLoaded = false;
        //  CheckButtonsStatus();
        //FirebaseAnalytics.LogEvent("ad_rewarded_shown");
        //Here give reward

        rVShownTime = Time.time;

        RewardGiven?.Invoke();
        try
        {
            //FirebaseInitialize.instance.LogEvent1("Rewarded_Received");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    #endregion

    #region Banner Ad Methods



    private void ToggleBannerVisibility()
    {
        if (!isBannerShowing)
        {
            MaxSdk.ShowBanner(BannerAdUnitId);
            isBannerShowing = true;
            //  showBannerButton.GetComponentInChildren<Text>().text = "Hide Banner";
        }
        else
        {
            MaxSdk.HideBanner(BannerAdUnitId);
            isBannerShowing = false;
            //  showBannerButton.GetComponentInChildren<Text>().text = "Show Banner";
        }
    }

    private void InitializeBannerAds()
    {
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
        MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.black);
        //MaxSdkCallbacks.OnBannerAdLoadedEvent += OnBannerLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += Banner_OnAdLoadedEvent; ;

    }

    private void Banner_OnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
        throw new NotImplementedException();
    }

    public void RemoveAdsIAP() 
    {
        PlayerPrefs.SetInt("remove_ads", 1);
        HideBanner();
        HideMRec();
    }


    public void OnBannerLoadedEvent(string BannerAdUnitId)
    {
       // AnalyticEvents.Instance.LogAd_Banner_shownEvent("game start");

    }
    public void ShowBanner()
    {
        if (!_isAdRemoved)
        {
            if (RemoveAds == 1)
            {
                _isAdRemoved = true;
                return;
            }
        }
        MaxSdk.ShowBanner(BannerAdUnitId);
        isBannerShowing = true;
        Debug.LogError("Banner Showing -----");
    }


    public void HideBanner()
    {
        MaxSdk.HideBanner(BannerAdUnitId);
        Debug.Log("<color=green>Banner Hiding -----</color>");
        isBannerShowing = false;
    }

    #endregion

    #region SaveData

    public int Gdpr_accepted
    { 
        get => PlayerPrefs.GetInt("gdpr_accepted", 0);
        set
        {
            PlayerPrefs.SetInt("gdpr_accepted", value);
        }
    }
    
    public int RemoveAds
    { 
        get => PlayerPrefs.GetInt("remove_ads", 0);
        set
        {
            PlayerPrefs.SetInt("remove_ads", value);
        }
    }
    
    public int Interstitial_count
    { 
        get => PlayerPrefs.GetInt("Interstitial_count", 0);
        set
        {
            PlayerPrefs.SetInt("Interstitial_count", value);
        }
    }
    
    public int Rewarded_count
    { 
        get => PlayerPrefs.GetInt("Rewarded_count", 0);
        set
        {
            PlayerPrefs.SetInt("Rewarded_count", value);
        }
    }

    #endregion
    
    
    
}
