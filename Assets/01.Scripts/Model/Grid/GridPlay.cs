using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

[RequireComponent(typeof(Spawner))]
public class GridPlay : Grid
{
    [System.Serializable]
    public struct PiecePrefab
    {
        public PieceType type;
        public GameObject prefab;
    };

    [System.Serializable]
    public struct PiecePosition
    {
        public PieceType type;
        public int x;
        public int y;
    };

    public struct ColumnInfor
    {
        public int highest;
        public int lowest;
    }

    bool isClearedOnThisFrame = false;
    bool isFilledInThisFrame = false;
    int movingPiece = 0;
    int clearingPiece = 0;

    public bool IsDebug = false;

    public Level level;

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;


    public PiecePosition[] initialPieces;

    private Dictionary<int, ColumnInfor> columns;
    public Dictionary<int, ColumnInfor> Columns => columns;

    Spawner spawner;
    public Spawner Spawner => spawner;

    private Dictionary<PieceType, GameObject> _piecePrefabDict;

    private GamePiece[,] _pieces;
    public GamePiece[,] Pieces => _pieces;

    private bool _inverse = false;

    private GamePiece _pressedPiece;
    private GamePiece _enteredPiece;

    public bool _isFilling;

    public bool IsFilling => _isFilling;

    public override int XDim
    {
        get { return xDim; }
        set
        {
            if (value < 0) return;
            xDim = value;
        }
    }
    public override int YDim
    {
        get { return yDim; }
        set
        {
            if (value < 0) return;
            yDim = value;
        }
    }
    public override int Angle
    {
        get { return (int)transform.eulerAngles.z; }
        set
        {
            transform.eulerAngles = new Vector3(0, 0, value);
        }
    }

    private ConnectGridComponent connectGridComponent;

    MaskGrid maskGrid;
    GameManager gameManager;

    private void Update() {
        if (IsDebug)
        {
            string debug = name + "\n";
            for (int y = 0; y < YDim; y++)
            {
                for (int x = 0; x < XDim; x++)
                {
                    debug += (_pieces[x, y].IsColored() ? _pieces[x, y].ColorComponent.Color.ToString() : _pieces[x, y].Type.ToString()) +" | ";
                }
                debug += "\n";
            }
            Debug.Log(debug);
        }
    }

    private void Awake() {
        spawner = GetComponent<Spawner>();
        maskGrid = GetComponentInChildren<MaskGrid>();
    }

    #region Init
    public void Init (int column, int row, PiecePosition[] initialPieces)
    {
        XDim = column;
        YDim = row;
        this.initialPieces = initialPieces;

        //this is not in awake because in the awake time does not have this component
        connectGridComponent = GetComponent<ConnectGridComponent>();

        gameManager = GameManager.Instance;
        level = gameManager.Level;

        spawner.Init();

        InitPieces();
        InitColumnInfor();

        maskGrid.Init(this, transform.rotation);
    }

    private void InitColumnInfor ()
    {
        columns = new Dictionary<int, ColumnInfor>();

        int countY, yHighest, yLowest;
        for (int x = 0; x < xDim; x ++)
        {
            // GetHighestRow
            countY = 0;
            while (_pieces[x, countY].Type == PieceType.NULL && countY < YDim)
            {
                countY++;
            }
            yHighest = countY;

            // GetLowestRow
            countY = YDim - 1;
            while (_pieces[x, countY].Type == PieceType.NULL && countY >= 0)
            {
                countY--;
            }
            yLowest = countY;

            columns.Add(x, new ColumnInfor() {
                highest = yHighest,
                lowest = yLowest
            });
        }
    }

    private void InitPieces()
    {
        // populating dictionary with piece prefabs types
        _piecePrefabDict = new Dictionary<PieceType, GameObject>();
        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            if (!_piecePrefabDict.ContainsKey(piecePrefabs[i].type))
            {
                _piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        // instantiate backgrounds
        DrawBackground();

        // instantiating pieces
        _pieces = new GamePiece[XDim, YDim];

        for (int i = 0; i < initialPieces.Length; i++)
        {
            if (initialPieces[i].x >= 0 && initialPieces[i].y < XDim
                && initialPieces[i].y >=0 && initialPieces[i].y <YDim )
            {
                SpawnNewPiece(initialPieces[i].x, initialPieces[i].y, initialPieces[i].type);
            }
        }

        for (int x = 0; x < XDim; x++)
        {
            for (int y = 0; y < YDim; y++)
            {
                if (_pieces[x, y] == null)
                {
                    SpawnNewPiece(x, y, PieceType.NORMAL);
                }
            }
        }
    }

    private void DrawBackground ()
    {
        for (int x = 0; x < XDim; x++)
        {
            for (int y = 0; y < YDim; y++)
            {
                bool isContinue = false;
                for (int i = 0; i < initialPieces.Length; i++)
                {
                    if (initialPieces[i].x == x && initialPieces[i].y == y && initialPieces[i].type == PieceType.NULL)
                    {
                        isContinue = true;
                        break;
                    }
                }
                if (isContinue) continue;
                GameObject background = Instantiate(backgroundPrefab, this.GetWorldPosition(x, y), transform.rotation);
                background.transform.parent = transform;
                background.isStatic = true;
            }
        }
    }

    #endregion

    private void TurnOffFillMode ()
    {
        _isFilling = false;
        gameManager.ChangeGameState(GameManager.GameState.WaitInput);
    }

    private void TurnOnFillMode ()
    {
        _isFilling = true;
        gameManager.ChangeGameState(GameManager.GameState.Filling);
    }

    private bool IsConnected (int x) { return connectGridComponent != null && connectGridComponent.IsConnect(x); }

    private GamePiece SpawnNewPieceAtColumn(int x, int y, PieceType type, ColorType color = ColorType.RANDOM)
    {
        _pieces[x, y] = spawner.NewSpawnNewPieceAtColumn(x, y, type, color);
        return _pieces[x, y];
    }

    private GamePiece SpawnNewPiece(int x, int y, PieceType type)
    {
        _pieces[x, y] = spawner.SpawnNewPieceAtPosition(x, y, type);
        return _pieces[x, y];
    }
    private GamePiece SpawnNewPiece(int x, int y, PieceType type, ColorType color)
    {
        _pieces[x, y] = spawner.SpawnNewPieceAtPosition(x, y, type, color);
        return _pieces[x, y];
    }

    private IEnumerator SpawnNewPieceCoroutine(int x, int y, PieceType type, ColorType color)
    {
        yield return new WaitForSeconds(GameStatus.CLEAR_TIME);
        spawner.DestroyPiece(_pieces[x, y]);
        var newPiece = SpawnNewPiece(x, y, type);
        if(newPiece.IsColored())
            newPiece.ColorComponent.SetColor(color);

        StartFillGrid();
    }

    public GamePiece SpawnEmptyPiece(int x, int y)
    {
        _pieces[x, y] = spawner.SpawnNewPieceAtPosition(x, y, PieceType.EMPTY);
        return _pieces[x, y];
    }

    private bool IsAdjacent(GamePiece piece1, GamePiece piece2)
    {
        return (piece1.X==piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1)
            || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
    }

    private void SwapPieces(GamePiece piece1, GamePiece piece2)
    {
        if (!piece1.IsMovable() || !piece2.IsMovable()) return;

        _pieces[piece1.X, piece1.Y] = piece2;
        _pieces[piece2.X, piece2.Y] = piece1;
        if (this.GetMatch(piece1, piece2.X, piece2.Y) != null || this.GetMatch(piece2, piece1.X, piece1.Y) != null
            || piece1.Type == PieceType.RAINBOW || piece2.Type == PieceType.RAINBOW)
        {
            TurnOnFillMode();

            int piece1X = piece1.X;
            int piece1Y = piece1.Y;

            //make peace 1 appear on peace 2 when swap
            piece1.SpritePieceComponent.SetSortingOrder(10);

            bool hasColorCleared = false;

            piece1.MovableComponent.Swap(piece2.X, piece2.Y, () => {
                if (piece1.Type == PieceType.RAINBOW && piece1.IsClearable() && piece2.IsColored())
                {
                    ClearColorPiece clearColor = piece1.GetComponent<ClearColorPiece>();

                    if (clearColor)
                    {
                        clearColor.Color = piece2.ColorComponent.Color;
                    }

                    ClearPiece(piece1.X, piece1.Y);
                    hasColorCleared = true;
                }
            });
            piece2.MovableComponent.Swap(piece1X, piece1Y, () => {
                if (piece2.Type == PieceType.RAINBOW && piece2.IsClearable() && piece1.IsColored())
                {
                    ClearColorPiece clearColor = piece2.GetComponent<ClearColorPiece>();

                    if (clearColor)
                    {
                        clearColor.Color = piece1.ColorComponent.Color;
                    }

                    ClearPiece(piece2.X, piece2.Y);
                }//if has color cleared, do not clear all valid matches
                else if (!hasColorCleared)
                {
                    StartCoroutine(ClearAllValidMatchesCoroutine(() => {
                        _pressedPiece = null;
                        _enteredPiece = null;
                    }));
                }

                // TODO consider doing this using delegates
                level.OnMove();
            });
        }
        else
        {
            _pieces[piece1.X, piece1.Y] = piece1;
            _pieces[piece2.X, piece2.Y] = piece2;
            int piece1X = piece1.X;
            int piece1Y = piece1.Y;

            piece1.MovableComponent.Swap(piece2.X, piece2.Y);
            piece2.MovableComponent.Swap(piece1X, piece1Y, () => {
                int tempPiece1X = piece1.X;
                int tempPiece1Y = piece1.Y;

                piece1.MovableComponent.Swap(piece2.X, piece2.Y);
                piece2.MovableComponent.Swap(tempPiece1X, tempPiece1Y);
            });

            _pressedPiece = null;
            _enteredPiece = null;
        }
    }

    public void PressPiece(GamePiece piece)
    {
        _pressedPiece = piece;
    }

    public void EnterPiece(GamePiece piece)
    {
        _enteredPiece = piece;
    }

    public void ReleasePiece()
    {
        if (_pressedPiece == null || _enteredPiece == null)
        {
            _pressedPiece = _enteredPiece = null;
            return;
        }

        if (IsAdjacent (_pressedPiece, _enteredPiece))
        {
            SwapPieces(_pressedPiece, _enteredPiece);
        }
    }

    public IEnumerator ClearAllValidMatchesCoroutine(TweenCallback onEnd = null)
    {
        //ensure just check valid match only if does not checked yet in this frame
        isClearedOnThisFrame = false;

        yield return new WaitForEndOfFrame();

        if (isClearedOnThisFrame)
        {
            yield break;
        }

        isClearedOnThisFrame = true;

        bool hasClear = false;
        for (int y = 0; y < YDim; y++)
        {
            for (int x = 0; x < XDim; x++)
            {
                if(ClearAllValidMatchesAt(x, y))
                {
                    hasClear = true;
                }
            }
        }

        //if don't have any clear, refill one time to make sure don't miss any dia down
        if (!hasClear)
        {
            StartFillGrid();
        }
        else onEnd?.Invoke();
    }

    public bool ClearAllValidMatchesAt(int x, int y)
    {
        if (!_pieces[x, y].IsClearable() || (_pieces[x, y].IsMovable() && _pieces[x, y].MovableComponent.IsRunning()))
        {
            return false;
        }

        List<GamePiece> match = this.GetMatch(_pieces[x, y], x, y, out List<GamePiece> horizontal, out List<GamePiece> vertical);
        if (match == null || match.Count == 0)
        {
            return false;
        }

        PieceType specialPieceType = PieceType.COUNT;
        GamePiece randomPiece = match[Random.Range(0, match.Count)];

        //check clear on match is cleared
        foreach (var piece in match)
        {
            if (!piece.gameObject.activeSelf || piece.IsClearable() && piece.ClearableComponent.IsBeingCleared)
            {
                return false;
            }
        }

        int specialPieceX = randomPiece.X;
        int specialPieceY = randomPiece.Y;
        // Spawning special pieces
        if (match.Count == 4)
        {
            if (_pressedPiece == null || _enteredPiece == null)
            {
                specialPieceType = (PieceType) Random.Range((int) PieceType.ROW_CLEAR, (int) PieceType.COLUMN_CLEAR);
            }
            else if (_pressedPiece.Y == _enteredPiece.Y)
            {
                specialPieceType = PieceType.ROW_CLEAR;
            }
            else
            {
                specialPieceType = PieceType.COLUMN_CLEAR;
            }
        } // Spawning a rainbow piece
        else if (match.Count >= 5)
        {
            if ((horizontal.Count == 2 && vertical.Count == 3)
            || (horizontal.Count == 3 && vertical.Count == 2))
                specialPieceType = PieceType.BOMB;
            else
            {
                specialPieceType = PieceType.RAINBOW;
            }
        }

        //try to clear all pieces at the same time, but it's seem not work
        foreach (GamePiece countPiece in match)
        {
            if (!this.CanClear(countPiece.X, countPiece.Y)) return false;
        }

        for (int i = 0; i < match.Count; i++)
        {
            // if (!ClearPiece(match[i].X, match[i].Y)) return true;
            if (!ClearPiece(match[i].X, match[i].Y)) {
                continue;
            }

            // needsRefill = true;

            if (match[i] != _pressedPiece && match[i] != _enteredPiece) continue;

            specialPieceX = match[i].X;
            specialPieceY = match[i].Y;
        }

        // Setting their colors
        if (specialPieceType == PieceType.COUNT)
        {
            return true;
        }

        // spawner.DestroyPiece(_pieces[specialPieceX, specialPieceY]);

        ColorType newColor = ColorType.COUNT;

        if ((specialPieceType == PieceType.ROW_CLEAR || specialPieceType == PieceType.COLUMN_CLEAR || specialPieceType == PieceType.BOMB)
            && match[0].IsColored())
        {
            newColor = match[0].ColorComponent.Color;
        }
        else if (specialPieceType == PieceType.RAINBOW)
        {
            newColor = ColorType.ANY;
        }

        if (newColor != ColorType.COUNT)
        {
            //ignore this position until special piece was spawned
            _pieces[specialPieceX, specialPieceY].ChangeToIgnore();
            // SpawnNewPiece(specialPieceX, specialPieceY, PieceType.IGNORE);
            StartCoroutine(SpawnNewPieceCoroutine(specialPieceX, specialPieceY, specialPieceType, newColor));
        }
        else
        {
            spawner.DestroyPiece(_pieces[specialPieceX, specialPieceY]);
        }

        return true;
    }

    private IEnumerator FillGridCoroutine ()
    {
        //ensure just fill only if does not fill yet in this frame
        isFilledInThisFrame = false;

        yield return new WaitForEndOfFrame();

        if (isFilledInThisFrame)
        {
            yield break;
        }

        if (!IsFilling) TurnOnFillMode();

        isFilledInThisFrame = true;

        if (!FillAllColumns() && movingPiece == 0 && clearingPiece == 0)
        {
            TurnOffFillMode();
        }
    }

    public void StartFillGrid ()
    {
        StartCoroutine(FillGridCoroutine());
    }

    private bool IsBottomEmpty (int x)
    {
        return (_pieces[x, this.GetHighestRow(x)].Type == PieceType.EMPTY);
    }

    private bool FallDownLowestPiece (int x)
    {
        bool hasMove = false;

        if (!connectGridComponent.IsConnect(x)) return hasMove;

        var target = connectGridComponent.Targets[x];

        if (!target.grid.IsBottomEmpty(target.x)) return hasMove;

        int yLowest = this.GetLowestRow(x);

        GamePiece piece = _pieces[x, yLowest];

        if (!piece.IsIgnored()
        && piece.IsMovable())// && !_pieces[x, yLowest].MovableComponent.IsRunning())
        {
            //because this action does not implement immediately
            //we need to ensure do not move this piece when it's waiting for fall down to another grid
            piece.ChangeToIgnore();

            piece.MovableComponent.FallDown(x, yLowest + 1, () => {
                piece.ResetIgnore();

                if (!target.grid.CanMoveDirectDown(target.x, target.grid.GetHighestRow(target.x) - 1, out int desY))
                {
                    return;
                }

                //generate piece at the bottom of connected grid
                SpawnNewPiece(x, yLowest, PieceType.EMPTY);
                target.grid.spawner.DestroyPiece(target.grid.Pieces[target.x, desY]);
                target.grid.SpawnNewPieceAtColumn(target.x, desY, piece.Type, piece.IsColored() ? piece.ColorComponent.Color : ColorType.RANDOM);
            }, () => {
                spawner.DestroyPiece(piece);
                //because after destroy piece, clear all valid piece coroutine will be not implement
                StartCoroutine(FillGridCoroutine());
            });

            //it's can be cleared when falling down, so we need to rollback grid position to avoid array out of range
            piece.MovableComponent.RollBackGridPosition();
        }

        return hasMove;
    }

    /// <returns>if have any piece is moved, return true</returns>
    private bool FillAllColumns ()
    {
        bool hasMove = false;

        int x;
        // for (x = 0; x < xDim; x++)
        for (int loopX = 0; loopX < xDim; loopX++)
        {
            x = loopX;

            if (_inverse) { x = XDim - 1 - loopX; }

            if (FillColumn(x)) hasMove = true;
        }

        //inverse the loopX to make the fill the column which has fixed piece, more even
        if (hasMove) _inverse = !_inverse;

        return hasMove;
    }

    private IEnumerator FallDiagnalDown (int x, int y)
    {
        //when the piece waited too long, it'll fall dianal down
        //this equal to distance of 1 cell
        yield return new WaitForSeconds(1.0f / GameStatus.SPEED_OF_PIECE);

        if (this.CanMoveDiagonalDown(x, y, out int diagX, out int diagY, _inverse))
        {
            var piece = _pieces[x, y];

            //to make sure the piece is end of fall down, because the under pieces can be clear before this piece move have to fall diagonal down
            if (!piece.IsMovable() || piece.MovableComponent.IsRunning()
            || (piece.IsClearable() && piece.ClearableComponent.IsBeingCleared))
            {
                //TODO maybe have a case of this is the last fill grid time, and the mode is filling forever
                // StartCoroutine(FillGridCoroutine());
                yield break;
            }

            GamePiece diagonalPiece = _pieces[diagX, diagY];

            spawner.DestroyPiece(diagonalPiece);
            piece.MovableComponent.FallDown(diagX, diagY);
            _pieces[diagX, diagY] = piece;
            // _pieces[diagX, diagY] = SpawnNewPiece(diagX, diagY, PieceType.IGNORE);
            SpawnNewPiece(x, y, PieceType.EMPTY);
        }
    }

    /// <returns>if have any piece is moved, return true</returns>
    private bool FillColumn (int x)
    {
        bool hasMove = false;

        int highestRow = this.GetHighestRow(x);
        int lowestRow = this.GetLowestRow(x);
        GamePiece piece;

        //fall down and diagnal down
        for (int y = lowestRow - 1; y >= highestRow; y --)
        {
            piece = _pieces[x, y];
            if (!piece.IsMovable() || piece.IsIgnored()) continue;

            int offset = this.CountEmptyPieceCanBeFallDown(x, y + 1);
            if (offset > 0)
            {
                hasMove = true;

                int newY = y + offset;
                spawner.DestroyPiece(_pieces[x, newY]);
                piece.MovableComponent.FallDown(x, newY);

                _pieces[x, newY] = piece;
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }
            else if (this.CanMoveDiagonalDown(x, y, out int diagX, out int diagY, _inverse))
            {
                hasMove = true;
                StartCoroutine(FallDiagnalDown(x, y));
            }
        }

        //fall down lowest piece to connect grids
        if(IsConnected(x)) hasMove = FallDownLowestPiece(x) ? true : hasMove;

        //spawn new piece at the top of column
        //if that colum is loss fertility column, it'll be skiped
        if (spawner.IsLossFertilityColumn(x)) return hasMove;

        int yHighest = this.GetHighestRow(x);
        GamePiece pieceBelow = _pieces[x, yHighest];

        if (pieceBelow.Type != PieceType.EMPTY) return hasMove;

        //if that column is get piece from another grid, it'll be skiped and get piece in the origin grid
        if (spawner.IsGetPieceFromAnotherGrid(x, out var originGrid, out var originX))
        {
            originGrid.FillColumn(originX);
            return hasMove;
        }

        int emptyPieceAmount = this.GetPieceAmountThatCanBeSpawnedAtColumn(x);

        if (emptyPieceAmount == 0) return hasMove;

        hasMove = true;

        for (int count = 0; count < emptyPieceAmount; count ++)
        {
            this.CanMoveDirectDown(x, yHighest - 1, out int desY);
            spawner.DestroyPiece(_pieces[x, desY]);
            SpawnNewPieceAtColumn(x, desY, PieceType.NORMAL);
        }

        return hasMove;
    }

    /// <returns>if the piece can be cleared, return true</returns>
    public bool ClearPiece(int x, int y, bool isIgnoreRunning = false)
    {
        if (!this.CanClear(x, y, isIgnoreRunning))
        {
            return false;
        }

        // if (_pieces[x, y].Type != PieceType.BUBBLE)
        // {
            //to ensure when the piece is clearing, does not have any piece move into this
            _pieces[x, y].ChangeToIgnore();
            _pieces[x, y].ClearableComponent.Clear();
        // }

        ClearObstacles(x, y);

        return true;
    }

    private void ClearObstacles(int x, int y)
    {
        for (int adjacentX = x - 1; adjacentX <= x + 1; adjacentX++)
        {
            if (adjacentX == x || adjacentX < 0 || adjacentX >= XDim) continue;

            if (_pieces[adjacentX, y].Type != PieceType.BUBBLE || !_pieces[adjacentX, y].IsClearable()) continue;

            _pieces[adjacentX, y].ClearableComponent.Clear();
        }

        for (int adjacentY = y - 1; adjacentY <= y + 1; adjacentY++)
        {
            if (adjacentY == y || adjacentY < 0 || adjacentY >= YDim) continue;

            if (_pieces[x, adjacentY].Type != PieceType.BUBBLE || !_pieces[x, adjacentY].IsClearable()) continue;

            _pieces[x, adjacentY].ClearableComponent.Clear();
        }
    }

    public void IncreaseMovingPiece ()
    {
        movingPiece++;
    }

    public void DecreaseMovingPiece ()
    {
        movingPiece--;
    }

    public void IncreaseClearingPiece () => clearingPiece++;

    public void DecreaseClearingPiece () => clearingPiece--;

    /// <summary>instance inital piece and then start fill</summary>
    public void StartFill()
    {
        StartCoroutine(ClearAllValidMatchesCoroutine(null));
        TurnOnFillMode();
    }

    public void GrantSkill (int x, int y, PieceType specialType)
    {
        var color = _pieces[x, y].ColorComponent.Color;

        _pieces[x, y].ClearableComponent.ReturnPool(false);

        _pieces[x, y] = SpawnNewPiece(x, y, specialType, color);
    }
}