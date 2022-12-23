using UnityEngine;
using System.Collections.Generic;
using System;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
using System.Linq;

public class GridDisplayCreator : MonoBehaviour {
    [System.Serializable]
    public struct ConnectGrid
    {
        public GridDisplay grid;
        public int column;

        public void Reset ()
        {
            grid = null;
            column = -1;
        }
    }

    private List<GridDisplay> grids = new List<GridDisplay>();
    [SerializeField] private GridDisplay gridPrefab;

    const string savePath = "Assets/Scripts/GameEditor/Map/";

    ConnectGrid origin, target;

    BuildingButtonHandle[] buildingButtonHandles;

    private void Awake() {
        buildingButtonHandles = FindObjectsOfType<BuildingButtonHandle>();
    }

    public GridDisplay CreateGridDisplay ()
    {
        GridDisplay grid = Instantiate(gridPrefab, Vector3.zero, Quaternion.identity, transform);
        grid.SetName(grids.Count + 1);
        grid.DrawGrid();
        grids.Add(grid);

        return grid;
    }
    public GridDisplay CreateGridDisplay (BuildingMap.GridData gridData)
    {
        GridDisplay grid = Instantiate(gridPrefab, gridData.position, gridData.rotation, transform);

        try
        {
            grid.SetName(int.Parse(gridData.name.Split(' ')[1]));
        }
        catch (System.InvalidCastException ex)
        {
            Debug.LogWarning($"grid name {gridData.name} is not correct format/n" + ex.StackTrace);
        }

        grid.DrawGrid();
        grids.Add(grid);

        grid.XDim = gridData.x;
        grid.YDim = gridData.y;

        foreach (var piece in gridData.initialPieces)
        {
            foreach (var buildPiece in buildingButtonHandles)
            {
                if (buildPiece.Piece.Type == piece.type)
                    grid.InitPiece(buildPiece.Piece, piece.x, piece.y);
            }
        }

        foreach (var column in gridData.lossFertilityColumns)
        {
            grid.InitLossFertilityColumn(column);
        }

        return grid;
    }

    public void ConnectGridDisplays (List<BuildingMap.GridData> gridDatas)
    {
        foreach (var gridData in gridDatas)
        {
            if (gridData.targetGrids.Count == 0) continue;

            GridDisplay grid = grids.Where(tempGrid => tempGrid.name == gridData.name).ToArray()[0];

            foreach (var targetData in gridData.targetGrids)
            {
                GridDisplay targetGrid = grids.Where(tempGrid => tempGrid.name == targetData.targetGridName).ToArray()[0];

                foreach (var column in targetData.connectColumns)
                {
                    grid.ConnectGrid(targetGrid, column.originColumn, column.targetColumn);
                }
            }
        }
    }

    public void DestroyGridDisplay (GridDisplay targetGrid)
    {
        bool isReduce = false;
        for (int gridNumber = 0; gridNumber < grids.Count; gridNumber ++)
        {
            if (!isReduce && grids[gridNumber] == targetGrid)
            {
                grids.RemoveAt(gridNumber);
                isReduce = true;
                gridNumber --;
            }
            else if (isReduce)
            {
                grids[gridNumber].SetName(gridNumber + 1);
            }
        }
        Destroy(targetGrid.gameObject);
    }

    public GridDisplay GetGridByName (string name)
    {
        foreach (var gridElement in grids)
        {
            if (gridElement.name == name) return gridElement;
        }

        return null;
    }

    private string GetMapName (bool isNew)
    {
        if (isNew)
        {
            return (Data.MaxLevel() + 1).ToString() + FileUtility.ASSET_EXTENTION;
            // return (DataLevel.data.maps.Count + 1).ToString() + FileUtility.ASSET_EXTENTION;
        }
        else
        {
            if (GameEditorSystem.Instance.FileManager.Map == null)
            {
                throw new System.NullReferenceException("Not selected any map yet");
            }
            else
            {
                return GameEditorSystem.Instance.FileManager.Map.name + FileUtility.ASSET_EXTENTION;
            }
        }
    }

    private BuildingMap GenerateBuildingMap ()
    {
        BuildingMap map;
        switch (GameEditorSystem.Instance.UIEditor.UILevelEditor.LevelSetting.Type)
        {
            case LevelType.MOVES:
                map = ScriptableObject.CreateInstance<BuildingMoveMap>();
                break;
            case LevelType.TIMER:
                map = ScriptableObject.CreateInstance<BuildingTimeLevel>();
                break;
            default:
                throw new System.NotSupportedException("invalute level type");
        }

        map.AddGrids(grids);
        map.AddLevel(GameEditorSystem.Instance.UIEditor.UILevelEditor.LevelSetting);

        return map;
    }

    private void SaveMapToDatabase (BuildingMap map, string mapName)
    {
// #if UNITY_EDITOR
//         AssetDatabase.CreateAsset(map, FileUtility.GetMapPathFromAssetsFolder() + '/' + mapName);
//         AssetDatabase.SaveAssets();

//         EditorUtility.FocusProjectWindow();
//         Selection.activeObject = map;
// #endif

        // StartCoroutine(Data.AddLevel(map, null, null));
    }

    /// <summary>Save to loading level</summary>
    public void SaveLevel (Action onSuccessful = null, Action<string> onFailed = null)
    {
        BuildingMap map = GenerateBuildingMap();
        // string mapName = GetMapName(false);

// #if UNITY_EDITOR
//         AssetDatabase.CreateAsset(map, FileUtility.GetMapPathFromAssetsFolder() + '/' + mapName);
//         AssetDatabase.SaveAssets();

//         EditorUtility.FocusProjectWindow();
//         Selection.activeObject = map;
// #endif

        APIAccessObject.Instance.StartCoroutine(Data.UpdateLevel(new KeyValuePair<int, BuildingMap>(int.Parse(GameEditorSystem.Instance.FileManager.Map.name), map), onSuccessful, onFailed));
    }

    public void GenerateLevel (Action onSuccessful = null, Action<string> onFailed = null)
    {
        BuildingMap map = GenerateBuildingMap();
        // string mapName = GetMapName(true);
        // SaveMapToDatabase(map, mapName);

        APIAccessObject.Instance.StartCoroutine(Data.AddLevel(map, onSuccessful, onFailed));
    }

    public void DeleteLevel (uint levelNumber, Action onSuccessful = null, Action<string> onFailed = null)
    {
        APIAccessObject.Instance.StartCoroutine(Data.DeleteLevel(levelNumber, onSuccessful, onFailed));
    }

    public void ResetGrids ()
    {
        foreach (var gridElement in grids)
        {
            Destroy(gridElement.gameObject);
        }

        grids.Clear();
    }

    public bool CanChooseOrigin() => origin.grid == null;
    public bool CanChooseTarget() => target.grid == null;

    public void ResetChooseColumnConnect ()
    {
        origin.Reset();
        target.Reset();
    }
    public void ResetChooseColumnConnect (bool isOrigin)
    {
        if(isOrigin) origin.Reset();
        else target.Reset();
    }

    public void ChooseColumnConnect (GridDisplay grid, int column, bool isOrigin)
    {
        if (isOrigin /*&& CanChooseOrigin()*/)
        {
            origin.grid = grid;
            origin.column = column;
        }
        else if (!isOrigin /*&& CanChooseTarget()*/)
        {
            target.grid = grid;
            target.column = column;

        }

        if (!CanChooseOrigin() && !CanChooseTarget())
        {
            //connect grid couple;
            var command = new ConnectGridCommand(origin, target);
            command.Execute();
            InputController.Instance.AddCommand(command);

            //reset origin and target couple
            origin.Reset();
            target.Reset();
        }
    }
}