﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISignUpHandle : MonoBehaviour
{
    Button signUpButton;
    TMP_InputField username, password;

    UILoginController loginController;

    private void Awake() {
        signUpButton = GetComponent<Button>();
    }

    private void OnSignUpCLicked ()
    {
        if (!username || !password)
        {
            throw new System.NullReferenceException("username or password input is null");
        }

        if (username.text.Trim() == "" || password.text.Trim() == "")
        {
            loginController.ModelWindow.StartBuild.SetTitle("Try again").SetMessage("username and password is not allowed empty").Show();
            return;
        }

        SignUp();
    }

    private void SignUp ()
    {
        DataUser user = new DataUser(username.text, password.text);
        string jsonUser = JsonUtility.ToJson(user);

        loginController.ModelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Sign Up").Show();
        StartCoroutine(APIAccesser.SignUpCoroutine(user, () => {
            loginController.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("Congragurate!!").SetMessage("Sign up succesful").Show();
        }, (message) => {
            loginController.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("Try again").SetMessage(message).Show();
        }));
    }

    public void Init (TMP_InputField username, TMP_InputField password)
    {
        this.username = username;
        this.password = password;

        signUpButton.onClick.AddListener(OnSignUpCLicked);

        loginController = UILoginController.Instance;
    }
}
