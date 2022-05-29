using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaserAnimation : EffectAnimation
{
    float duration = GameStatus.CLEAR_TIME;
    Vector3 target;

    public void Init (Vector3 position, Vector3 target)
    {
        this.target = target;
        transform.localPosition = position;
        transform.right = target - position;
    }

    public override void LauchEffect ()
    {
        gameObject.SetActive(true);
        transform.DOMove(target, duration).SetEase(Ease.OutCubic);
    }

    public override void ResetEffect ()
    {
        // transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
