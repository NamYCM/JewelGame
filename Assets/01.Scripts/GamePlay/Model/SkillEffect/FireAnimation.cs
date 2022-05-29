using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimation : EffectAnimation
{
    Camera cam;
    float move = 25f;

    private void Awake() {
        gameObject.SetActive(false);
        cam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (gameObject.activeSelf == true)
        {
            GamePiece piece = other.GetComponent<GamePiece>();
            if (piece == null) return;
            
            bool needClear = false;

            if (transform.right == Vector3.right || transform.right == Vector3.left)
            {
                if (piece.GridRef.GetWorldPosition(piece.X, piece.Y).y - transform.position.y <= 0.1f)
                    needClear = true;
            }
            else if (transform.right == Vector3.up || transform.right == Vector3.down)
            {
                if (piece.GridRef.GetWorldPosition(piece.X, piece.Y).x - transform.position.x <= 0.1f)
                    needClear = true;
            }
            else
            {
                return;
            }

            if(needClear) 
            {
                piece.GridRef.ClearPiece(piece.X, piece.Y, true);
                // if (piece.GridRef.ClearPiece(piece.X, piece.Y, true))
                    // piece.GridRef.StartFillGrid();
            }
            else
            {
                // Debug.Log(piece.X + " " + piece.Y);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.activeSelf == true)
        {
            transform.localPosition += transform.right * (Time.deltaTime * move);

            var viewPos = cam.WorldToViewportPoint(transform.position);
            if (viewPos.x < 0 || viewPos.x > 1|| viewPos.y < 0 || viewPos.y > 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Init (Direction direction, Vector3 position)
    {
        transform.position = position;
        switch (direction)
        {
            case Direction.Left:
                transform.right = Vector3.left;
                break;
            case Direction.Right:
                transform.right = Vector3.right;
                break;
            case Direction.Up:
                transform.right = Vector3.up;
                break;
            case Direction.Down:
                transform.right = Vector3.down;
                break;
            default:
                break;
        }
        
    }

    public override void LauchEffect ()
    {
        gameObject.SetActive(true);
    }

    public override void ResetEffect ()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
