using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UILoginAdminController : SingletonMono<UILoginController>
{
    [SerializeField] TMP_InputField username, password;
    [SerializeField] UIModelWindow modelWindow;
    [SerializeField] Button loginButton;
    public UIModelWindow ModelWindow => modelWindow;

    DataUser user;

    // Start is called before the first frame update
    void Start()
    {
        loginButton.onClick.AddListener(OnLoginCLicked);
    }

    private void OnLoginCLicked ()
    {
        if (!username || !password)
        {
            throw new System.NullReferenceException("username or password input is null");
        }

        if (username.text.Trim() == "" || password.text.Trim() == "")
        {
            modelWindow.StartBuild.SetTitle("Try again").SetMessage("username and password is not allowed empty").Show();
            return;
        }

        Login();
    }

    private void Login ()
    {
        // user = new DataUser(username.text, password.text);
        // string jsonUser = JsonUtility.ToJson(user);

        modelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Sign In").Show();
        StartCoroutine(APIAccesser.LoginAdminCoroutine(username.text, password.text, (userData) => {
            modelWindow.OnEndCloseAction(() => {
                UILoader.Instance.LoadScene("GameEditor");
            });
            modelWindow.Close();
        }, (message) => {
            modelWindow.StartBuild.SetLoadingWindow(false).SetTitle("error").SetMessage(message).Show();
        }));
    }
}
