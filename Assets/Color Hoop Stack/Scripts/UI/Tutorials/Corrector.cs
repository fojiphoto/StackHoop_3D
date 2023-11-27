#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corrector : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameObject iconCorrect;
    public GameObject iconIncorrect;

#if UNITY_EDITOR
    [Button]
#endif
    public void SetCorrect(bool isTrue)
    {
        if (isTrue)
        {
            iconCorrect.SetActive(true);
            iconIncorrect.SetActive(false);
            return;
        }
        else
        {
            iconCorrect.SetActive(false);
            iconIncorrect.SetActive(true);
            return;
        }
    }
}
