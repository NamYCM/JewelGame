using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBombPiece : ClearJewelPiece
{
    protected override void BeforeClear()
    {
        base.BeforeClear();
        // ((SpecialClearAnimation)clearAnimation).ResetEffect();
        ((BombClearAnimation)clearAnimation).InitBomb(piece.GridRef.GetWorldPosition(piece.X, piece.Y));

    }

    public override void Clear()
    {
        base.Clear();
    }
}
