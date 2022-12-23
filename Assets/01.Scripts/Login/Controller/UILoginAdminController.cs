using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoginAdminController : SingletonMono<UILoginController>
{
    [SerializeField] TMP_InputField username, password;
    [SerializeField] UIModelWindow modelWindow;
    [SerializeField] Button loginButton;
    public UIModelWindow ModelWindow => modelWindow;

    DataUser user;

    private void Awake() {
        UILoader.Instance.Close();
    }

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
        APIAccessObject.Instance.StartCoroutine(APIAccesser.LoginAdminCoroutine(username.text, password.text, (userData) => {
            Data.SetToken(userData.token);
            modelWindow.OnEndCloseAction(() => {
                UILoader.Instance.LoadScene("GameEditor");
            });
            modelWindow.Close();
        }, (message) => {
            modelWindow.StartBuild.SetLoadingWindow(false).SetTitle("error").SetMessage(message).Show();
        }));
    }

    string email = null;

    public void ForgetPassword ()
    {
        modelWindow.StartBuild.SetTitle("Enter your email").SetInputField1("Email", email, TMP_InputField.ContentType.EmailAddress).OnConfirmAction(() => {
            email = modelWindow.GetInputValue();
            if (string.IsNullOrEmpty(email))
            {
                modelWindow.OnEndCloseActionCoroutine(this, () => {
                    modelWindow.StartBuild.SetTitle("Error").SetMessage("Please enter your email").OnEndCloseActionCoroutine(this, ForgetPassword).Show();
                });
            }
            else
            {
                modelWindow.OnEndCloseAction(() => {
                    modelWindow.StartBuild.SetTitle("Sending").SetMessage("Sending reset password link to your email").SetLoadingWindow(true).Show();
                    APIAccessObject.Instance.StartCoroutine(APIAccesser.ResetPasswordAdmin(email, () => {
                        modelWindow.OnEndCloseAction(() => {
                            modelWindow.StartBuild.SetTitle("Successful").SetMessage("Please check your email").Show();
                        });
                        modelWindow.Close();
                    }, (message) => {
                        modelWindow.OnEndCloseActionCoroutine(this, () => {
                            modelWindow.StartBuild.SetTitle("Failed").SetMessage(message).OnEndCloseActionCoroutine(this, ForgetPassword).Show();
                        });
                        modelWindow.Close();
                    }));
                });
            }
        }).OnDeclineAction(() => {
            email = modelWindow.GetInputValue();
        }).Show();
    }

    public void Quit ()
    {
        modelWindow.StartBuild.SetTitle("Confirm").SetMessage("Do you want to quit?").OnConfirmAction(() => {
            Application.Quit();
        }).OnDeclineAction(() => {}).Show();
    }
}
