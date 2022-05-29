using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpritePiece : MonoBehaviour
{
    int sortingOrder = 0;
    SpriteRenderer sprite;

    ConcreteSpritePiece spritePieceInfor;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        spritePieceInfor = SpritePieceFactory.GetSpritePiece();
        
        sortingOrder = sprite.sortingOrder;
    }

    public void SetSprite (Sprite spriteRenderer)
    {
        sprite.sprite = spriteRenderer;
    }

    public void ResetScale ()
    {
        transform.localScale = Vector3.one;
    }

    public void ResetSprite ()
    {
        //reset size
        transform.localScale = Vector3.one;

        //reset color
        sprite.color = spritePieceInfor.DefaultColor;
    }

    public void SetSortingOrder (int order) 
    {
        sprite.sortingOrder = order;
    }

    public void ResetSortingOrder ()
    {
        sprite.sortingOrder = sortingOrder;
    }

    public void HidenJewel () 
    {
        sprite.sortingLayerName = spritePieceInfor.HiddenLayer;
    }

    public void ResetHiden ()
    {
        sprite.sortingLayerName = spritePieceInfor.NormalLayer;
    }
}

