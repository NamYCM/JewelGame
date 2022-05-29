using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    private GamePiece _piece;
    Animator animator;

    //piece's X, Y before move
    private int originX, originY;

    private Sequence currentSequence = null;
    private Queue<Sequence> SequenceQueue = new Queue<Sequence>();

    ConcreteMovablePiece movablePieceInfor;

    private void Awake()
    {
        _piece = GetComponent<GamePiece>();
        animator = GetComponent<Animator>();

        movablePieceInfor = MovablePieceFactory.GetMovablePieceInfor();
    }

    //used for spawn new piece at column
    public void MoveAndFallDown (int newX, int newY, TweenCallback onEndThisTween = null) 
    {
        Vector3 startPos = _piece.GridRef.GetWorldPosition(_piece.X, _piece.Y);  
        Vector3 breakPos = _piece.GridRef.GetWorldPosition(_piece.X, _piece.GridRef.GetHighestRow(_piece.X) - 1);
        Vector3 endPos = _piece.GridRef.GetWorldPosition(newX, newY);
        if (transform.position != startPos)

        originX = _piece.X;
        originY = _piece.Y;

        _piece.X = newX;
        _piece.Y = newY;

        float moveTime1 = (startPos - breakPos).magnitude / GameStatus.SPEED_OF_PIECE;
        float moveTime2 = (endPos - breakPos).magnitude / GameStatus.SPEED_OF_PIECE;

        //set snap = true because the position in move time is used for caculate for position of new spawned pieace
        transform.DOMove(breakPos, moveTime1, true).SetEase(Ease.Linear).OnStart(() => {
            if (_piece.HasSpritePiece())
            {
                _piece.SpritePieceComponent.HidenJewel();
            }
        }).OnComplete(() => {
            //avoid wrong end position value when using snap
            transform.position = breakPos;
            
            if (_piece.HasSpritePiece())
            {
                _piece.SpritePieceComponent.ResetHiden();
            }
        });
        AddAndPlay(transform.DOMove(endPos, moveTime2).SetEase(Ease.Linear).SetDelay(moveTime1)
        .OnKill( () => {
                onEndThisTween?.Invoke();
            }));
    }

    public void FallDown(int newX, int newY, TweenCallback onStartThisTween = null, TweenCallback onEndSequence = null)
    {
        Vector3 startPos = _piece.GridRef.GetWorldPosition(_piece.X, _piece.Y);  
        Vector3 endPos = _piece.GridRef.GetWorldPosition(newX, newY);

        originX = _piece.X;
        originY = _piece.Y;

        _piece.X = newX;
        _piece.Y = newY;

        float moveTime = (startPos - endPos).magnitude / GameStatus.SPEED_OF_PIECE;
        AddAndPlay(transform.DOMove(endPos, moveTime).SetEase(Ease.Linear).OnStart(onStartThisTween), onEndSequence);

        return;
    }

    public void Swap(int newX, int newY, TweenCallback onEndSwap = null)
    {
        Vector3 startPos = _piece.GridRef.GetWorldPosition(_piece.X, _piece.Y);  
        Vector3 endPos = _piece.GridRef.GetWorldPosition(newX, newY);

        originX = _piece.X;
        originY = _piece.Y;

        _piece.X = newX;
        _piece.Y = newY;

        float moveTime = (startPos - endPos).magnitude / GameStatus.SPEED_OF_PIECE;
        transform.DOMove(endPos, moveTime).OnComplete(() => {
                _piece.transform.position = endPos;
                _piece.SpritePieceComponent.ResetSortingOrder();
                
                onEndSwap?.Invoke();
            });
    }

    /// <summary>reset x, y's piece to nearest x, y</summary>
    public void RollBackGridPosition ()
    {
        _piece.X = originX;
        _piece.Y = originY;
    }

    private void AddAndPlay(Tween tween, TweenCallback onEndSequence = null)
    {
        // Create a paused DOTween sequence to "wrap" our tween
        var sequence = DG.Tweening.DOTween.Sequence();
        
        sequence.Pause();
        // "Wrap" the tween
        sequence.Append(tween);
        // Add tween to queue
        SequenceQueue.Enqueue(sequence);
        // If this is the only tween in queue, play it immediately
        if (currentSequence == null)
        {
            _piece.GridRef.IncreaseMovingPiece();
            
            if (animator) animator.SetTrigger(movablePieceInfor.StartMoveAnimationTrigger);
            // tween.SetEase(Ease.InOutQuad);
            tween.SetEase(Ease.InSine);
            currentSequence = SequenceQueue.Dequeue().Play();
        }
        // When the tween finishes, we'll evaluate the queue
        sequence
        .OnKill(() => {
            OnComplete();

            onEndSequence?.Invoke();
        });
    }
    
    private void OnComplete()
    {
        // Other tweens awaiting?
        if (SequenceQueue.Count > 0)
        {
            // Play next tween in queue
            currentSequence = SequenceQueue.Dequeue().Play();
        }
        else
        {
            currentSequence = null;

            if (animator) 
            {
                var nextClips = animator.GetNextAnimatorClipInfo(0);
                if (nextClips.Length > 0 && nextClips[0].clip.name == movablePieceInfor.StartMoveAnimationName) 
                {
                    //do not end move
                }    
                else if (_piece.GridRef.IsOutOfGrid(_piece.X, _piece.Y + 1) 
                    || !_piece.GridRef.Pieces[_piece.X, _piece.Y + 1].IsMovable() 
                    || !_piece.GridRef.Pieces[_piece.X, _piece.Y + 1].MovableComponent.IsRunning())
                {
                    animator.SetTrigger(movablePieceInfor.TouchGroundAnimationTrigger);
                }
            }
            if (gameObject.activeSelf == true)
                StartCoroutine(_piece.GridRef.ClearAllValidMatchesCoroutine());
            _piece.GridRef.DecreaseMovingPiece();

            // StartCoroutine(CountStoodStillTime());
        }
    }

    public bool IsRunning()
    {
        // Are tweens being processed?
        return currentSequence != null;
    }

    public void Destroy()
    {
        if (currentSequence == null) return;

        SequenceQueue.Clear();

        currentSequence.Kill();
        currentSequence = null;
    }
}
