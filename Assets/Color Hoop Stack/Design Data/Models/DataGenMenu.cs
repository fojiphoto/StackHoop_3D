using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class DataGenMenu : OdinEditorWindow
{
    [MenuItem("Tools/Color Stack Hoop/Data Generator")]
    private static void ShowWindow()
    {
        Init();
    }

    private static void Init()
    {
        GetWindow<DataGenMenu>(false, "Data Generator", true);
    }

    [Button]
    private void GenerateLevelsData()
    {
        string levelDataPath = "Assets/Color Hoop Stack/Design Data/LevelListConfig.asset";
        bool assetExists = AssetDatabase.GetMainAssetTypeAtPath(levelDataPath) != null;
        if (!assetExists)
        {
            string inputText;
            string settingFilePath = "Assets/Resources/Data/level.json";
            JsonTextReader reader;

            inputText = File.ReadAllText(settingFilePath);
            reader = new JsonTextReader(new StringReader(inputText));

            LevelListConfig levelListConfig = ScriptableObject.CreateInstance<LevelListConfig>();

            int stackNumber = -1;
            bool canReadLevel = false;
            bool canReadRing = false;
            LevelConfig currentLevelConfig = new LevelConfig();
            RingStackList currentRingStackList = new RingStackList();

            while (reader.Read())
            {
                if (reader.Value!=null)
                {
                    if (reader.Value.ToString() == "data")
                    {
                        if (!canReadLevel)
                            canReadLevel = true;
                    }
                    else if (reader.TokenType == JsonToken.Integer)
                    {
                        int ringType;
                        System.Int32.TryParse(reader.Value.ToString(), out ringType);
#if UNITY_EDITOR
                        Utils.Common.Log("value = " + ((RingType)ringType).ToString());
#endif
                        if (canReadLevel && canReadRing)
                        {
                            if (stackNumber == 3)
                            {
                                stackNumber = 0;
                                currentLevelConfig.ringStackList.Add(currentRingStackList);
                                currentRingStackList = new RingStackList();
                            }
                            else
                            {
                                stackNumber++;
                            }

                            currentRingStackList.ringList.Add((RingType)ringType);
                        }
                    }
                }
                else
                {
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        if (canReadLevel)
                        {
                            canReadRing = true;
                            currentLevelConfig = new LevelConfig();
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndArray)
                    {
                        if (canReadLevel)
                        {
                            canReadRing = false;
                            levelListConfig.levelList.Add(currentLevelConfig);
                        }
                    }
                }    
            }

            //GenerateRandomColor(levelListConfig);

            AssetDatabase.CreateAsset(levelListConfig, levelDataPath);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = levelListConfig;
        }
    }

    [Button]
    private void ClearLevelsData()
    {
        string levelDataPath = "Assets/Color Hoop Stack/Design Data/LevelListConfig.asset";
        bool assetExists = AssetDatabase.GetMainAssetTypeAtPath(levelDataPath) != null;
        if (assetExists)
        {
            AssetDatabase.DeleteAsset(levelDataPath);
        }

    }

    private void GenerateRandomColor(LevelListConfig levelListConfig)
    {
        foreach(LevelConfig levelConfig in levelListConfig.levelList)
        {
            GenerateRandomColorInLevel(levelConfig);
        }
    }

    private void GenerateRandomColorInLevel(LevelConfig levelConfig)
    {
        List<RingType> ringTypeAvailableList = new List<RingType>();

        ringTypeAvailableList.Add(RingType.RANDOM_1);
        ringTypeAvailableList.Add(RingType.GREEN_LIME);
        ringTypeAvailableList.Add(RingType.GREEN_GRASS);
        ringTypeAvailableList.Add(RingType.BLUE_SKY);
        ringTypeAvailableList.Add(RingType.BLUE_OCEAN);
        ringTypeAvailableList.Add(RingType.VIOLET);
        ringTypeAvailableList.Add(RingType.GRAY);
        ringTypeAvailableList.Add(RingType.PINK);
        ringTypeAvailableList.Add(RingType.RED);
        ringTypeAvailableList.Add(RingType.BLACK);
        ringTypeAvailableList.Add(RingType.ORANGE);
        ringTypeAvailableList.Add(RingType.RANDOM_2);

        List<RingType> ringTypeRandomList = new List<RingType>();

        foreach (RingStackList ringStackList in levelConfig.ringStackList)
        {
            foreach(RingType ringType in ringStackList.ringList)
            {
                if ((ringType != RingType.RANDOM_1) && (ringType != RingType.RANDOM_2))
                {
                    for (int i = 0; i < ringTypeAvailableList.Count; i++)
                    {
                        if (ringTypeAvailableList[i] == ringType)
                            continue;
                        ringTypeAvailableList.Remove(ringType);
                        break;
                    }
                }
                else
                {
                    if (ringTypeRandomList.Count == 2)
                        break;
                    if (ringTypeRandomList.Count == 0)
                    {
                        ringTypeRandomList.Add(ringType);
                        break;
                    }
                    if (ringTypeRandomList.Count == 1)
                    {
                        if ((ringTypeRandomList[0] == RingType.RANDOM_1) && (ringType == RingType.RANDOM_1))
                            break;
                        else if ((ringTypeRandomList[0] == RingType.RANDOM_2) && (ringType == RingType.RANDOM_2))
                            break;
                        else
                            ringTypeRandomList.Add(ringType);
                    }
                }
            }
        }

        if (ringTypeRandomList.Count > 0)
        {
            List<RingType> ringTypeRandomNewList = new List<RingType>();

            Debug.Log("random output number = " + ringTypeRandomList.Count + ", ring input number = " + ringTypeAvailableList.Count + " !");
            for(int i = 0; i < ringTypeRandomList.Count; i++)
            {
                if (ringTypeAvailableList.Count == 0)
                    break;
                RingType ringRandom = ringTypeAvailableList[(int)Random.Range(0, ringTypeAvailableList.Count - 1)];
                ringTypeRandomNewList.Add(ringRandom);
                ringTypeAvailableList.Remove(ringRandom);
            }

            for (int i = 0; i < levelConfig.ringStackList.Count; i++)
            {
                for (int j = 0; j < levelConfig.ringStackList[i].ringList.Count; j++)
                {
                    for (int k = 0; k < ringTypeRandomList.Count; k++)
                    {
                        if (levelConfig.ringStackList[i].ringList[j] == ringTypeRandomList[k])
                        {
                            if (k < ringTypeRandomNewList.Count)
                            {
                                levelConfig.ringStackList[i].ringList[j] = ringTypeRandomNewList[k];
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif