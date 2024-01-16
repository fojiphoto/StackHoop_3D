using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject splashScreen;
    public GameObject loadScreen;
    public Text leveltext;
    void Start()
    {
        int currentLevel=PlayerPrefs.GetInt("Levelnumber")+1;
        leveltext.text=currentLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelStart(){
        StartCoroutine(SplashWait());
    }
    public IEnumerator SplashWait(){
        splashScreen.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        splashScreen.SetActive(false);
        loadScreen.SetActive(false);
        GameplayMgr.Instance.LevelGo();
    }

    public void MainMenuOpen(){
         loadScreen.SetActive(true);
    }

    public void RateUs()
    {  
       string rateus = "Playstore k lye";
       Application.OpenURL(rateus);
    }
    public void MoreGames()
    {
        string moreGames = "Playstore k lye";
        Application.OpenURL(moreGames);
    }
}
