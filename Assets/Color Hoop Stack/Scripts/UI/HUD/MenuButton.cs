using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject panelBackground;
    public GameObject settingMenuPanel;

    public void TurnOnSettingMenuPanel()
    {

        settingMenuPanel.SetActive(true);
        panelBackground.SetActive(true);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
    }
}
