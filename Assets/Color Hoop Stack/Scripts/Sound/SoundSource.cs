using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public AudioSource audioSource;
    private float timeIntro = 0f;
    private float timeLoop = 0f;
    public bool isLoop = false;

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (!isLoop)
            {

                SoundsMgr.Instance.sfxPooler.ReturnPooledObject(this.gameObject);
            }
        }

    }

    private void FixedUpdate()
    {
        if (isLoop)
        {
            if ((timeIntro >= 0f) && (timeLoop > 0f))
            {
                if (audioSource.isPlaying)
                {
                
                    if (audioSource.time > (timeIntro + timeLoop))
                    {
                        audioSource.time = timeIntro;
                    }
                }
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
    }

    public void PlayAudio(AudioClip clip, bool isLoop, float timeIntro, float timeLoop)
    {
        this.isLoop = isLoop;
        this.timeIntro = timeIntro;
        this.timeLoop = timeLoop;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayAudio(AudioClip clip, bool isLoop)
    {
        this.isLoop = isLoop;
        this.timeIntro = -1f;
        this.timeLoop = -1f;
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
