using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    
    public void RestartLevel()
    {
        List<int> specialLevels = new List<int> { 8, 11, 13, 17, 23, 26, 31, 39, 41, 44, 47, 51 };
        if (specialLevels.Contains(GameplayMgr.Instance.currentLevel))
        {
            PlayerPrefs.SetInt("Restart", 1);
            SceneManager.LoadScene("MainScene");
        }
        else
        {

            GameplayMgr.Instance.GoToLevel(GameplayMgr.Instance.currentLevel);
            PlayerPrefs.SetInt("Restart", 0);
        }
       
       
        //GameplayMgr.Instance.GoToLevel(GameplayMgr.Instance.currentLevel);
        SoundsMgr.Instance.PlaySFX(SoundsMgr.Instance.sfxListConfig.sfxConfigDic[SFXType.BUTTON], false);
        GameplayMgr.Instance.DeactivateCapForAllRingStacks();
    }
   
}
