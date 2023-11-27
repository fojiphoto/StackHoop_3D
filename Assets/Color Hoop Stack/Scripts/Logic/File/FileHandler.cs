using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class FileHandler
{
    readonly public string settingFilePath = Application.persistentDataPath + "/settingData.json";
    readonly public string levelFilePath = Application.persistentDataPath + "/levelData.json";

    public bool IsFileExist(string path)
    {
        if (File.Exists(path))
            return true;
        return false;
    }

    public void ReadSettingData()
    {
        string inputText;
        inputText = File.ReadAllText(settingFilePath);

        SettingData settingData = new SettingData();

        settingData = JsonUtility.FromJson<SettingData>(inputText);

        GameManager.Instance.SoundEnable = settingData.enabledSound;
        GameManager.Instance.VibrateEnable = settingData.enabledVibration;
    }

    public void SaveSettingData()
    {
        SettingData settingData
            = new SettingData(
                GameManager.Instance.SoundEnable,
                GameManager.Instance.VibrateEnable
                );

        string jsonData = JsonUtility.ToJson(settingData);
        File.WriteAllText(settingFilePath, jsonData);
    }

    public void SaveSettingDataDefault()
    {
        SettingData settingData = new SettingData(true, true);

        string jsonData = JsonUtility.ToJson(settingData);
        File.WriteAllText(settingFilePath, jsonData);
    }

    public void SaveLevelData()
    {
        LevelData levelData = new LevelData(
            false,
            GameplayMgr.Instance.mapDataStack, 
            GameplayMgr.Instance.currentLevel, 
            new MapData(GameplayMgr.Instance.ringStackList, GameplayMgr.Instance.stackCompleteNumber, GameplayMgr.Instance.ringStackList.Count), 
            GameplayMgr.Instance.ringTypeNumber
            );

        string jsonData = JsonUtility.ToJson(levelData);
        File.WriteAllText(levelFilePath, jsonData);
    }

    public void SaveLevelDataDefault()
    {
        LevelData levelData = new LevelData();

        string jsonData = JsonUtility.ToJson(levelData);
        File.WriteAllText(levelFilePath, jsonData);
    }

    public void LoadLevelData()
    {
        if (IsFileExist(levelFilePath))
        {
            string input = File.ReadAllText(levelFilePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(input);
            GameplayMgr.Instance.currentLevel = levelData.currentLevel;
            levelData.mapDataStack.Reverse();
            GameplayMgr.Instance.mapDataStack = new Stack<MapData>(levelData.mapDataStack);
            GameplayMgr.Instance.ringTypeNumber = levelData.ringTypeNumber;
            if ((!levelData.blankData) && (levelData.mapDataStack.Count > 0))
                GameplayMgr.Instance.LoadLevelMapData(levelData.mapDataCurrent);
        }
    }
}
