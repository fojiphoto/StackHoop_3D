#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectPooler : ObjectPooler
{
    public RectTransform correctorParent;

#if UNITY_EDITOR
    [Button]
#endif
    public Corrector GetNextCorrector(bool isTrue)
    {
        GameObject GO = GetNextPooledObject();
        Corrector corrector = GO.GetComponent<Corrector>();
        corrector.SetCorrect(isTrue);
        if (correctorParent.gameObject.activeSelf)
        {
            corrector.rectTransform.SetParent(correctorParent);
        }
        corrector.rectTransform.localScale = Vector3.one;
        return corrector;
    }

#if UNITY_EDITOR
    [Button]
#endif
    public Corrector GetNextCorrector(bool isTrue, Vector2 position)
    {
        Corrector corrector = GetNextCorrector(isTrue);
        corrector.rectTransform.anchoredPosition3D = new Vector3(position.x, position.y, 0f);
        return corrector;
    }
}
