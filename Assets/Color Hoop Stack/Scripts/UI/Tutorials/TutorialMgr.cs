using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class TutorialMgr : MonoBehaviour
{
    [SerializeField] private GameObject tutorialText;
    [SerializeField] private GameObject tutorialCursor;
    [SerializeField] private GameObject tutorialCorrect;
    [SerializeField] private GameObject tutorialMoreStack;

    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_ENABLED_TUTORIAL, param => EnableTutorial());
        EventDispatcher.Instance.RegisterListener(EventID.ON_DISABLED_TUTORIAL, param => DisableTutorial());
    }
    public void Update()
    {
        Debug.Log(GameplayMgr.Instance.currentLevel);
    }

    public void EnableTutorial()
    {
        Debug.Log("Level No"+GameplayMgr.Instance.currentLevel);
        if (GameplayMgr.Instance.currentLevel == 0)
        {
            tutorialText.SetActive(true);
            tutorialCursor.SetActive(true);
        }
        else if (GameplayMgr.Instance.currentLevel == 1)
        {
            tutorialText.SetActive(true);
            tutorialCorrect.SetActive(true);
        }
        else if (GameplayMgr.Instance.currentLevel == 2)
        {
            
            tutorialMoreStack.SetActive(true);
            tutorialCursor.SetActive(true);
        }
    }
    public void OnEnable()
    {
        this.gameObject.AddComponent<Button>();
        this.gameObject.GetComponent<Button>().onClick.AddListener(buttonfnc);
    }
    private void buttonfnc()
    {
        this.gameObject.SetActive(false);
    }
    public void DisableTutorial()
    {
        if (tutorialText.activeSelf)
        {
            tutorialText.SetActive(false);
        }
        if (tutorialCursor.activeSelf)
        {
            tutorialCursor.SetActive(false);
        }
        if (tutorialCorrect.activeSelf)
        {
            tutorialCorrect.SetActive(false);
        }
        if (tutorialMoreStack.activeSelf)
        {
            tutorialMoreStack.SetActive(false);
        }
    }
   


}
