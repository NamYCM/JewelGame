using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;

[Serializable]
public class LevelData
{
    const string DATA_KEY = "LevelData";

    public int version = 0;
    public Dictionary<int, MapLevelData> maps;

    public LevelData () {}

    public LevelData (List<MapData> listMaps)
    {
        maps = new Dictionary<int, MapLevelData>();

        for (var i = 0; i < listMaps.Count; i++)
        {
            // MapLevelData levelData = null;

            // if (listMaps[i].Map.StarScore.LevelType == LevelType.MOVES)
            // {
            //     levelData = new MoveMapData();
            //     ((MoveMapData)levelData).InitData(listMaps[i].Map);
            // }
            // else if (listMaps[i].Map.StarScore.LevelType == LevelType.TIMER)
            // {
            //     levelData = new TimeMapData();
            //     ((TimeMapData)levelData).InitData(listMaps[i].Map);
            // }
            // else
            // {
            //     throw new Exception("obstacle level is not supported so far");
            // }

            // maps.Add(i + 1, levelData);
            maps.Add(i + 1, new MapLevelData(listMaps[i].Map));
        }
    }

    private LevelData GetDataFromPlayerPrefs ()
    {
        string jsonData = PlayerPrefs.GetString(DATA_KEY, null);
        if (jsonData == null) return null;

        return JsonConvert.DeserializeObject<LevelData>(jsonData);
    }

    private void SaveDataToPlayerPrefs (LevelData levelData)
    {
        PlayerPrefs.SetString(DATA_KEY, JsonConvert.SerializeObject(levelData, Formatting.None, new JsonSerializerSettings()
        {
            //inside the quaternion cause Self referencing loop
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        }));
    }

    private IEnumerator GetDataFromFirestore (Action<LevelData> onSucessfulGet, Action<string> onFailedGet)
    {
        yield return APIAccesser.GetAllMapCoroutine((levelData) => {
            SaveDataToPlayerPrefs(levelData);
            onSucessfulGet?.Invoke(levelData);
        }, onFailedGet);
    }

    private IEnumerator GetCurrentVersion (Action<LevelData> onSucessfulGet, Action<string> onFailedGet)
    {
        yield return APIAccesser.GetCurrentVersionOfMapCoroutine(onSucessfulGet, onFailedGet);
    }

    public IEnumerator LoadData (Action<LevelData> onSucessfulLoadData, Action<string> onFailedLoadData)
    {
        var data = GetDataFromPlayerPrefs();

        //the first time load game
        if (data == null)
        {
            Debug.Log("data is null, load from firestore");
            //load data from firestore
            yield return GetDataFromFirestore(onSucessfulLoadData, onFailedLoadData);
            yield break;
        }
        else
        {
            LevelData serverData = null;
            //check version
            yield return GetCurrentVersion((levelData) => {
                serverData = levelData;
            }, onFailedLoadData);

            if (serverData.version != data.version)
            {
                Debug.Log("data is obsoleted, load from firestore");
                yield return GetDataFromFirestore(onSucessfulLoadData, onFailedLoadData);
            }
            else
            {
                Debug.Log("load from playerprefs");
                onSucessfulLoadData?.Invoke(data);
            }
        }
    }

    public IEnumerator LoadDataFromFirebase (Action<LevelData> onSucessfulLoadData, Action<string> onFailedLoadData)
    {
        yield return GetDataFromFirestore(onSucessfulLoadData, onFailedLoadData);
        // var data = GetDataFromPlayerPrefs();

        // //the first time load game
        // if (data == null)
        // {
        //     Debug.Log("data is null, load from firestore");
        //     //load data from firestore
        //     yield return GetDataFromFirestore(onSucessfulLoadData, onFailedLoadData);
        //     yield break;
        // }
        // else
        // {
        //     LevelData serverData = null;
        //     //check version
        //     yield return GetCurrentVersion((levelData) => {
        //         serverData = levelData;
        //     }, onFailedLoadData);

        //     if (serverData.version != data.version)
        //     {
        //         Debug.Log("data is obsoleted, load from firestore");
        //         yield return GetDataFromFirestore(onSucessfulLoadData, onFailedLoadData);
        //     }
        //     else
        //     {
        //         Debug.Log("load from playerprefs");
        //         onSucessfulLoadData?.Invoke(data);
        //     }
        // }
    }
}

[Serializable]
public class MapLevelData
{
    public List<BuildingMap.GridData> grids;
    public BuildingMap.StarScoreData starScore;
    public BuildingMoveMap.MoveLevelData moveMapData = new BuildingMoveMap.MoveLevelData();
    public BuildingTimeLevel.TimeLevelData timeMapData = new BuildingTimeLevel.TimeLevelData();

    public MapLevelData () {}

    public MapLevelData (BuildingMap mapData)
    {
        InitData(mapData);
    }

    public virtual void InitData (BuildingMap mapData)
    {
        grids = mapData.Grids;
        starScore = mapData.StarScore;

        if (mapData.StarScore.LevelType == LevelType.MOVES)
        {
            moveMapData = ((BuildingMoveMap)mapData).LevelData;
        }
        else if (mapData.StarScore.LevelType == LevelType.TIMER)
        {
            timeMapData = ((BuildingTimeLevel)mapData).LevelData;
        }
        else
        {
            throw new Exception("obstacle level is not supported so far");
        }
    }

    // public void ConvertFromBuildingMap (BuildingMap buildingMap)
    // {
    //     MapLevelData levelData = null;

    //     if (listMaps[i].Map.StarScore.LevelType == LevelType.MOVES)
    //     {
    //         levelData = new MoveMapData();
    //         ((MoveMapData)levelData).InitData(listMaps[i].Map);
    //     }
    //     else if (listMaps[i].Map.StarScore.LevelType == LevelType.TIMER)
    //     {
    //         levelData = new TimeMapData();
    //         ((TimeMapData)levelData).InitData(listMaps[i].Map);
    //     }
    //     else
    //     {
    //         throw new Exception("obstacle level is not supported so far");
    //     }
    // }

    public BuildingMap ConvertToBuildingMap ()
    {
        BuildingMap buildingMap;
        if (starScore.LevelType == LevelType.MOVES)
        {
            buildingMap = new BuildingMoveMap();
            ((BuildingMoveMap)buildingMap).SetLevelData(moveMapData);
        }
        else if (starScore.LevelType == LevelType.TIMER)
        {
            buildingMap = new BuildingTimeLevel();
            ((BuildingTimeLevel)buildingMap).SetLevelData(timeMapData);
        }
        else
        {
            throw new Exception("obstacle map is not supported so far");
        }

        buildingMap.SetGrids(grids);
        buildingMap.SetStarScore(starScore);

        return buildingMap;
    }
}

// [Serializable]
// public class MoveMapData : MapLevelData
// {

//     public override void InitData(BuildingMap mapData)
//     {
//         base.InitData(mapData);

//         moveMapData = ((BuildingMoveMap)mapData).LevelData;
//     }
// }

// [Serializable]
// public class TimeMapData : MapLevelData
// {

//     public override void InitData(BuildingMap mapData)
//     {
//         base.InitData(mapData);

//         timeMapData = ((BuildingTimeLevel)mapData).LevelData;
//     }
// }