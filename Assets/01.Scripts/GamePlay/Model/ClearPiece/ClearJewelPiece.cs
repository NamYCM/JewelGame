using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearJewelPiece : ClearablePiece
{
    ColorPiece colorPiece;

    protected override void Awake()
    {
        base.Awake();
        
        colorPiece = GetComponent<ColorPiece>();
    }

    protected override void BeforeClear()
    {
        base.BeforeClear();

        if(piece.IsMovable())
            piece.MovableComponent.Destroy();    
        
        ((NormalClearAnimation)clearAnimation).Init(animator, colorPiece.Color);
    }
}
