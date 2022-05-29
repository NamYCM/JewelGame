using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    [System.Serializable]
    public struct ConnectData
    {
        public int X;
        public GridPlay OriginGrid;
    };

    public PiecePrefab[] piecePrefabs;

    protected Dictionary<PieceType, GameObject> _piecePrefabDict;

    protected GridPlay grid;

    private ConnectGridComponent connectGridComponent;
    PiecePool piecePool;

    private HashSet<int> lossFertilityColumns;
    public HashSet<int> LossFertilityColumns => lossFertilityColumns;

    //enshire pieces amount of column which was generated and did not finalize the fall down move yet
    private Dictionary<int, int> spawnedColumn = new Dictionary<int, int>();
    private Dictionary<int, GamePiece> highestSpawnedPieces = new Dictionary<int, GamePiece>();

    private Dictionary<int, ConnectData> connectedColumns = new Dictionary<int, ConnectData>();

    private void Awake() {
        piecePool = PiecePool.Instance;
        _piecePrefabDict = new Dictionary<PieceType, GameObject>();
        lossFertilityColumns = new HashSet<int>();

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!_piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                _piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }
    }

    public void Init()
    {
        connectGridComponent = GetComponent<ConnectGridComponent>();
        grid = GetComponent<GridPlay>();

        for (int x = 0; x < grid.XDim; x ++)
        {
            highestSpawnedPieces.Add(x, null);
        }
    }

    private void UpdateSpawnedPiece(int x, GamePiece piece)
    {
        if (highestSpawnedPieces[x] == null
        || grid.GetGridPosition(piece.transform.position).y <= grid.GetGridPosition(highestSpawnedPieces[x].transform.position).y)
        {
            highestSpawnedPieces[x] = piece;
        }
    }

    private void RemoveSpawnedPiece (int x, GamePiece piece)
    {
        if (highestSpawnedPieces[x] != null && highestSpawnedPieces[x] == piece)
            highestSpawnedPieces[x] = null;
    }

    private Vector3 GetSpawnWorldPosition (int x)
    {
        GamePiece highestPiece = highestSpawnedPieces[x];
        Vector3 position;
        Vector2Int gridPositionOfHighestPiece;
        int highestRow = grid.GetHighestRow(x);

        if (highestPiece == null)
        {
            position = grid.GetWorldPosition(x, highestRow - 1);
        }
        else
        {
            gridPositionOfHighestPiece = grid.GetGridPosition(highestPiece.transform.position);
            if (gridPositionOfHighestPiece.y >= highestRow)
                position = grid.GetWorldPosition(x, highestRow - 1);
            else
                position = grid.GetWorldPosition(gridPositionOfHighestPiece.x, gridPositionOfHighestPiece.y - 1);
        }

        return position;
    }

    public void SetLossFertilityColumnData (int[] lossFertilityColumnsData)
    {
        if (lossFertilityColumnsData == null)
            //it's mean does not have any loss fertility column
            return;

        foreach (var column in lossFertilityColumnsData)
        {
            try
            {
                lossFertilityColumns.Add(column);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("this column lossed fertility already/n" + ex.StackTrace);
            }
        }
    }

    public void SetConnectColumn (GridPlay originGrid, int xOrigin, int xTarget)
    {
        try
        {
            connectedColumns.Add(xTarget, new ConnectData() {
                OriginGrid = originGrid,
                X = xOrigin
            });
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("this column is already connected");
            throw ex;
        }
    }

    public bool IsLossFertilityColumn (int x)
    {
        return LossFertilityColumns != null && LossFertilityColumns.Contains(x);
    }

    public bool IsGetPieceFromAnotherGrid (int x, out GridPlay originGrid, out int originX)
    {
        originGrid = null;
        originX = -1;

        if (connectedColumns.ContainsKey(x))
        {
            originGrid = connectedColumns[x].OriginGrid;
            originX = connectedColumns[x].X;
            return true;
        }

        return false;
    }
    public bool IsGetPieceFromAnotherGrid (int x) => connectedColumns.ContainsKey(x);

    public GamePiece NewSpawnNewPieceAtColumn(int x, int y, PieceType type, ColorType color = ColorType.RANDOM)
    {
        // check in spawned column
        if (spawnedColumn.ContainsKey(x))
        {
            spawnedColumn[x]++;
        }
        else
        {
            spawnedColumn.Add(x, 1);
        }
        //spaw new piece
        GamePiece newPiece;

        newPiece = piecePool.GetPiece(type, GetSpawnWorldPosition(x), grid.transform.rotation);
        UpdateSpawnedPiece(x, newPiece);

        //init piece
        newPiece.Init(x, grid.GetGridPosition(newPiece.transform.position).y, grid, type);

        if (newPiece.IsColored())
        {
            if (color == ColorType.RANDOM)
                newPiece.ColorComponent.SetColor((ColorType)Random.Range(0, newPiece.ColorComponent.NumColors - 2));
            else
                newPiece.ColorComponent.SetColor(color);
        }
        //fall down and at the end of fall down, decrete amount of corresponding spawned column
        if (newPiece.IsMovable())
        {
            newPiece.MovableComponent.MoveAndFallDown(x, y, () => {
                RemoveSpawnedPiece(x, newPiece);
            });
        }

        return newPiece;
    }

    public GamePiece SpawnNewPieceAtPosition(int x, int y, PieceType type)
    {
        GamePiece newPiece = piecePool.GetPiece(type, grid.GetWorldPosition(x, y), grid.transform.rotation);

        newPiece.Init(x, y, grid, type);

        if (newPiece.IsColored())
        {
            if (newPiece.Type != PieceType.RAINBOW)
                newPiece.ColorComponent.SetColor((ColorType)Random.Range(0, newPiece.ColorComponent.NumColors - 2));
            else
                newPiece.ColorComponent.SetColor(ColorType.ANY);
        }

        return newPiece;
    }

    public GamePiece SpawnNewPieceAtPosition(int x, int y, PieceType type, ColorType color)
    {
        GamePiece newPiece = piecePool.GetPiece(type, grid.GetWorldPosition(x, y), grid.transform.rotation);

        newPiece.Init(x, y, grid, type);

        if (type != PieceType.RAINBOW && newPiece.IsColored())
            newPiece.ColorComponent.SetColor(color);

        return newPiece;
    }

    public void DestroyPiece (GamePiece gamePiece)
    {
        if (gamePiece.IsClearable() && gamePiece.ClearableComponent.IsBeingCleared)
        {
            gamePiece.ClearableComponent.ReturnPool();
            return;
        }
        piecePool.ReturnToPool(gamePiece);
    }
}