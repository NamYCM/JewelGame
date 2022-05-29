using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class ClearAnimation : SubjectMono
{
    protected Animator animator;

    /// <summary>implement clear animation</summary>
    protected virtual void ClearAction ()
    {
        //reset animator to implement clear action immidiately
        animator.Rebind();
        DOTween.Kill(this);
    }
    public virtual void ResetEffect ()
    {
        // animator.Rebind();
        // DOTween.Kill(this);
    }

    public IEnumerator ClearCoroutine()
    {
        if (animator)
        {
            ClearAction();
            yield return new WaitForSeconds(GameStatus.CLEAR_TIME);

            ResetEffect();

            //wait to end of frame to fill column
            //this thing'll make fill column of clear action execute last one
            //to make sure does not get miss any match just because the upper piece is moving 
            // yield return new WaitForEndOfFrame();
            SendMessage(ClearEvent.EndClear);
        }
    }

    public enum ClearEvent
    {
        EndClear
    }
}
