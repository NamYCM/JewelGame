using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "BuildingMap", menuName = "Build/BuildingMap", order = 0)]
[Serializable]
public class BuildingMap : ScriptableObject {
    [SerializeField] List<GridData> grids = new List<GridData>();
    public List<GridData> Grids => grids;

    [SerializeField] StarScoreData starScore = new StarScoreData();
    public StarScoreData StarScore => starScore;

    public void SetGrids (List<GridData> gridsData)
    {
        grids = gridsData;
    }

    public void SetStarScore (StarScoreData starScoreData)
    {
        starScore = starScoreData;
    }

    public virtual void AddLevel (UILevelSetting levelSetting)
    {
        starScore.Star1Score = levelSetting.Star1Score;
        starScore.Star2Score = levelSetting.Star2Score;
        starScore.Star3Score = levelSetting.Star3Score;
        starScore.LevelType = levelSetting.Type;
    }

    public void AddGrids (List<GridDisplay> gridDisplays)
    {
        for (int gridCount = 0; gridCount < gridDisplays.Count; gridCount ++)
        {
            GridData gridData = new GridData();
            gridData.X = gridDisplays[gridCount].XDim;
            gridData.Y = gridDisplays[gridCount].YDim;
            gridData.Position = gridDisplays[gridCount].transform.position;
            gridData.Rotation = gridDisplays[gridCount].transform.rotation;
            gridData.InitialPieces = new GridPlay.PiecePosition[gridDisplays[gridCount].InitialPieces.Count];
            for (int pieceCount = 0; pieceCount < gridDisplays[gridCount].InitialPieces.Count; pieceCount ++)
            {
                GridPlay.PiecePosition pieceData = new GridPlay.PiecePosition();
                pieceData.x = gridDisplays[gridCount].InitialPieces[pieceCount].x;
                pieceData.y = gridDisplays[gridCount].InitialPieces[pieceCount].y;
                pieceData.type = gridDisplays[gridCount].InitialPieces[pieceCount].Piece.Type;

                gridData.InitialPieces[pieceCount] = pieceData;
            }

            gridData.Name = gridDisplays[gridCount].name;

            gridData.TargetGrids = new List<ConnectData>();
            var connectGrids = gridDisplays[gridCount].ConnectLines;
            var connectGridsDictionary = new Dictionary<string, List<Tuple<int, int>>>();
            foreach (var gridElement in connectGrids)
            {
                var connectColumn = Tuple.Create<int, int>(gridElement.OriginColumn, gridElement.TargetColumn);
                if (connectGridsDictionary.ContainsKey(gridElement.TargetGrid.name))
                {
                    connectGridsDictionary[gridElement.TargetGrid.name].Add(connectColumn);
                }
                else
                {
                    var connectColumns = new List<Tuple<int, int>>();
                    connectColumns.Add(connectColumn);
                    connectGridsDictionary.Add(gridElement.TargetGrid.name, connectColumns);
                }
            }
            foreach (var connectElement in connectGridsDictionary)
            {
                ConnectData connectData = new ConnectData();

                connectData.TargetGridName = connectElement.Key;
                connectData.ConnectColumns = new ConnectColumn[connectElement.Value.Count];

                for (var columnCount = 0; columnCount < connectElement.Value.Count; columnCount++)
                {
                    ConnectColumn connectColumn = new ConnectColumn();
                    connectColumn.originColumn = connectElement.Value[columnCount].Item1;
                    connectColumn.targetColumn = connectElement.Value[columnCount].Item2;
                    connectData.ConnectColumns[columnCount] = connectColumn;
                }

                gridData.TargetGrids.Add(connectData);
            }


            //add loss fertility columns
            var lossFertilityColumns = new int[gridDisplays[gridCount].LossFertilityColumns.Count];
            int countColumn = 0;
            foreach (var lossFertilityColumn in gridDisplays[gridCount].LossFertilityColumns)
            {
                lossFertilityColumns[countColumn] = lossFertilityColumn.Key;
                countColumn++;
            }
            gridData.LossFertilityColumns = lossFertilityColumns;

            //Add data of grid
            this.grids.Add(gridData);
        }
    }

    [System.Serializable]
    public struct ConnectColumn
    {
        public int originColumn;
        public int targetColumn;
    }


    [System.Serializable]
    public struct ConnectData
    {
        public string TargetGridName;
        public ConnectColumn[] ConnectColumns;
    }

    [System.Serializable]
    public struct GridData
    {
        public string Name;
        public int X;
        public int Y;
        public Vector3 Position;
        public Quaternion Rotation;
        public GridPlay.PiecePosition[] InitialPieces;
        public List<ConnectData> TargetGrids;
        public int[] LossFertilityColumns;
    }

    [System.Serializable]
    public struct StarScoreData
    {
        public LevelType LevelType;
        public int Star1Score;
        public int Star2Score;
        public int Star3Score;
    }
}

