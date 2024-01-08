using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateObjectsOnDisable : MonoBehaviour
{
    public List<GameObject> ObjectsToEnable;
    public List<GameObject> ObjectsToDisable;

    // Use this for initialization
    void OnDisable()
    {
        foreach (GameObject gameObject in ObjectsToEnable)
        {
            gameObject.SetActive(true);
        }
        foreach (GameObject gameObject in ObjectsToDisable)
        {
            gameObject.SetActive(false);
        }
    }
}
