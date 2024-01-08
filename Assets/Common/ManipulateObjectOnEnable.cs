using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateObjectOnEnable : MonoBehaviour {

    public List<GameObject> ObjectsToEnable;
    public List<GameObject> ObjectsToDisable;
    public float[] timeToEnable;
    public float[] timeToDisable;

    // Use this for initialization
    private void OnEnable () 
	{
        ManipulateObjects();
    }

    void ManipulateObjects()
    {
        foreach (var item in ObjectsToEnable)
        {
            StartCoroutine(ObjectManipulation(item, timeToEnable[ObjectsToEnable.IndexOf(item)], true));
        }

        foreach (var item in ObjectsToDisable)
        {
            StartCoroutine(ObjectManipulation(item, timeToDisable[ObjectsToDisable.IndexOf(item)], false));
        }
    }

    IEnumerator ObjectManipulation(GameObject obj, float time, bool enableStatus)
    {
        yield return new WaitForSeconds(time);
        if (obj)
        {
            obj.SetActive(enableStatus);
        }
    }
}
