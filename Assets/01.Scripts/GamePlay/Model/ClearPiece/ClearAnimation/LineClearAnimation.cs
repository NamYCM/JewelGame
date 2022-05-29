using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineClearAnimation : SpecialClearAnimation
{
    // [SerializeField] bool isRow;
    // EffectPool effectPool;
    // List<FireAnimation> effects = new List<FireAnimation>();

    // protected override void Awake()
    // {
    //     base.Awake();

    //     effectPool = EffectPool.Instance;
    // }

    // protected override float ClearAction()
    // {
    //     foreach (var fire in effects)
    //     {
    //         fire.LauchEffect();
    //     }

    //     return base.ClearAction();
    // }

    public void InitFire (bool isRow, Vector3 position)
    {
        FireAnimation fire1, fire2;
        if (isRow)
        {
            fire1 = effectPool.GetEffect(EffectType.Fire).GetComponent<FireAnimation>();
            fire1.Init(FireAnimation.Direction.Left, position);

            fire2 = effectPool.GetEffect(EffectType.Fire).GetComponent<FireAnimation>();
            fire2.Init(FireAnimation.Direction.Right, position);
        }
        else
        {
            fire1 = effectPool.GetEffect(EffectType.Fire).GetComponent<FireAnimation>();
            fire1.Init(FireAnimation.Direction.Up, position);

            fire2 = effectPool.GetEffect(EffectType.Fire).GetComponent<FireAnimation>();
            fire2.Init(FireAnimation.Direction.Down, position);
        }

        effects.Add(fire1);
        effects.Add(fire2);
    }

    // public void ResetEffect()
    // {
    //     foreach (var fire in effects)
    //     {
    //         fire.ResetEffect();
    //     }

    //     effects.Clear();
    // }
}
