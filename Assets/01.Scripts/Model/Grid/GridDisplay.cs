using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridDisplay : Grid
{
    [System.Serializable]
    public struct BuildPiece
    {
        public int x, y;
        public BuildingPiece Piece;      
        public GameObject Instance;
    }

    [System.Serializable]
    public struct ConnectLine
    {
        public GridDisplay TargetGrid;
        public int OriginColumn;
        public int TargetColumn;
        public LineRenderer Instance;
    }

    public GameObject xPrefab;

    // int XDim = 0;
    // int YDim = 0;
    // int angle = 0;
    GameObject[,] grids;
    GameObject arrow;

    [SerializeField] List<BuildPiece> initialPieces = new List<BuildPiece>();
    [SerializeField] List<ConnectLine> connectLines = new List<ConnectLine>();
    Dictionary<int, GridDisplay> connectedColumns = new Dictionary<int, GridDisplay>();
    Dictionary<int, GameObject> lossFertilityColumns = new Dictionary<int, GameObject>();

    public List<BuildPiece> InitialPieces => initialPieces;
    public List<ConnectLine> ConnectLines => connectLines;
    public Dictionary<int, GameObject> LossFertilityColumns => lossFertilityColumns;

    public GameObject emptySquare;

    public GameObject downArrowPrefab;

    public GameObject linePrefab;

    private MoveableControl moveableControl;
    public MoveableControl MoveableControl => moveableControl;

    private BuildingControl buildingControl;
    public BuildingControl BuildingControl => buildingControl;

    private ConnectControl connectControl;
    public ConnectControl ConnectControl => connectControl;

    private LossFertilityControl lossFertilityControl;
    public LossFertilityControl LossFertilityControl => lossFertilityControl;

    public override int XDim
    {
        get { return xDim; }
        set 
        {
            if (value < 0) return;
            xDim = value; 
            ReDrawGrid();
        }
    }
    public override int YDim
    {
        get { return yDim; }
        set 
        { 
            if (value < 0) return;
        
            yDim = value; 
            ReDrawGrid();
        }
    }
    public override int Angle
    {
        get { return (int)transform.eulerAngles.z; }
        set 
        { 
            // angle = value; 
            transform.eulerAngles = new Vector3(0, 0, value);
            ReDrawGrid();
        }
    }

    private void Awake() {
        moveableControl = GetComponent<MoveableControl>();
        buildingControl = GetComponent<BuildingControl>();
        connectControl = GetComponent<ConnectControl>();
        lossFertilityControl = GetComponent<LossFertilityControl>();
    }   

    private void OnDestroy() {
        var browsedGrid = new HashSet<GridDisplay>();

        foreach (var connect in connectedColumns)
        {
            if (!browsedGrid.Contains(connect.Value))
            {
                //disconnect origin grid connect to this 
                connect.Value.DisconnectToGrid(this);
            }

            browsedGrid.Add(connect.Value);
        }
        
        //disconnect all target girds which are connected
        DisconnectAll();
    }

    private void DisconnectToGrid (GridDisplay targetGrid)
    {
        for (var lineCount = 0; lineCount < connectLines.Count; lineCount++)
        {
            if (connectLines[lineCount].TargetGrid == targetGrid)
            {
                Destroy(connectLines[lineCount].Instance);
                connectLines.RemoveAt(lineCount);
                lineCount--;
            }
        }
    }

    private void DisconnectAll()
    {
        for (var lineCount = 0; lineCount < connectLines.Count; lineCount++)
        {
            connectLines[lineCount].TargetGrid.RemoveConnectedColumn(connectLines[lineCount].TargetColumn);
            Destroy(connectLines[lineCount].Instance);
            connectLines.RemoveAt(lineCount);
            lineCount--;
        }
    }

    private void AddConnectedColumn(int targetColumn, GridDisplay originGrid)
    {
        connectedColumns.Add(targetColumn, originGrid);
    }

    private void RemoveConnectedColumn (int targetColumn)
    {
        connectedColumns.Remove(targetColumn);
    }

    private LineRenderer DrawConnectLine (GridDisplay targetGrid, int originColumn, int targetColumn)
    {
        LineRenderer lineRenderer = Instantiate(linePrefab, transform.position, transform.rotation, transform).GetComponent<LineRenderer>();
        // var targetGridPosition = targetGrid.GetBottomColumnPosition(targetColumn);
        // var originGridPosition = GetUpperColumnPosition(originColumn);
        var targetGridPosition = targetGrid.GetUpperColumnPosition(targetColumn);
        var originGridPosition = GetBottomColumnPosition(originColumn);
        lineRenderer.SetPosition(0, targetGrid.GetWorldPosition(targetGridPosition.x, targetGridPosition.y));
        lineRenderer.SetPosition(1, this.GetWorldPosition(originGridPosition.x, originGridPosition.y));

        return lineRenderer;
    }
    private LineRenderer DrawConnectLine (ref LineRenderer line, GridDisplay targetGrid, int originColumn, int targetColumn)
    {
        // var targetGridPosition = targetGrid.GetBottomColumnPosition(targetColumn);
        // var originGridPosition = GetUpperColumnPosition(originColumn);
        var targetGridPosition = targetGrid.GetUpperColumnPosition(targetColumn);
        var originGridPosition = GetBottomColumnPosition(originColumn);
        line.SetPosition(0, targetGrid.GetWorldPosition(targetGridPosition.x, targetGridPosition.y));
        line.SetPosition(1, this.GetWorldPosition(originGridPosition.x, originGridPosition.y));

        return line;
    }

    public void SetValue (int column, int row)
    {
        this.XDim = column;
        this.YDim = row;
    }

    public void DrawGrid ()
    {
        Quaternion quaternion = transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, Angle);
        grids = new GameObject[XDim, YDim];
        arrow = Instantiate(downArrowPrefab, this.GetWorldPosition(-1, 0), quaternion, transform);
        for (int y = 0; y < YDim; y ++)
        {
            for (int x = 0; x < XDim; x ++)
            {
                grids[x, y] = Instantiate(emptySquare, this.GetWorldPosition(x, y), quaternion, transform);
                grids[x, y].name = "Board " + x.ToString() + "-" + y.ToString();
                grids[x, y].transform.parent = transform;
            }
        }

        for (int count = 0; count < initialPieces.Count; count ++)
        {
            var tempPiece = initialPieces[count];
            
            tempPiece.Instance = Instantiate(tempPiece.Piece.Prefab, this.GetWorldPosition(tempPiece.x, tempPiece.y), transform.rotation, transform);
            initialPieces[count] = tempPiece;
        }
    }

    public void ReDrawGrid ()
    {
        foreach (var grid in grids)
        {
            Destroy(grid);
        }
        Destroy(arrow);
        foreach (var piece in initialPieces)
        {
            Destroy(piece.Instance);
        }

        DrawGrid();
    }

    public void SetName (int index)
    {
        name = "Grid " + index;
    }

    public bool IsMoveableControl ()
    {
        return moveableControl != null;
    }

    public bool IsBuildingControl ()
    {
        return buildingControl != null;
    }

    public bool IsConnectControl ()
    {
        return connectControl != null;
    }

    public bool IsLossFertilityControl ()
    {
        return lossFertilityControl != null;
    }

    public bool OutOfGrid (int column, int row)
    {
        return column >= this.XDim || column < 0 || row >= this.YDim || row < 0;
    }

    public void InitPiece (BuildingPiece piece, int column, int row)
    {
        //Destroy if there is already a piece at this grid position
        for (int i = 0; i < initialPieces.Count; i++)
        {
            if (initialPieces[i].x == column && initialPieces[i].y == row)
            {
                Destroy(initialPieces[i].Instance);
                var tempPiece = initialPieces[i];
            
                tempPiece.Piece = piece;
                tempPiece.Instance = Instantiate(piece.Prefab, this.GetWorldPosition(tempPiece.x, tempPiece.y), transform.rotation, transform);
                initialPieces[i] = tempPiece;

                return;
            }
        }

        //Enshrine and generate piece at this grid position
        BuildPiece newPiece = new BuildPiece();
        newPiece.x = column;
        newPiece.y = row;
        newPiece.Piece = piece;
        newPiece.Instance = Instantiate(piece.Prefab, this.GetWorldPosition(column, row), transform.rotation, transform);
        initialPieces.Add(newPiece);
    }

    public BuildingPiece RemovePiece (int column, int row)
    {
        for (int i = 0; i < initialPieces.Count; i++)
        {
            if (initialPieces[i].x == column && initialPieces[i].y == row)
            {
                BuildingPiece piece = initialPieces[i].Piece;
                Destroy(initialPieces[i].Instance);
                initialPieces.RemoveAt(i);
                return piece;
            }
        }

        return null;
    }

    public bool HasInitPeace (int column, int row)
    {
        for (int i = 0; i < initialPieces.Count; i++)
        {
            if (initialPieces[i].x == column && initialPieces[i].y == row)
            {
                return true;
            }
        }

        return false;
    }

    public Vector2Int GetBottomColumnPosition (int column)
    {
        return new Vector2Int(column, YDim - 1);
    }

    public Vector2Int GetUpperColumnPosition (int column)
    {
        return new Vector2Int(column, 0);
    }

    public Vector2Int GetGridPositionOntoGrid (int column)
    {
        return new Vector2Int(column, -1);
    }

    public bool BeOriginOfAnotherGrid (int column)
    {
        foreach (var lineElement in connectLines)
        {
            if (lineElement.OriginColumn == column) return true;
        }

        return false;
    }

    /// <summary>Disconnect to target grid at origin position</summary>
    /// <returns>The connect grid which is disconnected</returns>
    public GridDisplayCreator.ConnectGrid DisconnectGridAtColumn (int column)
    {
        var targetGrid = new GridDisplayCreator.ConnectGrid();

        for (var lineCount = 0; lineCount < connectLines.Count; lineCount++)
        {
            if (connectLines[lineCount].OriginColumn == column)
            {
                targetGrid.grid = connectLines[lineCount].TargetGrid;
                targetGrid.column = connectLines[lineCount].TargetColumn;

                connectLines[lineCount].TargetGrid.RemoveConnectedColumn(connectLines[lineCount].TargetColumn);
                Destroy(connectLines[lineCount].Instance);
                connectLines.RemoveAt(lineCount);
                
                return targetGrid;
            }
        }

        return targetGrid;    
    }

    /// <summary>Connect to target grid where origin column is column of this grid and target column is column of target grid</summary>
    public void ConnectGrid (GridDisplay targetGrid, int originColumn, int targetColumn)
    {
        ConnectLine connectLine = new ConnectLine();
        connectLine.TargetGrid = targetGrid;
        connectLine.OriginColumn = originColumn;
        connectLine.TargetColumn = targetColumn;

        //Draw line from target to origin
        connectLine.Instance = DrawConnectLine(connectLine.TargetGrid, connectLine.OriginColumn, connectLine.TargetColumn);

        if (connectLines.Count == 0) connectLines.Add(connectLine);
        else
        {
            for (var lineCount = 0; lineCount < connectLines.Count; lineCount++)
            {
                if (connectLines[lineCount].OriginColumn == connectLine.OriginColumn)
                {
                    connectLines[lineCount].TargetGrid.RemoveConnectedColumn(connectLines[lineCount].TargetColumn);
                    Destroy(connectLines[lineCount].Instance);
                    connectLines[lineCount] = connectLine;
                    break;
                }
                else if (lineCount == connectLines.Count - 1)
                {
                    connectLines.Add (connectLine);
                    break;
                }
            }
        }

        connectLine.TargetGrid.AddConnectedColumn(connectLine.TargetColumn, this);
    }

    /// <summary>Redraw all instances of connected line</summary>
    public void RedrawConnectLine ()
    {
        connectLines = connectLines.Select(connect => 
        { 
            DrawConnectLine(ref connect.Instance, connect.TargetGrid, connect.OriginColumn, connect.TargetColumn); 
            return connect; 
        }).ToList();

        foreach (var connect in connectedColumns)
        {
            connect.Value.RedrawConnectLine();
        }
    }

    public bool IsTargetOfAnotherGrid (int targetColumn)
    {
        return connectedColumns.ContainsKey(targetColumn);
    }

    public void InitLossFertilityColumn (int column)
    {
        if (!lossFertilityColumns.ContainsKey(column))
        {
            var gridPositionOntoGrid = GetGridPositionOntoGrid(column);
            lossFertilityColumns.Add(column, Instantiate(xPrefab, this.GetWorldPosition(gridPositionOntoGrid.x, gridPositionOntoGrid.y), transform.rotation, transform));
        }
    }

    public void RemoveLossFertilityColumn (int column)
    {
        if (lossFertilityColumns.ContainsKey(column))
        {
            Destroy(lossFertilityColumns[column]);
            lossFertilityColumns.Remove(column);
        }
    }

    public bool IsLossFertilityAt (int column)
    {
        return lossFertilityColumns.ContainsKey(column);
    }
}
