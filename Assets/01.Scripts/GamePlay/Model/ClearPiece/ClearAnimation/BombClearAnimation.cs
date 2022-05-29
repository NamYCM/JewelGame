using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombClearAnimation : SpecialClearAnimation
{
    public void InitBomb (Vector3 position)
    {
        BombAnimation bomb = effectPool.GetEffect(EffectType.Bomb).GetComponent<BombAnimation>();
        bomb.Init(position);

        effects.Add(bomb);
    }
}
