using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorClearAnimation : SpecialClearAnimation
{
    public void InitLaser(Vector3 originPosition, List<Vector3> targetPositions)
    {
        foreach (var position in targetPositions)
        {
            var laser = effectPool.GetEffect(EffectType.Laser).GetComponent<LaserAnimation>();
            laser.Init(originPosition, position);
            effects.Add(laser);
        }
    }
}
