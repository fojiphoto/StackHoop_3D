using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public void OnParticleSystemStopped()
    {
        VFXMgr.Instance.DestroyVFX(this.gameObject);
    }
}
