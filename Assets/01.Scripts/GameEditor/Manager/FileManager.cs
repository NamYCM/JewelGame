using System.Collections;
using System.Collections.Generic;
// using UnityEditor;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    // string path;

    BuildingMap map;
    public BuildingMap Map => map;

    GameEditorSystem gameEditorSystem;

    private void Start() {
        gameEditorSystem = GameEditorSystem.Instance;
    }

    // private void GetMap ()
    // {
    //     if (string.IsNullOrEmpty(path)) return;

    //     LoadMapData();

    //     gameEditorSystem.GenerateMap(map);
    // }

//     //TODO change this function
//     private void LoadMapData ()
//     {
// #if UNITY_EDITOR
//         string assetPath = GetAssetPath(path);
//         map = (BuildingMap)AssetDatabase.LoadAssetAtPath(assetPath, typeof(BuildingMap));

//         if (map == null)
//         {
//             Debug.LogWarning($"file at {assetPath} is incorrect");
//             throw new System.InvalidCastException($"file at {assetPath} is incorrect");
//         }
// #endif
//     }

    /// <summary>Get path from asset folder by computer path</summary>
    // private string GetAssetPath (string path)
    // {
    //     string assetPath = "";
    //     string[] folders = path.Split('/');
    //     int beginFolder;

    //     for (beginFolder = 0; beginFolder < folders.Length; beginFolder++)
    //     {
    //         if (folders[beginFolder] == "Assets")
    //         {
    //             assetPath = folders[beginFolder];
    //             break;
    //         }
    //     }

    //     for (beginFolder = beginFolder  + 1 ; beginFolder < folders.Length; beginFolder ++)
    //     {
    //         assetPath += "/" + folders[beginFolder];
    //     }

    //     return assetPath;
    // }

    public void OpenExplorer ()
    {
        int max = Data.MaxLevel();
        List<string> levels = new List<string>();
        for (var level = 0; level < max; level++)
        {
            levels.Add((level + 1).ToString());
        }

        gameEditorSystem.UIEditor.ModelWindow.StartBuild.SetTitle("Choose level")
            .SetMessage("Which level do you want to load?")
            .SetDropDownInput(levels)
            .OnConfirmAction(() => {
                var level = int.Parse(gameEditorSystem.UIEditor.ModelWindow.GetInputValue());
                map = Data.GetMapInfor(level);
                map.name = level.ToString();
                gameEditorSystem.GenerateMap(map);
            })
            .OnDeclineAction(() => {})
            .Show();
        // Debug.Log(Data.MaxLevel());

        // path = EditorUtility.OpenFilePanel("Open map", FileUtility.GetDefaultMapPath(), "asset");

        // GetMap();
    }

    public void ResetData ()
    {
        // path = null;
        map = null;
    }
}
