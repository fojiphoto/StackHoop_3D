using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    public TextMeshProUGUI undoNumberText;
    [SerializeField]
    private GameObject watchAd; 
    private bool riseEnableButtonFlag = false;
    private bool riseDisableButtonFlag = false;

    private void Awake()
    {
      
        EventDispatcher.Instance.RegisterListener(EventID.ON_LOADED_REWARDED_AD, param => EnableButtonAd());
        EventDispatcher.Instance.RegisterListener(EventID.ON_FAILED_LOAD_REWARDED_AD, param => DisableButtonAd());
        EventDispatcher.Instance.RegisterListener(EventID.ON_CHANGED_UNDO_TIME, param => OnChangedUndoTime());
    }

    private void Update()
    {
        if (riseEnableButtonFlag)
        {
            button.interactable = true;
            riseEnableButtonFlag = false;
            undoNumberText.gameObject.SetActive(true);
        }

        if (riseDisableButtonFlag)
        {
            button.interactable = false;
            riseDisableButtonFlag = false;
            undoNumberText.gameObject.SetActive(false);
        }
    }
public GameObject RingStackObj;
    public void UndoMove()
    {
        if ( PlayerPrefs.GetInt("Cap")>0)
        { 
            RingStackObj = RingStack.ringStacks.gameObject;
           RingStack.ringStacks.transform.GetChild(2).gameObject.SetActive(false);
            PlayerPrefs.SetInt("Cap",0);
            Debug.Log("Cap bool is :"+PlayerPrefs.GetInt("Cap"));
        }
        if ((GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayIdle) ||
            (GameplayMgr.Instance.stateMachine.CurrentState == GameplayMgr.Instance.stateGameplayRingReady))
        {
            if (GameplayMgr.Instance.mapDataStack.Count > 0)
            {
                if (GameplayMgr.Instance.undoTime == 0)
                {
                    watchAd.SetActive(false);

                    //nadeem

                     AdsManager.instance.ShowRewardedAd(()=>{AdsManager.instance.rewardedTypeAd=AdsManager.RewardType.UNDO;});
                    // CASAds.instance.ShowRewarded(()=> 
                    // {
                    //     CASAds.instance.rewardedTypeAd = CASAds.RewardType.UNDO;
                    // });
                }

                GameplayMgr.Instance.UndoLevel();
                SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
            }
        }
    }

    public void EnableButtonAd()
    {
        if (GameplayMgr.Instance.undoTime == 0)
        {
            EnableButton();
            Debug.Log("He he me go active OwO !");
        }
    }

    public void DisableButtonAd()
    {
        if (GameplayMgr.Instance.undoTime == 0)
        {
            DisableButton();
            Debug.Log("He he me go deactive OwO !");
        }
    }

    public void EnableButton()
    {
        riseEnableButtonFlag = true;
        riseDisableButtonFlag = false;

    }

    public void DisableButton()
    {
        riseEnableButtonFlag = false;
        riseDisableButtonFlag = true;
    }

    public void OnChangedUndoTime()
    {
          GameplayMgr.Instance.DeactivateCapForAllRingStacks();
        undoNumberText.text = "<sprite index=" + GameplayMgr.Instance.undoTime + ">";
        if (GameplayMgr.Instance.undoTime == 0)
        {
            watchAd.SetActive(true);
        }
    }
}
