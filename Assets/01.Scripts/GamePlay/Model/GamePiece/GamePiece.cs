using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int IsGet = 0;
    public bool IsDiagDown = false;
    public int score;

    private int _x;
    private int _y;

    public int X
    {
        get => _x;
        set { if (IsMovable()) { _x = value; } }
    }

    public int Y
    {
        get => _y;
        set { if (IsMovable()) { _y = value; } }
    }

    private PieceType originType;
    private PieceType _type;
    public PieceType Type => _type;

    private GridPlay _grid;
    public GridPlay GridRef => _grid;

    private GridPlay[] grids;
    public GridPlay[] Grids => grids;

    private MovablePiece _movableComponent;
    public MovablePiece MovableComponent => _movableComponent;

    private ColorPiece _colorComponent;
    public ColorPiece ColorComponent => _colorComponent;

    private ClearablePiece _clearableComponent;
    public ClearablePiece ClearableComponent => _clearableComponent;

    private SpritePiece spritePieceComponent;
    public SpritePiece SpritePieceComponent => spritePieceComponent;

    private ChoosePiece choosePieceComponent;
    public ChoosePiece ChoosePieceComponent => choosePieceComponent;

    protected virtual void Awake()
    {
        _movableComponent = GetComponent<MovablePiece>();
        _colorComponent = GetComponent<ColorPiece>();
        _clearableComponent = GetComponentInChildren<ClearablePiece>();
        spritePieceComponent = GetComponentInChildren<SpritePiece>();
        choosePieceComponent = GetComponentInChildren<ChoosePiece>();

        grids = GameManager.Instance.Grids;
    }

    public void Init(int x, int y, GridPlay grid, PieceType type)
    {
        _x = x;
        _y = y;
        _grid = grid;
        _type = type;
        originType = type;
    }

    public bool IsMovable()
    {
        return _movableComponent != null;
    }

    public bool IsColored()
    {
        return _colorComponent != null;
    }

    public bool IsClearable()
    {
        return _clearableComponent != null;
    }

    public bool HasSpritePiece()
    {
        return spritePieceComponent != null;
    }

    public bool IsChoosable()
    {
        return choosePieceComponent != null;
    }

    public void ChangeGridRef(GridPlay grid)
    {
        _grid = grid;
        IsGet = 0;
        // transform.parent = grid.transform;
    }

    /// <summary>will be ignore when count piece amount will be spawned or move down but still have another feature</summary>
    public void ChangeToIgnore ()
    {
        _type = PieceType.IGNORE;
    }

    public void ResetIgnore ()
    {
        _type = originType;
    }

    public bool IsIgnored()
    {
        return _type == PieceType.IGNORE;
    }

    public void UpgradeSkill (PieceType specialType)
    {
        _grid.GrantSkill(X, Y, specialType);
    }

    public bool IsSpecialPiece ()
    {
        return _type == PieceType.ROW_CLEAR || _type == PieceType.COLUMN_CLEAR || _type == PieceType.BOMB || _type == PieceType.RAINBOW;
    }
}
