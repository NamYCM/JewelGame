using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleClearAnimation : ClearAnimation
{
    [SerializeField] AnimationClip clearAnimation;

    // Animator animator;

    public void Init (Animator animator)
    {
        this.animator = animator;
    }

    protected override void ClearAction()
    {
        base.ClearAction();
        animator.Play(clearAnimation.name);

        return;
    }

    // public override IEnumerator ClearCoroutine()
    // {
    //     if (animator)
    //     {
    //         animator.Play(clearAnimation.name);

    //         yield return new WaitForSeconds(clearAnimation.length);

    //         SendMessage(ClearEvent.EndClear);
    //     }
    // }
}
