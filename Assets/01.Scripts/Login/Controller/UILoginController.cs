﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UILoginController : SingletonMono<UILoginController>
{
    [SerializeField] TMP_InputField username, password;
    [SerializeField] UIModelWindow modelWindow;
    public UIModelWindow ModelWindow => modelWindow;

    UILoginHandle loginButton;
    UISignUpHandle signUpButton;

    private void Awake() {
        loginButton = GetComponentInChildren<UILoginHandle>();
        signUpButton = GetComponentInChildren<UISignUpHandle>();

        //load level data
        modelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Loading data level").Show();
        Data.InitLevelData(() => {
            modelWindow.Close();
        }, (message) => {
            Debug.Log("log failed");
            modelWindow.StartBuild.OnEndCloseAction(() => {
                modelWindow.StartBuild.SetTitle("Error").SetMessage(message).Show();
            });
            modelWindow.Close();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        loginButton.Init(username, password);
        signUpButton.Init(username, password);
    }
}
