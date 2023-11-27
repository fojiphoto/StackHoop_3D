using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelListConfig", menuName = "Game Configuration/Level List Config", order = 1)]
public class LevelListConfig : ScriptableObject
{
    [ShowInInspector] public List<LevelConfig> levelList = new List<LevelConfig>();
}
