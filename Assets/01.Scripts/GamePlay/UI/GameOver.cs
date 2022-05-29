using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject screenParent;
    public GameObject scoreParent;
    public Text loseText;
    public Text scoreText;
    public Button nextButton;
    public Button replayButton;
    public Button menuButton;
    public Image[] stars;

    private void Start ()
	{
		screenParent.SetActive(false);

	    for (int i = 0; i < stars.Length; i++)
	    {
	        stars[i].enabled = false;
	    }

        if (nextButton)
            nextButton.onClick.AddListener(OnNextClicked);
        if (replayButton)
            replayButton.onClick.AddListener(OnReplayClicked);
        if (menuButton)
            menuButton.onClick.AddListener(OnMenuClicked);
    }

    public void ShowLose()
    {
        screenParent.SetActive(true);

        nextButton.gameObject.SetActive(false);
        scoreParent.SetActive(false);

        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play("GameOverShow");
        }
    }

    public void ShowWin(int score, int starCount)
    {
        screenParent.SetActive(true);
        loseText.enabled = false;

        scoreText.text = score.ToString();
        scoreText.enabled = false;

        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play("GameOverShow");
        }

        StartCoroutine(ShowWinCoroutine(starCount));
    }

    private IEnumerator ShowWinCoroutine(int starCount)
    {
        yield return new WaitForSeconds(0.5f);

        if (starCount < stars.Length)
        {
            for (int i = 0; i <= starCount; i++)
            {
                stars[i].enabled = true;

                if (i > 0)
                {
                    stars[i - 1].enabled = false;
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        scoreText.enabled = true;
    }

    private void LoadGamePlayScene ()
    {
        UILoader.Instance.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnReplayClicked()
    {
        LoadGamePlayScene();
    }

    private void OnNextClicked()
    {
        if (Data.IsMaxLevel(Data.GetCurrentLevel()))
        {
            GameManager.Instance.ModelWindow.StartBuild.SetTitle("Sad :(").SetMessage("you are in the max level").Show();
            return;
        }

        GameManager.Instance.ModelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Updating current level of user").Show();

        //ensure this coroutine always run regardless this object is deactived
        GameManager.Instance.StartCoroutine(Data.UpdateCurrentLevel(Data.GetNextLevel(), () => {
            GameManager.Instance.ModelWindow.StartBuild.OnEndCloseAction(() => {
                LoadGamePlayScene();
            }).Close();
        }, (message) => {
            GameManager.Instance.ModelWindow.StartBuild.SetLoadingWindow(false).SetTitle("update current level failed").SetMessage(message).Show();
        }));
    }

    private void OnMenuClicked ()
    {
        UILoader.Instance.LoadScene("LevelSelect");
    }
}
