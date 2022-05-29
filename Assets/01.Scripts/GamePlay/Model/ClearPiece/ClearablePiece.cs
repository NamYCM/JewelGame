using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ClearablePiece : MonoBehaviour, IObserver
{
    protected bool _isBeingCleared = false;
    public bool IsBeingCleared => _isBeingCleared;

    protected GamePiece piece;

    PiecePool piecePool;

    protected ClearAnimation clearAnimation;

    GameManager gameManager;

    protected Animator animator { get; private set; }

    protected virtual void Awake()
    {
        piece = GetComponent<GamePiece>();
        clearAnimation = GetComponentInChildren<ClearAnimation>();
        animator = GetComponent<Animator>();

        clearAnimation.RegisterObserver(ClearAnimation.ClearEvent.EndClear, this);

        piecePool = PiecePool.Instance;
        gameManager = GameManager.Instance;
    }

    protected virtual void BeforeClear()
    {
        piece.GridRef.IncreaseClearingPiece();

        _isBeingCleared = true;
    }

    protected virtual void AfterClear()
    {
        piece.GridRef.DecreaseClearingPiece();

        ReturnPool();

        if (piece.GridRef.Pieces[piece.X, piece.Y].Type == piece.Type)
            piece.GridRef.SpawnEmptyPiece(piece.X, piece.Y);

        piece.GridRef.StartFillGrid();
    }

    public virtual void Clear()
    {
        if (IsBeingCleared)
        {
            return;
        }

        BeforeClear();
        try
        {
            StartCoroutine(clearAnimation.ClearCoroutine());
        }
        catch (System.Exception ex)
        {
            Debug.Log(gameObject.activeSelf + "/n" + ex.StackTrace);
        }
    }

    public void ReturnPool (bool isEarnScore = true)
    {
        _isBeingCleared = false;
        clearAnimation.ResetEffect();
        piecePool.ReturnToPool(piece);

        if (piece.HasSpritePiece()) piece.SpritePieceComponent.ResetSprite();

        if (isEarnScore) gameManager.Level.OnPieceCleared(piece);
    }

    public void OnNotify(object key, object data)
    {
        if (typeof(ClearAnimation.ClearEvent) == key.GetType())
        {
            switch ((ClearAnimation.ClearEvent)key)
            {
                case ClearAnimation.ClearEvent.EndClear:
                    AfterClear ();
                    break;
            }
        }
    }
}
