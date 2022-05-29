using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class LoadMapData : MonoBehaviour
{
    public void LoadMapToFirestore()
    {
        // string body = JsonConvert.SerializeObject(new LevelData(DataLevel.data.maps), Formatting.None, new JsonSerializerSettings()
        // {
        //     //inside the quaternion cause Self referencing loop
        //     ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        // });

        StartCoroutine(APIAccesser.LoadAllMapCoroutine(PlayerPrefs.GetString("LevelData", null)));
    }

    public void GetAllMap ()
    {
        LevelData levelData = new LevelData();
        StartCoroutine(levelData.LoadData((upToDateData) => {
            Debug.Log("load successful " + upToDateData.maps.Count + " version: " + upToDateData.version);
        }, (message) => {
            Debug.Log("load failed: " + message);
        }));
        // StartCoroutine(APIAccesser.GetAllMapCoroutine(null, null));
    }
}
