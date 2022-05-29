using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class ClearColorPiece : ClearJewelPiece
{
    private ColorType _color;
    private List<GamePiece> targetPieces;

    public ColorType Color
    {
        get => _color;
        set => _color = value;
    }

    protected override void Awake()
    {
        base.Awake();

        targetPieces = new List<GamePiece>();
    }

    private void ExecuteClearColor ()
    {
        targetPieces.ForEach(pieceElement => 
        {
            //if that piece was cleared by another, tha position'll not clear again
            if (pieceElement.GridRef.Pieces[pieceElement.X, pieceElement.Y] != pieceElement)
                return;
            pieceElement.GridRef.ClearPiece(pieceElement.X, pieceElement.Y, true);
        });
        targetPieces.Clear();
    }

    protected override void BeforeClear()
    {
        base.BeforeClear();

        foreach (var grid in piece.Grids)
        {
            targetPieces.AddRange(grid.GetColor(_color));
        }

        //Get positions of simular color piece
        List<Vector3> targetPositions = targetPieces.Select(pieceElement => {
            pieceElement.ChangeToIgnore();

            return pieceElement.transform.position;
            }).ToList();

        ((ColorClearAnimation)clearAnimation).InitLaser(piece.GridRef.GetWorldPosition(piece.X, piece.Y), targetPositions);
    }

    protected override void AfterClear()
    {
        base.AfterClear();
        ExecuteClearColor();
    }
}
