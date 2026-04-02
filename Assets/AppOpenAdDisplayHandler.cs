using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class AppOpenAdDisplayHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;

    // [SerializeField] private bool m_CanShowAppOpenAd => AdmobImplementation.CanAppOpenShow;

    private void OnApplicationFocus(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //Debug.LogError("Showing App Open");
            Invoke(nameof(ShowAppOpen), 0.5f);
        }
        //m_Text.text = pauseStatus ? "Came From Focus" : "None";

       // Debug.LogError(pauseStatus ? "Came From Focus" + AdsManager.instance.isAppOpenShowing : "None" + AdsManager.instance.isAppOpenShowing);
    }

    void ShowAppOpen() 
    {
        
           AdsManager.instance.ShowAppOpenAdIfReady(null);
        
    }
}
