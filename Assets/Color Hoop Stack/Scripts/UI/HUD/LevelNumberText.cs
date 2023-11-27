using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelNumberText : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Awake()
    {
        EventDispatcher.Instance.RegisterListener(EventID.ON_INIT_LEVEL, param => UpdateLevelNumber());
    }

    public void UpdateLevelNumber()
    {
        text.text = "";
        int currentLevel = GameplayMgr.Instance.currentLevel + 1;
        while (currentLevel != 0)
        {
            text.text = UpdateLevelNumberDigit(currentLevel % 10) + text.text;
            currentLevel /= 10;
        }
        text.ForceMeshUpdate(true);
    }

    private string UpdateLevelNumberDigit(int digit)
    {
        return "<sprite index=" + digit.ToString() + ">";
    }
}
