using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimation : EffectAnimation
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (gameObject.activeSelf == true)
        {
            GamePiece piece = other.GetComponent<GamePiece>();
            if (piece == null) return;
            
            piece.GridRef.ClearPiece(piece.X, piece.Y, true);
        }
    }

    public override void LauchEffect()
    {
        gameObject.SetActive(true);
    }

    public override void ResetEffect()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void Init (Vector3 position)
    {
        transform.position = position;
    }
}
