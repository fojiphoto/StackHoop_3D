using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreStackButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    private bool riseEnableButtonFlag = false;
    private bool riseDisableButtonFlag = false;
    private bool canUse = true;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_LOADED_REWARDED_AD, param => EnableButton());
        EventDispatcher.Instance.RegisterListener(EventID.ON_FAILED_LOAD_REWARDED_AD, param => DisableButton());
    }

    private void Update()
    {
        if (riseEnableButtonFlag)
        {
            button.interactable = true;
            riseEnableButtonFlag = false;
        }

        if (riseDisableButtonFlag)
        {
            button.interactable = false;
            riseDisableButtonFlag = false;
        }
    }

    public void InitLevel()
    {
        canUse = true;
    }

    public void AddMoreStack()
    {
        if (GameplayMgr.Instance.enabledTutorial)
        {
            GameplayMgr.Instance.stateMachine.StateChange(GameplayMgr.Instance.stateGameplayAddStack);
            EventDispatcher.Instance.PostEvent(EventID.ON_DISABLED_TUTORIAL);
            DisableButton();
        }
        else
        {
            
            CASAds.instance.ShowRewarded(()=> { CASAds.instance.rewardedTypeAd = CASAds.RewardType.RING_STACK; });
            canUse = false;
            SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
            DisableButton();
        }
    }

    public void EnableButton()
    {
        if (canUse)
        {
            riseEnableButtonFlag = true;
            riseDisableButtonFlag = false;
        }
    }

    public void DisableButton()
    {
        if (canUse)
        {
            riseEnableButtonFlag = false;
            riseDisableButtonFlag = true;
        }
    }
}
