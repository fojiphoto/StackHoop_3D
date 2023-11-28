//using Firebase.Extensions;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Performance Params")]
    public int framerate = 60;
    public float ratio = 1.5f;

    [Header("Options")]
    public bool SoundEnable = true;
    public bool VibrateEnable = true;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_LOAD_SERVICE_DONE, param => CheckServicesLoad());
#if !UNITY_EDITOR
        Application.targetFrameRate = framerate;
        if (Screen.width <= 1080)
        {
            Screen.SetResolution((int)((float)Screen.width * 3/4), (int)((float)Screen.height * 3/4), true);
        }
        else
        {
            Screen.SetResolution((int)((float)Screen.width / ratio), (int)((float)Screen.height / ratio), true);
        }
#endif
    }

    private void CheckServicesLoad()
    {   //nadeem
        // if (RemoteConfigMgr.Instance.isDoneInitRemoteConfig &&
        //     CASAds.instance.isDoneCASInit)
        // {
        //     GameplayMgr.Instance.Init();
        // }
    }

    private void Start() {
        Invoke(nameof(ShowBannerAd),2.5f);
    }

    public void ShowBannerAd(){
        AdsManager.instance.ShowBanner();
    }
}
