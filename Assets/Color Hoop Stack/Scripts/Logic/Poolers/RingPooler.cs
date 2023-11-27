#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPooler : ObjectPooler
{
#if UNITY_EDITOR
    [Button]
#endif
    public GameObject GetNextRing(RingType ringType)
    {
        GameObject nextRing = GetNextPooledObject();
        nextRing.GetComponent<Ring>().InitRing(ringType);
        return nextRing;
    }
}
