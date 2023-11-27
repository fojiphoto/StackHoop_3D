using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectListPooler : Singleton<ObjectListPooler>
{
    public bool systemPool = true;
    public int initializeSize;
    protected Dictionary<string, Stack<GameObject>> objectListsPooled;

    protected void Awake()
    {
        Instance = this;
        if (systemPool)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        objectListsPooled = new Dictionary<string, Stack<GameObject>>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    public void InitStackPooler(GameObject objectToPool)
    {
        string key = PrefabUtility.GetCorrespondingObjectFromSource(objectToPool).name;
        if (!objectListsPooled.ContainsKey(key))
        {
            objectListsPooled.Add(key, new Stack<GameObject>());

            for (int i = 0; i < initializeSize; i++)
            {
                SpawnNewObject(objectToPool, key);
            }
        }
    }

    private GameObject SpawnNewObject(GameObject objectToPool)
    {
        GameObject newGO = Instantiate(objectToPool, transform.position, transform.rotation);
        newGO.transform.SetParent(transform);
        string key = PrefabUtility.GetCorrespondingObjectFromSource(objectToPool).name;
        objectListsPooled[key].Push(newGO);
        newGO.SetActive(false);
        return newGO;
    }

    private GameObject SpawnNewObject(GameObject objectToPool, string key)
    {
        GameObject newGO = Instantiate(objectToPool, transform.position, transform.rotation);
        newGO.transform.SetParent(transform);
        objectListsPooled[key].Push(newGO);
        newGO.SetActive(false);
        return newGO;
    }

    public virtual GameObject GetNextPooledObject(GameObject objectToPool)
    {
        string key = PrefabUtility.GetCorrespondingObjectFromSource(objectToPool).name;
        if (objectListsPooled[key].Count <= 0)
        {
            SpawnNewObject(objectToPool, key);
        }
        GameObject GO = objectListsPooled[key].Pop();
        GO.SetActive(true);
        return GO;
    }

    public virtual void ReturnPooledObject(GameObject returnObject)
    {
        string key = PrefabUtility.GetCorrespondingObjectFromSource(returnObject).name;
        objectListsPooled[key].Push(returnObject);
        returnObject.SetActive(false);
    }
    */
}
