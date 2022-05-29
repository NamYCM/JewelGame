using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelPiece : GamePiece
{
    private SpriteRenderer pieceSprite;
    
    protected override void Awake()
    {
        base.Awake();

        pieceSprite = transform.Find("piece").GetComponent<SpriteRenderer>();
    }
}
