using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Model window Builder
public abstract class WindowBuilder
{
    protected UIModelWindow modelWindow;

    protected WindowBuilder(){}

    protected void StartBuild(UIModelWindow modelWindow)
    {
        this.modelWindow = modelWindow;

        //to be able set active for another child object
        modelWindow.gameObject.SetActive(true);
    }

    public UIModelWindow Show()
    {
        modelWindow.Show();
        return modelWindow;
    }

    public UIModelWindow Close()
    {
        modelWindow.Close();
        return modelWindow;
    }
}

public class WindowHeaderBuilder<B> : WindowBuilder where B : WindowHeaderBuilder<B>
{
    protected WindowHeaderBuilder () {}

    protected B Self => (B) this;

    public B SetTitle(string title)
    {
        modelWindow.SetTitle(title);
        return Self;
    }
}

public class WindowContentBuilder<B> : WindowHeaderBuilder<B> where B : WindowContentBuilder<B>
{
    protected WindowContentBuilder () {}

    public B SetMessage (string message)
    {
        modelWindow.SetMessage(message);
        return Self;
    }

    public B SetIcon (Sprite icon)
    {
        modelWindow.SetIcon(icon);
        return Self;
    }

    public B SetType (ModelWindowType type)
    {
        modelWindow.SetType(type);
        return Self;
    }
}

public class WindowInputBuilder<B> : WindowContentBuilder<B> where B : WindowInputBuilder<B>
{
    protected WindowInputBuilder () {}

    public B SetDropDownInput (List<TMP_Dropdown.OptionData> options)
    {
        modelWindow.SetDropDownInput(options);
        return Self;
    }
    public B SetDropDownInput (List<string> options)
    {
        modelWindow.SetDropDownInput(options);
        return Self;
    }

    public B SetInputField1 (string holder, string value, TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard, int characterLimit = 0)
    {
        modelWindow.SetInputField1(holder, value, contentType, characterLimit);
        return Self;
    }
    public B SetInputField2 (string holder, string value, TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard, int characterLimit = 0)
    {
        modelWindow.SetInputField2(holder, value, contentType, characterLimit);
        return Self;
    }
    public B SetInputField3 (string holder, string value, TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard, int characterLimit = 0)
    {
        modelWindow.SetInputField3(holder, value, contentType, characterLimit);
        return Self;
    }
    public B SetInputField4 (string holder, string value, TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard, int characterLimit = 0)
    {
        modelWindow.SetInputField4(holder, value, contentType, characterLimit);
        return Self;
    }
}

public class WindowFooterBuilder<B> : WindowInputBuilder<B> where B : WindowFooterBuilder<B>
{
    protected WindowFooterBuilder () {}

    public B OnConfirmAction(Action action)
    {
        modelWindow.OnConfirmAction(action);
        return Self;
    }

    public B OnDeclineAction(Action action)
    {
        modelWindow.OnDeclineAction(action);
        return Self;
    }

    public B OnAlternateAction(Action action)
    {
        modelWindow.OnAlternateAction(action);
        return Self;
    }

    public B ChangeConfirmButtonName(string name)
    {
        modelWindow.ChangeConfirmButtonName(name);
        return Self;
    }

    public B ChangeDeclineButtonName(string name)
    {
        modelWindow.ChangeDeclineButtonName(name);
        return Self;
    }

    public B ChangeAlternateButtonName(string name)
    {
        modelWindow.ChangeAlternateButtonName(name);
        return Self;
    }

    public B SetLoadingWindow (bool isLoading)
    {
        modelWindow.SetLoadingWindow(isLoading);
        return Self;
    }

    public B OnEndCloseAction (Action action)
    {
        modelWindow.OnEndCloseAction(action);
        return Self;
    }

    /// <summary>use to when you want open this model window at the end of close with actions are not null</summary>
    public B OnEndCloseActionCoroutine (MonoBehaviour coroutineObject, Action action)
    {
        modelWindow.OnEndCloseActionCoroutine(coroutineObject, action);
        return Self;
    }
}

//all window builder, use to actually build model window
public class ModelWindowBuilder<B> : WindowFooterBuilder<B> where B : ModelWindowBuilder<B>
{
    protected ModelWindowBuilder () {}
}
