using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    Level level;
    GameOver gameOver;

    public Text remainingText;
    public Text remainingSubText;
    public Text targetText;
    public Text targetSubtext;
    public Image[] stars;

    UIScore score;
    public UIScore Score => score;

    private int _starIndex = 0;

    public void Init() {
        score = GetComponentInChildren<UIScore>();
        gameOver = FindObjectOfType<GameOver>();
        level = GameManager.Instance.Level;

	    for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i == _starIndex;
        }
    }

    public void SetScore(int score)
    {
        this.score.SetScoreText(score);

        int visibleStar = 0;

        if (score >= level.Score1Star && score < level.Score2Star)
        {
            visibleStar = 1;
        }
        else if  (score >= level.Score2Star && score < level.Score3Star)
        {
            visibleStar = 2;
        }
        else if (score >= level.Score3Star)
        {
            visibleStar = 3;
        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = (i == visibleStar);
        }

        _starIndex = visibleStar;
    }

    public void SetTarget(int target)
    {
        targetText.text = target.ToString();
    }

    public void SetRemaining(int remaining)
    {
        remainingText.text = remaining.ToString();
    }

    public void SetRemaining(string remaining)
    {
        remainingText.text = remaining;
    }

    public void SetLevelType(LevelType type)
    {
        switch (type)
        {
            case LevelType.MOVES:
                remainingSubText.text = "moves remaining";
                targetSubtext.text = "target score";
                break;
            case LevelType.OBSTACLE:
                remainingSubText.text = "moves remaining";
                targetSubtext.text = "bubbles remaining";
                break;
            case LevelType.TIMER:
                remainingSubText.text = "time remaining";
                targetSubtext.text = "target score";
                break;
        }
    }

    public void OnGameWin(int score)
    {
        gameOver.ShowWin(score, _starIndex);
        // if (_starIndex > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name, 0))
        // {
        //     PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, _starIndex);
        // }
    }

    public void OnGameLose()
    {
        gameOver.ShowLose();
    }
}
