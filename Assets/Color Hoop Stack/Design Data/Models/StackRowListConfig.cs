using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StackRowListConfig", menuName = "Game Configuration/Stack Row List Config", order = 1)]
public class StackRowListConfig : ScriptableObject
{
    public List<StackRowConfig> stackRowList = new List<StackRowConfig>();
}
