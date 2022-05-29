using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
[AddComponentMenu("UI/Blur Panel")]
public class BlurPanel : Image
{
    public bool animate;
    public float time = 0.5f;
    public float delay = 0f;

    CanvasGroup canvasGroup;

    protected override void Reset()
    {
        base.Reset();
        color = Color.black * 0.1f;
    }

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        if (Application.isPlaying)
        {
            material.SetFloat("_Size", 0);
            canvasGroup.alpha = 0;
            DOTween.To(
                () => 0f,
                x => {
                    //update  blur
                    material.SetFloat("_Size", x);
                    canvasGroup.alpha = x;
                },
                1f, time 
            ).SetDelay(delay).SetUpdate(true);
        }
    }
}
