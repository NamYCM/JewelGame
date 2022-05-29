using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UILoginHandle : MonoBehaviour
{
    Button loginButton;
    TMP_InputField username, password;
    DataUser user;

    UILoginController loginController;

    private void Awake() {
        loginButton = GetComponent<Button>();
    }

    private void OnLoginCLicked ()
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

        Login();
    }

    private void Login ()
    {
        user = new DataUser(username.text, password.text);
        string jsonUser = JsonUtility.ToJson(user);

        loginController.ModelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Sign In").Show();
        StartCoroutine(APIAccesser.LoginCoroutine(user, (userData) => {
            Data.InitUserData(userData);
            loginController.ModelWindow.OnEndCloseAction(() => {
                UILoader.Instance.LoadScene("LevelSelect");
            });
            loginController.ModelWindow.Close();
        }, (message) => {
            loginController.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("error").SetMessage(message).Show();
        }));
    }

    public void Init (TMP_InputField username, TMP_InputField password)
    {
        this.username = username;
        this.password = password;

        loginButton.onClick.AddListener(OnLoginCLicked);

        loginController = UILoginController.Instance;
    }
}
