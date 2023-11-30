using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel()
    {
        GameplayMgr.Instance.GoToLevel(GameplayMgr.Instance.currentLevel);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
        GameplayMgr.Instance.DeactivateCapForAllRingStacks();
    }
}
