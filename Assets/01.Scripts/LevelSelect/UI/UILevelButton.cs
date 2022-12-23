using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelButton : MonoBehaviour, IObserver
{
    [SerializeField] Image[] stars;
    TextMeshProUGUI text;

    UIChain chain;

    int level = 0;

    Button button;

    //TODO if use pool pattern, create a disable function to reset this value
    bool isInit = false;
    public bool IsInit => isInit;

    private void Awake() {
        button = GetComponentInChildren<Button>();
        chain = GetComponentInChildren<UIChain>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private bool IsLock (int level) => Data.IsLock(level);
    private bool IsAbleToOpen (int level) => Data.IsAbleToOpen(level);

    private void SetChainActive ()
    {
        if (IsLock(level))
        {
            chain.gameObject.SetActive(true);
        }
        else if (IsAbleToOpen(level))
        {
            chain.gameObject.SetActive(true);

            chain.Init(level);
            // chain.StartDestroyChain();
        }
        else
        {
            chain.gameObject.SetActive(false);
        }
    }

    private int CaculateStarAmount ()
    {
        int currentScore = Data.GetScore(level);

        var starScore = Data.GetMapInfor(level).StarScore;

        if (currentScore < starScore.star1Score)
            return 0;
        else if (currentScore < starScore.star2Score)
            return 1;
        else if (currentScore < starScore.star3Score)
            return 2;
        else
            return 3;
    }

    private void SetStarActive ()
    {
        if (IsLock(level))
        {
            foreach (var star in stars)
            {
                star.gameObject.SetActive(false);
            }
        }
        else
        {
            int starAmount = CaculateStarAmount();

            for (int i = 0; i < stars.Length; i ++)
            {
                stars[i].gameObject.SetActive(i < starAmount);
            }
        }
    }

    private void OnButtonClicked ()
    {
        UILoader.Instance.CanLoad = false;
        UILoader.Instance.LoadScene("GamePlay");
        APIAccessObject.Instance.StartCoroutine(Data.UpdateCurrentLevel(level, () => {
            Debug.Log("1");
            UILoader.Instance.CanLoad = true;
        }, (message) => {
            UILoader.Instance.Close();
            Debug.LogError("update current level failed \n" + message);
        }));

        //check version
        // APIAccessObject.Instance.StartCoroutine(LevelData.GetCurrentVersion((levelData) => {
        //     if (levelData.version == Data.GetLevelVersion())
        //     {
        //         UILoader.Instance.CanLoad = false;
        //         UILoader.Instance.LoadScene("GamePlay");
        //         APIAccessObject.Instance.StartCoroutine(Data.UpdateCurrentLevel(level, () => {
        //             UILoader.Instance.CanLoad = true;
        //         }, (message) => {
        //             UILoader.Instance.StopAllCoroutines();
        //             Debug.LogError("update current level failed \n" + message);
        //         }));
        //     }
        //     else
        //     {
        //         LevelSelectManager.Instance.ModelWindow.StartBuild.SetTitle("Notification").SetMessage("Data is obsoleted")
        //         .OnConfirmAction(() => {
        //             UILoader.Instance.LoadScene(0);
        //         }).Show();
        //     }
        // }, (message) => {
        //     Debug.LogError(message);
        //     LevelSelectManager.Instance.ModelWindow.StartBuild.SetTitle("Error").SetMessage(message).Show();
        // }));
    }

    private void SetTextLevel ()
    {
        text.text = level.ToString();
    }

    public void Init (int level)
    {
        //to ensure does not init again
        if (isInit) return;

        isInit = true;

        this.name = level.ToString();
        this.level = level;

        SetChainActive();
        SetStarActive();
        SetTextLevel();

        button.onClick.AddListener(OnButtonClicked);
    }

    public void StartChainAnimation ()
    {
        if (chain.gameObject.activeSelf == true && IsAbleToOpen(level))
        {
            chain.StartDestroyChain();
        }
    }

    public void OnNotify(object key, object data)
    {
        throw new System.NotImplementedException();
    }
}
