using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RingTypeConfig", menuName = "Game Configuration/SFX Config", order = 1)]
public class SFXListConfig : ScriptableObject
{
    public KVPList<SFXType, AudioClip> sfxConfigDic;
}
