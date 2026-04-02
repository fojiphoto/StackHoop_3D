using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public Image fillImage;      // Assign in Inspector (Type = Filled)
    private float duration = 12f; // Same as Invoke time
    private bool adShown = false;

    void Start()
    {
        if (fillImage != null)
            fillImage.fillAmount = 0f;

        StartCoroutine(SplashSequence());
    }

    private IEnumerator SplashSequence()
    {
        float timer = 0f;

        while (timer < duration)
        {
            // Fill progress
            if (fillImage != null)
                fillImage.fillAmount = timer / duration;

            // Show ad at 80% (0.8 sec)
            if (!adShown && timer >= duration * 0.8f)
            {
                adShown = true;
                AdsManager.instance.ShowAppOpenAdIfReady(null);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure full fill
        if (fillImage != null)
            fillImage.fillAmount = 1f;

        SceneManager.LoadScene("MainScene");
    }
}
