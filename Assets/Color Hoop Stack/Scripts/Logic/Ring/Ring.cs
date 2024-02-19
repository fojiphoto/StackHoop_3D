using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public RingType ringType = RingType.NONE;
    public MeshRenderer meshRenderer;
    public BoxCollider boxCol;
    [HideInInspector] public bool isMoving = false;

    public void Awake()
    {
       
       

        //transform.rotation = Quaternion.Euler(90, 0, 0);
    }
    private void Start()
    {
        List<int> specialLevels = new List<int> { 8, 11, 13, 17, 23, 26, 31, 39, 41, 44, 47, 51 };
        if (specialLevels.Contains(GameplayMgr.Instance.currentLevel))
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(90, Random.Range(0, 360), 0);
        }
    }
    public void ChangeRingType(RingType ringType)
    {
        this.ringType = ringType;
        meshRenderer.material.color = GameplayMgr.Instance.ringColorConfig.configDic[ringType];
    }

    public void InitRing(RingType ringType)
    {
        ChangeRingType(ringType);
    }
}
