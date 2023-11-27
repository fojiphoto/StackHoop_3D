using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(fileName = "RingColorConfig", menuName = "Game Configuration/Ring Color Config", order = 1)]
public class RingColorConfig : ScriptableObject
{
    public KVPList<RingType, Color> configDic;
}
