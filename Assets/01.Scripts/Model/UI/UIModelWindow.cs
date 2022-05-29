using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIModelWindow : MonoBehaviour
{
    [SerializeField] Transform box;

    [Header("Header")]
    [SerializeField] Transform headerArea;
    [SerializeField] TextMeshProUGUI titleField;

    [Header("Content")]
    [SerializeField] Transform contentArea;
    [SerializeField] Transform verticalLayoutArea;
    [SerializeField] Image verticalIcon;
    [SerializeField] TextMeshProUGUI verticalText;
    [Space()]
    [SerializeField] Transform horizontalLayoutArea;
    [SerializeField] Image horizontalIcon;
    [SerializeField] TextMeshProUGUI horizontalText;

    [Header("Input")]
    [SerializeField] TMP_Dropdown dropDown;

    [Header("Footer")]
    [SerializeField] Transform footerArea;
    [SerializeField] Button confirmButton;
    [SerializeField] TextMeshProUGUI confirmText;
    [SerializeField] Button declineButton;
    [SerializeField] TextMeshProUGUI declineText;
    [SerializeField] Button alternateButton;
    [SerializeField] TextMeshProUGUI alternateText;
    [SerializeField] Transform loading;

    Action onConfirmAction;
    Action onDeclineAction;
    Action onAlternateAction;
    Action onEndCloseAction;

    UIAnimateCompoment animCompoment;

    //default active value
    bool boxActive = false;
    bool headerActive = false;
    bool contentActive = false;
    bool footerActive = true;

    ModelWindowType currentType = ModelWindowType.Horizontal;

    private void Awake() {
        ResetActive();

        //add button event
        if (confirmButton)
            confirmButton.onClick.AddListener(() => Confirm());
        if (declineButton)
            declineButton.onClick.AddListener(() => Decline());
        if (alternateButton)
            alternateButton.onClick.AddListener(() => Alternate());

        //set appear and disappear animation
        animCompoment = GetComponentInChildren<UIAnimateCompoment>(true);
        animCompoment.ShowOnEnable = false;
        animCompoment.Duration = 0.2f;
        animCompoment.AnimationType =  UIAnimationTypes.Scale;
        animCompoment.From = Vector3.zero;
        animCompoment.To = Vector3.one;
        animCompoment.EaseType = DG.Tweening.Ease.OutQuad;
        animCompoment.StartPositionOffset = true;

        //set animation event
        animCompoment.OnCompleteActionDisable.AddListener(() =>
        {
            ResetActive();

            this.gameObject.SetActive(false);
            //call end close action after set active false to avoid set active false
            //if we want to open model window on end close action
            onEndCloseAction?.Invoke();
            ResetAction();
        });
    }

    private void ResetAction ()
    {
        onEndCloseAction = onAlternateAction = onConfirmAction = onDeclineAction = null;
    }

    private void ResetActive ()
    {
        //set active box
        box.gameObject.SetActive(boxActive);

        //set active header area
        headerArea.gameObject.SetActive(headerActive);

        //set active content area
        horizontalLayoutArea.gameObject.SetActive(false);
        verticalLayoutArea.gameObject.SetActive(false);
        contentArea.gameObject.SetActive(contentActive);
        SetIcon(null);

        //set data input area
        dropDown.ClearOptions();
        dropDown.gameObject.SetActive(false);

        //set active footer area
        confirmButton.gameObject.SetActive(true);
        declineButton.gameObject.SetActive(false);
        alternateButton.gameObject.SetActive(false);
        loading.gameObject.SetActive(false);
        footerArea.gameObject.SetActive(footerActive);

    }

    private void Open() {
        animCompoment.Show();
    }

    public void Close()
    {
        if (box.gameObject.activeSelf == false) return;

        animCompoment.Disable();
    }

    private void Confirm()
    {
        onConfirmAction?.Invoke();
        Close();
    }

    private void Decline()
    {
        onDeclineAction?.Invoke();
        Close();
    }

    private void Alternate()
    {
        onAlternateAction?.Invoke();
        Close();
    }

    public void SetTitle(string title)
    {
        bool hasTittle = !string.IsNullOrEmpty(title);
        headerArea.gameObject.SetActive(hasTittle);
        titleField.text = title;
    }

    public void SetMessage(string message)
    {
        horizontalText.text = message;
        verticalText.text = message;

        bool hasContent = !string.IsNullOrEmpty(message.Trim()) || horizontalIcon.sprite != null || verticalIcon.sprite != null;

        contentArea.gameObject.SetActive(hasContent);
        if(hasContent)
        {
            horizontalLayoutArea.gameObject.SetActive(currentType == ModelWindowType.Horizontal);
            verticalLayoutArea.gameObject.SetActive(currentType == ModelWindowType.Vertical);
        }
    }

    public void SetIcon(Sprite icon)
    {
        bool hasIcon = icon != null;
        // bool hasContent = icon != null || !string.IsNullOrEmpty(horizontalText.text.Trim()) || !string.IsNullOrEmpty(verticalText.text.Trim());

        horizontalIcon.gameObject.SetActive(hasIcon);
        verticalIcon.gameObject.SetActive(hasIcon);

        horizontalIcon.sprite = icon;
        verticalIcon.sprite = icon;
    }

    public void SetType(ModelWindowType type)
    {
        horizontalLayoutArea.gameObject.SetActive(type == ModelWindowType.Horizontal);
        verticalLayoutArea.gameObject.SetActive(type == ModelWindowType.Vertical);
    }

    public void OnConfirmAction(Action action)
    {
        onConfirmAction = action;
    }

    public void OnDeclineAction(Action action)
    {
        bool hasDecline = (action != null);
        declineButton.gameObject.SetActive(hasDecline);
        onDeclineAction += action;
    }

    public void OnAlternateAction(Action action)
    {
        bool hasAlternate = (action != null);
        alternateButton.gameObject.SetActive(hasAlternate);
        onAlternateAction += action;
    }

    public void OnEndCloseAction(Action action)
    {
        //I not use = to can add the action after show
        //And I have to reset this action after close
        onEndCloseAction += action;
    }

    public void ChangeConfirmButtonName(string name)
    {
        confirmText.text = name;
    }

    public void ChangeDeclineButtonName(string name)
    {
        declineText.text = name;
    }

    public void ChangeAlternateButtonName(string name)
    {
        alternateText.text = name;
    }

    public void SetLoadingWindow (bool isLoading)
    {
        loading.gameObject.SetActive(isLoading);

        if (isLoading)
        {
            confirmButton.gameObject.SetActive(false);
            alternateButton.gameObject.SetActive(false);
            declineButton.gameObject.SetActive(false);
        }
        else
        {
            confirmButton.gameObject.SetActive(true);
            alternateButton.gameObject.SetActive(onAlternateAction != null);
            declineButton.gameObject.SetActive(onDeclineAction != null);
        }
    }

    public void SetDropDownInput (List<string> options)
    {
        var hasDropDown = (options != null && options.Count > 0);

        dropDown.gameObject.SetActive (hasDropDown);

        if (!hasDropDown) return;

        dropDown.AddOptions(options);
    }
    public void SetDropDownInput (List<TMP_Dropdown.OptionData> options)
    {
        var hasDropDown = (options != null && options.Count > 0);

        dropDown.gameObject.SetActive (hasDropDown);

        if (!hasDropDown) return;

        dropDown.AddOptions(options);
    }

    public string GetInputValue ()
    {
        return dropDown.options[dropDown.value].text;
    }

    public void Show()
    {
        Open();
    }

    //instance builder
    public class Builder : ModelWindowBuilder<Builder>
    {
        public Builder (UIModelWindow modelWindow)
        {
            StartBuild(modelWindow);
        }
    }

    public Builder StartBuild => new Builder(this);
}

