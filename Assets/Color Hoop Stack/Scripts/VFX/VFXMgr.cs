using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXMgr : Singleton<VFXMgr>
{
    [SerializeField] private PSPooler VFXPooler;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateVFX()
    {
        VFXPooler.GetNextPS();
    }

    public void DestroyVFX(GameObject VFX)
    {
        VFXPooler.ReturnPooledObject(VFX);
    }
}
