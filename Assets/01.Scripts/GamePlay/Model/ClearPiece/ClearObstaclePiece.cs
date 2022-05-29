using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObstaclePiece : ClearablePiece
{
    // [SerializeField] AnimationClip clearAnimation;

    // protected override IEnumerator ClearCoroutine()
    // {
    //     _isBeingCleared = true;

    //     Animator animator = GetComponent<Animator>();

    //     if (animator)
    //     {
    //         animator.Play(clearAnimation.name);

    //         yield return new WaitForSeconds(clearAnimation.length);
            
    //         ReturnPool();
    //         // Destroy(gameObject);
    //     }

    //     _isBeingCleared = false;
    // }

    protected override void BeforeClear()
    {        
        base.BeforeClear();

        ((ObstacleClearAnimation)clearAnimation).Init(animator);
    }

    // protected override void AfterClear()
    // {
    //     base.AfterClear();        
    //     piece.GridRef.StartFillColumn();
    // }
}
