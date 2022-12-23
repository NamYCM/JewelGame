using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
// using Newtonsoft.Json.Utilities;

public class UILoginHandle : MonoBehaviour
{
    Button loginButton;
    [SerializeField] TMP_InputField username, password;
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
        APIAccessObject.Instance.StartCoroutine(APIAccesser.LoginCoroutine(user, (userData) => {
            Data.InitUserData(userData);
            Data.SetToken(userData.token);

            // loginController.ModelWindow.SetTitle("load map data");
            try
            {
                Data.InitLevelData(() => {
                    Debug.Log("can load to level select " + Data.MaxLevel());
                    Data.SetVersionBearer(Data.GetLevelVersion());
                    UILoader.Instance.CanLoad = true;

                    // loginController.ModelWindow.OnEndCloseAction(() => {
                    //     UILoader.Instance.LoadScene("LevelSelect");
                    // });
                    // loginController.ModelWindow.Close();
                }, (message) => {
                    // loginController.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("error").SetMessage(message).Show();
                    UILoader.Instance.CanLoad = true;
                    Debug.LogError("load level data failed: \n" + message);
                });
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }

            // UILoader.Instance.condition.methodName = Test;
            loginController.ModelWindow.OnEndCloseAction(() => {
                Debug.Log("start load to new scene");
                UILoader.Instance.CanLoad = false;
                UILoader.Instance.LoadScene("LevelSelect");
            });
            loginController.ModelWindow.Close();
        }, (message) => {
            loginController.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("error").SetMessage(message).Show();
        }));
    }

    bool Test ()
    {
        return true;
    }

    public void Init ()
    {
        // this.username = username;
        // this.password = password;

        loginButton.onClick.AddListener(OnLoginCLicked);

        loginController = UILoginController.Instance;
    }
}
