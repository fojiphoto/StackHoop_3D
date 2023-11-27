using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolerMgr : Singleton<PoolerMgr>
{
    public RingPooler ringPooler;
    public RingStackPooler ringStackPooler;
    public PSPooler VFXCompletePooler;
    public CorrectPooler correctPooler;
}
