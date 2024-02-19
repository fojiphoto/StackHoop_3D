using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel()
    {
        GameplayMgr.Instance.GoToLevel(GameplayMgr.Instance.currentLevel);
        //SceneManager.LoadScene("MainScene");
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
        GameplayMgr.Instance.DeactivateCapForAllRingStacks();
    }
}
