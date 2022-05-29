using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NormalClearAnimation : ClearAnimation
{
    [System.Serializable]
    public struct ColorClear
    {
        public ColorType color;
        public string trigger;
    }

    [SerializeField] ColorClear[] colorClears;

    Dictionary<ColorType, string> colorClearDict;

    ColorType color;

    SpriteRenderer sprite;

    protected virtual void Awake()
    {
        colorClearDict = new Dictionary<ColorType, string>();

        for (int i = 0; i < colorClears.Length; i++)
        {
            if (!colorClearDict.ContainsKey (colorClears[i].color))
            {
                colorClearDict.Add(colorClears[i].color, colorClears[i].trigger);
            }
        }

        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void ClearAction()
    {
        base.ClearAction();

        animator.SetTrigger(colorClearDict[color]);
        return;
    }

    public override void ResetEffect()
    {
        base.ResetEffect();
        
        if (sprite)
        {
            sprite.sprite = null;
        }
    }

    public void Init (Animator animator, ColorType colorType)
    {
        this.animator = animator;
        color = colorType;
    }
}
