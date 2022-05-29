using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public enum UIAnimationTypes
{
    Move,
    Scale,
    Fade,
}

public class UIAnimateCompoment : MonoBehaviour
{
    public GameObject ObjectAnimate;
    public UIAnimationTypes AnimationType;
    public Ease EaseType;
    public float Duration;
    public float Delay = 0;

    public bool Loop = false;
    public bool StartPositionOffset;
    [Tooltip("if is face animation, the value will be x")]
    public Vector3 From, To;
    public bool ShowOnEnable = true;
    public UnityEvent OnCompleteActionEnable = null, OnCompleteActionDisable = null;
    public UnityEvent OnStartAction = null, OnEnableAction = null;

    private Tween tweenObject;

    private void Start() {
        if (OnStartAction != null)
        {
            OnStartAction.Invoke();
        }
    }

    private void OnEnable() {
        if (OnEnableAction != null)
        {
            OnEnableAction.Invoke();
        }
        if (ShowOnEnable) Show();
    }

    //* Handle tween depend on above data
    private void HandleTween(bool isEnable)
    {
        if (ObjectAnimate == null)
        {
            ObjectAnimate = gameObject;
        }

        switch (AnimationType)
        {
            case UIAnimationTypes.Fade:
                Fade();
                break;
            case UIAnimationTypes.Move:
                Move();
                break;
            case UIAnimationTypes.Scale:
                Scale();
                break;
        }

        tweenObject.SetDelay(Delay);

        if(isEnable) tweenObject.onComplete += OnCompleteEnable; //.SetOnComplete(OnCompleteEnable);
        else
        {
            tweenObject.onComplete += OnCompleteDisable; //.setOnComplete(OnCompleteDisable);
        }


        if (Loop)
        {
            tweenObject.SetLoops(int.MaxValue);
        }
    }

    private void OnCompleteEnable()
    {
        if (OnCompleteActionEnable != null)
        {
            OnCompleteActionEnable?.Invoke();
        }
    }

    private void OnCompleteDisable()
    {
        SwapDirection();
        gameObject.SetActive(false);
        if (OnCompleteActionDisable != null)
        {
            OnCompleteActionDisable?.Invoke();
        }
    }

    private void Fade()
    {
        CanvasGroup canvasGroup;

        if (gameObject.GetComponent<CanvasGroup>() == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup = ObjectAnimate.GetComponent<CanvasGroup>();

        if (StartPositionOffset)
        {
            canvasGroup.alpha = From.x;
        }

        tweenObject = canvasGroup.DOFade(To.x, Duration);
    }

    private void Move()
    {
        RectTransform rectTransform = ObjectAnimate.GetComponent<RectTransform>();

        if (StartPositionOffset) rectTransform.anchoredPosition = From;

        tweenObject = rectTransform.DOAnchorPos3D(To, Duration, false);
    }

    private void Scale ()
    {
        RectTransform rectTransform = ObjectAnimate.GetComponent<RectTransform>();

        if (StartPositionOffset) rectTransform.localScale = From;

        tweenObject = rectTransform.DOScale(To, Duration);
    }

    //swap form and to before disapare
    void SwapDirection()
    {
        var temp = From;
        From = To;
        To = temp;
    }

    //start to appear
    public void Show()
    {
        if (!ShowOnEnable) gameObject.SetActive(true);
        HandleTween(true);
    }

    public void Disable()
    {
        SwapDirection();
        HandleTween(false);
    }
}
