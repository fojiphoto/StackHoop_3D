using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public List<RingStackData> listRingStack;
    public int stackCompleteNumber = 0;
    public int ringStackNumber = 0;

    public List<RingStackData> ListRingStack { get => listRingStack; set => listRingStack = value; }
    public int StackCompleteNumber { get => stackCompleteNumber; set => stackCompleteNumber = value; }
    public int RingStackNumber { get => ringStackNumber; set => ringStackNumber = value; }

    public MapData(List<RingStack> listRingStack, int stackCompleteNumber, int ringStackNumber)
    {
        this.StackCompleteNumber = stackCompleteNumber;
        this.ringStackNumber = ringStackNumber;
        this.ListRingStack = new List<RingStackData>();

        foreach (RingStack ringStack in listRingStack)
        {
            RingStackData ringStackData = new RingStackData(ringStack);
            this.ListRingStack.Add(ringStackData);
        }
    }

    public MapData()
    {
        this.ListRingStack = new List<RingStackData>();
    }
}
