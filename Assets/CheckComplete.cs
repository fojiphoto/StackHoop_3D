using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CheckComplete : MonoBehaviour
{
    public static CheckComplete Instance;
    public List<GameObject> RingStack = new List<GameObject>();
    // Start is called before the first frame update
    //IEnumerator Start()
    //{
    //    yield return new WaitForSeconds(1);
    //    for (int i = 0; i < this.transform.childCount; i++)
    //    {
    //        yield return new WaitForSeconds(0.5f);

    //        RingStack.Add(this.transform.GetChild(i).gameObject);
    //    }
    //}

    public void CheckFinalRocketsToFly()
    {
        for (int i = 0; i < RingStack.Count; i++)
        {
            if (RingStack[i].activeSelf)
            {
                RingStack[i].transform.DOMoveY(20, 1)
                     .SetEase(Ease.Linear);
                Utils.Common.Log("Moving Last Stack");
            }
        }
    }

    public void StartInit()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            RingStack.Add(this.transform.GetChild(i).gameObject);
        }
    }
}
