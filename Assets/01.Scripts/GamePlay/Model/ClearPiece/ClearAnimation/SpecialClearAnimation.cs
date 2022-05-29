using System.Collections.Generic;

//TODO consider to change this class to component pattern
public class SpecialClearAnimation : NormalClearAnimation
{
    protected EffectPool effectPool;
    protected List<EffectAnimation> effects = new List<EffectAnimation>();

    protected override void Awake()
    {
        base.Awake();

        effectPool = EffectPool.Instance;
    }

    protected override void ClearAction()
    {
        //TODO change inherit system
        base.ClearAction();
        foreach (var effect in effects)
        {
            effect.LauchEffect();
        }

        return;
    }

    public override void ResetEffect()
    {
        base.ResetEffect();
        
        foreach (var effect in effects)
        {
            effect.ResetEffect();
            effectPool.ReturnToPool(effect);
        }

        effects.Clear();
    }
}
