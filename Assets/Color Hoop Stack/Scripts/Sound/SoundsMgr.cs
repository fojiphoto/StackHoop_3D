using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SFXPooler))]
public class SoundsMgr : Singleton<SoundsMgr>
{
    public SFXPooler sfxPooler;
    public SFXListConfig sfxListConfig;

    private GameObject bgmPlaying = null;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlaySFX(AudioClip clip, bool isLoop)
    {
        if (GameManager.Instance.SoundEnable)
        {
            sfxPooler.GetNextPooledObject(clip, isLoop);
        }
    }
}
