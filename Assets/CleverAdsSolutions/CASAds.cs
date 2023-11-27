using System;
using System.Collections;
using System.Collections.Generic;
using CAS;
using UnityEngine;

public class CASAds : MonoBehaviour
{
    public enum RewardType { NONE, RING_STACK, UNDO };
    public static CASAds instance = null;
    [HideInInspector] public bool isDoneCASInit = false;
    private static IMediationManager _manager = null;
    private static IAdView _lastAdView = null;
    private static IAdView _lastMrecAdView = null;
    private static Action _lastAction = null;
    [HideInInspector]
    public RewardType rewardedTypeAd = RewardType.NONE;

    private void Awake()
    {
        if ( instance == null )
            instance = this;
        
        DontDestroyOnLoad( this );
    }

    private void Start()
    {
        CAS.MobileAds.settings.isExecuteEventsOnUnityThread = true;

        Init();
    }

    private void Init()
    {
        _manager = MobileAds.BuildManager()
            .WithInitListener(CreateAdView)
            // Call Initialize method in any case to get IMediationManager instance
            .Initialize();
        isDoneCASInit = true;
        _manager.OnRewardedAdCompleted += _lastAction;

       // AdmobGA_Helper.GA_Log(AdmobGAEvents.Initialized);

        try
        {
            //F/irebaseInitialize.instance.LogEvent1("CAS:INIT");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    private void CreateAdView(bool success, string error)
    {
        _lastAdView = _manager.GetAdView(AdSize.Banner);
        _lastMrecAdView = _manager.GetAdView(AdSize.MediumRectangle);
        _lastAdView.SetActive(false);
        _lastMrecAdView.SetActive(false);

        //AdmobGA_Helper.GA_Log(AdmobGAEvents.RequestBannerAd);
    }
 
    public void ShowBanner(AdPosition position)
    {
        if (_lastAdView == null)
        {
            CreateAdView(true, ""); 
        }

        if (_lastAdView != null)
        {
            _lastAdView.position = position;
            _lastAdView.SetActive(true);
        }

        //AdmobGA_Helper.GA_Log(AdmobGAEvents.BannerAdDisplayed);
        try
        {
           // FirebaseInitialize.instance.LogEvent1("CAS:BANNER_SHOW");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void ShowMrecBanner(AdPosition position)
    {
        if (_lastMrecAdView == null)
        {
            CreateAdView(true, "");
        }

        if (_lastMrecAdView != null)
        {
            _lastMrecAdView.position = position;
            _lastMrecAdView.SetActive(true);
        }
       // AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowMREC);

        try
        {
           // FirebaseInitialize.instance.LogEvent1("CAS:MREC_BANNER_SHOW");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    public void HideBanner()
    {
        if ( _lastAdView != null )
        {
            _lastAdView.SetActive( false );
        }
       // AdmobGA_Helper.GA_Log(AdmobGAEvents.BannerAdRemoved);
    }

    public void HideMrecBanner()
    {
        if (_lastMrecAdView != null)
        {
            _lastMrecAdView.SetActive(false);
        }
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.HideMREC);
    }

    public void ShowInterstitial()
    {
        _manager?.ShowAd( AdType.Interstitial );
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowInterstitialAd);

        try
        {
            //FirebaseInitialize.instance.LogEvent1("CAS:INTER_SHOW");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void ShowRewarded( Action complete )
    {
        if ( _manager == null )
            return;
        
        if ( _lastAction != null)
        {
            _manager.OnRewardedAdCompleted -= _lastAction;
        }

        _lastAction = complete;
        _manager.OnRewardedAdCompleted += _lastAction;
        _manager?.ShowAd(AdType.Rewarded);

        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowRewardedAd);
        try
        {
           // FirebaseInitialize.instance.LogEvent1("CAS:REWARDED_SHOW");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}
