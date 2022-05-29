using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "DataObject", menuName = "DataObject", order = 0)]
public class DataObject : ScriptableObject {
    [SerializeField] public List<MapData> maps;
    [SerializeField] int currentLevel = 1;
    public int CurrentLevel
    {
        get { return currentLevel; }
        set
        {
            if (value <= 0 || value > MapAmount())
                throw new System.IndexOutOfRangeException($"level {value} is not exists");

            currentLevel = value;
        }
    }

    private void OnValidate() {
        try
        {
            if (maps.Count == 0)
            {
                InitDataObject();
                return;
            }
        }
        catch (System.StackOverflowException)
        {
            Debug.LogWarning("don't have any level in " + FileUtility.MAP_PATH);
        }

        for (int levelCount = 0; levelCount < maps.Count; levelCount ++)
        {
            if (maps[levelCount].LevelNumber != levelCount)
            {
                maps[levelCount].UpdateLevelNumber(levelCount);
            }

            //reset score if that level is locked
            if (maps[levelCount].State == MapData.LevelState.Locked && maps[levelCount].Score != 0)
                maps[levelCount].SetScore(0);
        }
    }

    private MapData GetMapDataByLevel (int level)
    {
        return maps[level - 1];
    }

    private string GetLevelPath (int level)
    {
        return  FileUtility.MAP_PATH + '/' + level;
    }

    public void InitDataObject ()
    {
        // maps = AssetDatabase.LoadAllAssetsAtPath(FileUtility.GetMapPathFromAssetsFolder()).Select(
        //     map => new MapData() {
        //         LevelNumber = int.Parse(map.name) - 1,
        //         Map = (BuildingMap)map,
        //         IsLock = true
        //     }).ToList();

        //TODO change to another way which is optimizer
        int level = 1;
        BuildingMap buildingMap = Resources.Load<BuildingMap>(GetLevelPath(level));
        MapData mapData;

        while (buildingMap != null)
        {
            mapData = new MapData(buildingMap);
            maps.Add(mapData);

            level ++;
            buildingMap = Resources.Load<BuildingMap>(GetLevelPath(level));
        }
    }

    public int MapAmount () => maps.Count;

    public void AddLevel (BuildingMap map)
    {
        int level = int.Parse(map.name);

        if (level <= maps.Count)
        {
            GetMapDataByLevel(level).UpdateMap(map);
        }
        else
            maps.Add(new MapData(map));
    }

    public bool IsLock (int level)
    {
        return GetMapDataByLevel(level).State == MapData.LevelState.Locked;
    }
    public bool IsAbleToOpen (int level)
    {
        return GetMapDataByLevel(level).State == MapData.LevelState.AbleToOpen;
    }
    public bool IsOpened (int level)
    {
        return GetMapDataByLevel(level).State == MapData.LevelState.Opened;
    }

    public BuildingMap GetMapInfor (int level)
    {
        return GetMapDataByLevel(level).Map;
    }

    public int GetScore (int level)
    {
        return GetMapDataByLevel(level).Score;
    }

    public void UpdateScore (int level, int score)
    {
        GetMapDataByLevel(level).SetScore(score);
    }

    /// <summary>open level which has able to open state</summary>
    public void OpenLevel (int level)
    {
        if (GetMapDataByLevel(level).State != MapData.LevelState.AbleToOpen)
        {
            throw new System.InvalidOperationException($"state of level {level} is {GetMapDataByLevel(level).State}, can not open");
        }

        GetMapDataByLevel(level).SetLevelState(MapData.LevelState.Opened);
    }

    /// <summary>unlock from locked level to able to open level</summary>
    public void UnlockLevel (int level)
    {
        if (GetMapDataByLevel(level).State != MapData.LevelState.Locked) return;

        GetMapDataByLevel(level).SetLevelState(MapData.LevelState.AbleToOpen);
    }
}