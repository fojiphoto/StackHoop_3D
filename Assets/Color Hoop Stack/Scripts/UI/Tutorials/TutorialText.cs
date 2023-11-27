using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI context;

    // Start is called before the first frame update
    private void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_CHANGE_TUTORIAL_TEXT, param => ChangeText());
        EventDispatcher.Instance.RegisterListener(EventID.ON_RESET_TUTORIAL_TEXT, param => ResetText());
    }

    public void ChangeText()
    {
        if (GameplayMgr.Instance.currentLevel == 0)
        {
            context.text = "Tap the right stack to put rings into it";
        }
        else if (GameplayMgr.Instance.currentLevel == 1)
        {
            context.text = "Only sort the hoop into the same color";
        }
    }

    public void ResetText()
    {
        context.text = "Tap the left stack to pick its rings up";
    }
}
