using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public bool blankData = true;
    public List<MapData> mapDataStack;
    public int currentLevel;
    public MapData mapDataCurrent;
    public int ringTypeNumber;

    public LevelData()
    {
        this.blankData = true;
        this.mapDataStack = new List<MapData>();
        this.currentLevel = 0;
        this.mapDataCurrent = new MapData();
        this.ringTypeNumber = 0;
    }

    public LevelData(bool blankData, Stack<MapData> mapDataStack, int currentLevel, MapData mapDataCurrent, int ringTypeNumber)
    {
        this.blankData = blankData;
        this.mapDataStack = new List<MapData>(mapDataStack);
        this.currentLevel = currentLevel;
        this.mapDataCurrent = mapDataCurrent;
        this.ringTypeNumber = ringTypeNumber;
    }
}
