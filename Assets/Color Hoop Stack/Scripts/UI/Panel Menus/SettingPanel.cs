using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MenuPanel
{
    public GameObject ratePanel;
    public GameObject policyPanel;

    public void OpenRatePanel()
    {
        ratePanel.SetActive(true);
        gameObject.SetActive(false);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
    }

    public void OpenPolicyPanel()
    {
        policyPanel.SetActive(true);
        gameObject.SetActive(false);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
    }
    public void openUrl()
    {
        Application.OpenURL("https://orbitgamesglobal-privacy-policy.blogspot.com/");
    }
    public void SaveSetting()
    {
        FileHandler fileHandler = new FileHandler();
        fileHandler.SaveSettingData();
    }

    public void DisablePanel()
    {
        SaveSetting();
        base.DisablePanel();
    }
}
