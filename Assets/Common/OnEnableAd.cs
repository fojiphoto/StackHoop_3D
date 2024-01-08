using UnityEngine;

public class OnEnableAd : MonoBehaviour
{
    private void OnEnable()
    {
        //AdsManager.Instance?.ShowMRec();
        if (PlayerPrefs.GetInt("NoAds") < 1)
        {
            CASAds.instance.ShowMrecBanner(CAS.AdPosition.TopCenter);
        }
    }
}
