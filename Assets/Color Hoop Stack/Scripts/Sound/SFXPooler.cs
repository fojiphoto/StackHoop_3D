using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPooler : ObjectPooler
{
    public GameObject SpawnNewObject()
    {
        GameObject newGO = Instantiate(objectToPool, transform.position, transform.rotation);
        newGO.transform.SetParent(transform);
        pooledObjects.Push(newGO);
        newGO.SetActive(false);
        return newGO;
    }

    public GameObject GetNextPooledObject(AudioClip clip, bool isLoop)
    {
        if (pooledObjects.Count <= 0)
        {
            SpawnNewObject();
        }
        GameObject GO = pooledObjects.Pop();
        GO.SetActive(true);
        GO.GetComponent<SoundSource>().PlayAudio(clip, isLoop);
        return GO;
    }
}
