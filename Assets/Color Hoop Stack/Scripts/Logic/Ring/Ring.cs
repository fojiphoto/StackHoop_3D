using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public RingType ringType = RingType.NONE;
    public MeshRenderer meshRenderer;
    public MeshRenderer[] meshRenderer_;
    public BoxCollider boxCol;
    [HideInInspector] public bool isMoving = false;
    public GameObject anim;

    public void Awake()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void ChangeRingType(RingType ringType)
    {
        this.ringType = ringType;
        meshRenderer.material.color = GameplayMgr.Instance.ringColorConfig.configDic[ringType];
        for (int i = 0; i < meshRenderer_.Length; i++)
        {
            meshRenderer_[i].material.color = GameplayMgr.Instance.ringColorConfig.configDic[ringType];
        }
        
    }

    public void InitRing(RingType ringType)
    {
        ChangeRingType(ringType);
    }
    public void ringAnimOff()
    {
        anim.SetActive(false);
    }
    
}
