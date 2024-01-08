using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDPR : MonoBehaviour
{
    public string PrivacyPolicy;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("GDPR")>0)
        {
            Accept();
        }
        
    }


    public void PrivacyPolicyOpen() 
    {
        Application.OpenURL(PrivacyPolicy);
    }

    public void Accept() 
    {
        if (Application.loadedLevelName=="GDPR")
        {
        Application.LoadLevel("Splash");
        PlayerPrefs.SetInt("GDPR",1);

        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
