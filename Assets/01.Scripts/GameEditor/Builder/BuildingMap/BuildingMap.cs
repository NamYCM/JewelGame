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
        starScore.star1Score = levelSetting.Star1Score;
        starScore.star2Score = levelSetting.Star2Score;
        starScore.star3Score = levelSetting.Star3Score;
        starScore.levelType = levelSetting.Type;
    }

    public void AddGrids (List<GridDisplay> gridDisplays)
    {
        for (int gridCount = 0; gridCount < gridDisplays.Count; gridCount ++)
        {
            GridData gridData = new GridData();
            gridData.x = gridDisplays[gridCount].XDim;
            gridData.y = gridDisplays[gridCount].YDim;
            gridData.position = gridDisplays[gridCount].transform.position;
            gridData.rotation = gridDisplays[gridCount].transform.rotation;
            gridData.initialPieces = new GridPlay.PiecePosition[gridDisplays[gridCount].InitialPieces.Count];
            for (int pieceCount = 0; pieceCount < gridDisplays[gridCount].InitialPieces.Count; pieceCount ++)
            {
                GridPlay.PiecePosition pieceData = new GridPlay.PiecePosition();
                pieceData.x = gridDisplays[gridCount].InitialPieces[pieceCount].x;
                pieceData.y = gridDisplays[gridCount].InitialPieces[pieceCount].y;
                pieceData.type = gridDisplays[gridCount].InitialPieces[pieceCount].Piece.Type;

                gridData.initialPieces[pieceCount] = pieceData;
            }

            gridData.name = gridDisplays[gridCount].name;

            gridData.targetGrids = new List<ConnectData>();
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

                connectData.targetGridName = connectElement.Key;
                connectData.connectColumns = new ConnectColumn[connectElement.Value.Count];

                for (var columnCount = 0; columnCount < connectElement.Value.Count; columnCount++)
                {
                    ConnectColumn connectColumn = new ConnectColumn();
                    connectColumn.originColumn = connectElement.Value[columnCount].Item1;
                    connectColumn.targetColumn = connectElement.Value[columnCount].Item2;
                    connectData.connectColumns[columnCount] = connectColumn;
                }

                gridData.targetGrids.Add(connectData);
            }


            //add loss fertility columns
            var lossFertilityColumns = new int[gridDisplays[gridCount].LossFertilityColumns.Count];
            int countColumn = 0;
            foreach (var lossFertilityColumn in gridDisplays[gridCount].LossFertilityColumns)
            {
                lossFertilityColumns[countColumn] = lossFertilityColumn.Key;
                countColumn++;
            }
            gridData.lossFertilityColumns = lossFertilityColumns;

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
        public string targetGridName;
        public ConnectColumn[] connectColumns;
    }

    [System.Serializable]
    public struct GridData
    {
        public string name;
        public int x;
        public int y;
        public Vector3 position;
        public Quaternion rotation;
        public GridPlay.PiecePosition[] initialPieces;
        public List<ConnectData> targetGrids;
        public int[] lossFertilityColumns;
    }

    [System.Serializable]
    public struct StarScoreData
    {
        public LevelType levelType;
        public int star1Score;
        public int star2Score;
        public int star3Score;
    }
}

