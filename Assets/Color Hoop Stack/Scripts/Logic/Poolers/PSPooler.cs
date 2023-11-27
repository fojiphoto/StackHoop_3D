#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSPooler : ObjectPooler
{
#if UNITY_EDITOR
    [Button]
#endif
    public GameObject GetNextPS()
    {
        GameObject nextGO = GetNextPooledObject();
        ParticleSystem particleSystem = nextGO.GetComponent<ParticleSystem>();
        particleSystem.Play(true);
        return nextGO;
    }
}
