using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModelWindowType
{
    Horizontal,
    Vertical
}

public interface IModelWindowBuilder
{
    IModelWindowBuilder SetTitle(string title);
    IModelWindowBuilder SetMessage(string message);
    IModelWindowBuilder SetIcon(Sprite icon);
    IModelWindowBuilder SetType(ModelWindowType type);
    IModelWindowBuilder OnConfirmAction(Action action);
    IModelWindowBuilder OnDeclineAction(Action action);
    IModelWindowBuilder OnAlternateAction(Action action);
    IModelWindowBuilder ChangeConfirmButtonName(string name);
    IModelWindowBuilder ChangeDeclineButtonName(string name);
    IModelWindowBuilder ChangeAlternateButtonName(string name);
    void Show();
}
