using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public void EnableTutorial()
    {
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
        else if (GameplayMgr.Instance.currentLevel == 9)
        {
            tutorialMoreStack.SetActive(true);
            tutorialCursor.SetActive(true);
        }
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
