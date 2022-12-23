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
                gameEditorSystem.ActiveDeleteMapButton();
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
        gameEditorSystem.DeactiveDeleteMapButton();
    }
}
