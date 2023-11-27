#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingStackPooler : ObjectPooler
{
#if UNITY_EDITOR
    [Button]
#endif
    public GameObject GetNextRingStack()
    {
        GameObject nextRing = GetNextPooledObject();
        return nextRing;
    }
}
