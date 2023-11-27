using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RingStackData
{
    public List<RingType> ringList;
    public bool canControl;
    public int number;

    public RingStackData()
    {
        this.ringList = new List<RingType>();
        this.canControl = true;
        this.number = 0;
    }

    public RingStackData(RingStack ringStack)
    {
        this.canControl = ringStack.canControl;
        this.number = ringStack.number;
        this.ringList = new List<RingType>();

        if (ringStack.ringStack.Count > 0)
        {
            foreach (Ring ring in ringStack.ringStack)
            {
                this.ringList.Add(ring.ringType);
            }
        }
    }
}
