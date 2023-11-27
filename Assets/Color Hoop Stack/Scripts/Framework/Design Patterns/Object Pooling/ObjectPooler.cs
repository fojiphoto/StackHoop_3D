#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject objectToPool;
    public int initializeSize;
    protected Stack<GameObject> pooledObjects;

    private void Awake()
    {
        pooledObjects = new Stack<GameObject>();
        for (int i = 0; i < initializeSize; i++)
        {
            SpawnNewObject();
        }
    }

    public virtual GameObject SpawnNewObject()
    {
        GameObject newGO = Instantiate(objectToPool, transform.position, transform.rotation);
        newGO.transform.SetParent(transform);
        pooledObjects.Push(newGO);
        newGO.SetActive(false);
        return newGO;
    }

#if UNITY_EDITOR
    [Button]
#endif
    public virtual GameObject GetNextPooledObject()
    {
        if (pooledObjects.Count <= 0)
        {
            SpawnNewObject();
        }
        GameObject GO = pooledObjects.Pop();
        GO.SetActive(true);
        return GO;
    }

#if UNITY_EDITOR
    [Button]
#endif
    public virtual void ReturnPooledObject(GameObject returnObject)
    {
        pooledObjects.Push(returnObject);
        returnObject.SetActive(false);
    }
}
