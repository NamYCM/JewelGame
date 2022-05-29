using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILogOutButtonHandle : MonoBehaviour
{
    Button loginButton;

    LevelSelectManager levelSelectManager;

    private void Start() {
        levelSelectManager = LevelSelectManager.Instance;

        loginButton = GetComponent<Button>();
        loginButton.onClick.AddListener(OnLogOutCLicked);
    }

    private void OnLogOutCLicked ()
    {
        levelSelectManager.ModelWindow.StartBuild.SetTitle("Notification").SetMessage("Do you want to log out?").OnConfirmAction(() => {
            LogOut();
        }).OnDeclineAction(() => {
            //close model and do nothing
        }).Show();
    }

    private void LogOut ()
    {
        UILoader.Instance.LoadScene("Login");
    }
}
