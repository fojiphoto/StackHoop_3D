using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public WinMenu winMenu;
    public RestartButton restartButton;
    public MoreStackButton moreStackButton;
    public UndoButton undoButton;
    public GameObject bannerAdWarpper;
    public GameObject loadingScreen;
    public float winPanelTime = 0.5f;

    private Coroutine enableWinMenu = null;

    // Start is called before the first frame update
    void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_COMPLETE_LEVEL, param => CompleteLevelUI());
        EventDispatcher.Instance.RegisterListener(EventID.ON_INIT_LEVEL, param => InitLevelUI());
        EventDispatcher.Instance.RegisterListener(EventID.ON_LOAD_SERVICE_DONE, param => CheckServicesLoad());
        EventDispatcher.Instance.RegisterListener(EventID.ON_LOADED_BANNER_AD, param => ShowBannerWarpper());
        EventDispatcher.Instance.RegisterListener(EventID.ON_DESTROYED_BANNER_AD, param => HideBannerWarpper());
    }

    public void InitLevelUI()
    {
        if (enableWinMenu != null)
            StopCoroutine(enableWinMenu);
        winMenu.gameObject.SetActive(false);
        moreStackButton.InitLevel();
        if (GameplayMgr.Instance.currentLevel == 9)
        {
            moreStackButton.EnableButton();
        }
    }

    public void CompleteLevelUI()
    {
        enableWinMenu = StartCoroutine(EnableWinMenuAfter(winPanelTime));
    }

    private IEnumerator EnableWinMenuAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        winMenu.gameObject.SetActive(true);
    }

    public void ShowBannerWarpper()
    {
        bannerAdWarpper.SetActive(true);
    }

    public void HideBannerWarpper()
    {
        bannerAdWarpper.SetActive(false);
    }

    private void CheckServicesLoad()
    {
        if (RemoteConfigMgr.Instance.isDoneInitRemoteConfig &&
            CASAds.instance.isDoneCASInit)
        {
            loadingScreen.SetActive(false);
        }
    }
}
