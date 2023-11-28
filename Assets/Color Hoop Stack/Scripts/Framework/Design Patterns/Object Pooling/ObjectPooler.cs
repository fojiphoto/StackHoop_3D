#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject objectToPool;
    // public GameObject objectToPool1;
    public int initializeSize;
    protected Stack<GameObject> pooledObjects;
   
    private void Awake()
    {
         // int currentLevel = GameplayMgr.Instance.currentLevel + 1;
        pooledObjects = new Stack<GameObject>();
        for (int i = 0; i < initializeSize; i++)
        {
             SpawnNewObject();
            // if (currentLevel%5==0)
            // {
            //      SpawnNewObject(objectToPool1);
            // }
            // else
            // {
            //      SpawnNewObject(objectToPool);
            // }
           
        }
    }
    // private void Update() {
    //      int currentLevel = GameplayMgr.Instance.currentLevel + 1;
    //     if (currentLevel%5==0)
    //         {
    //              SpawnNewObject(objectToPool1);
    //         }
    //         else
    //         {
    //              SpawnNewObject(objectToPool);
    //         }
    // }

    // public virtual GameObject SpawnNewObject(GameObject prefab)
    // {
    //    // int currentLevel = GameplayMgr.Instance.currentLevel + 1;
        
    //          GameObject newGO = Instantiate(prefab, transform.position, transform.rotation);
    //          Debug.Log("this method is called");
    //             newGO.transform.SetParent(transform);
    //             pooledObjects.Push(newGO);
    //             newGO.SetActive(false);
    //             return newGO;
        
        
        
    // }

    public virtual GameObject SpawnNewObject()
    {
       // int currentLevel = GameplayMgr.Instance.currentLevel + 1;
        
             GameObject newGO = Instantiate(objectToPool, transform.position, transform.rotation);
             Debug.Log("this method is called");
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
            //SpawnNewObject(objectToPool);
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
