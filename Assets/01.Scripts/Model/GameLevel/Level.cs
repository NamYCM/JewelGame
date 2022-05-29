using System.Collections;
using UnityEngine;

public abstract class Level : MonoBehaviour, IObserver
{
    private bool _didWin;
    private bool didEnd = false;

    protected GameManager gameManager;
    protected HUD hud;

    protected int currentScore;

    int score1Star;
    int score2Star;
    int score3Star;

    LevelType type;

    public int Score1Star
    {
        get { return score1Star; }
        set
        {
            if (value < 0) return;
            score1Star = value;
        }
    }
    public int Score2Star
    {
        get { return score2Star; }
        set
        {
            if (value < 0) return;
            score2Star = value;
        }
    }
    public int Score3Star
    {
        get { return score3Star; }
        set
        {
            if (value < 0) return;
            score3Star = value;
        }
    }

    public LevelType Type
    {
        get { return type; }
        protected set { type = value; }
    }

    virtual protected void OnDestroy() {
        if (gameManager != null) gameManager.RemoveRegister(GameManager.GameState.WaitInput, this);
    }

    public virtual void Init()
    {
        gameManager = GameManager.Instance;

        gameManager.RegisterObserver(GameManager.GameState.WaitInput, this);

        if (!hud)
        {
            hud = gameManager.HUD;
        }
    }

    public virtual void StartLevel()
    {
        hud.SetScore(currentScore);
    }

    protected void EndGame ()
    {
        didEnd = true;
    }

    public virtual void GameWin()
    {
        didEnd = true;
        // _didWin = true;
        // StartCoroutine(WaitForGridFill());
    }

    public virtual void GameLose()
    {
        didEnd = true;
        // _didWin = false;
        // StartCoroutine(WaitForGridFill());
    }

    /// <summary>check game win or lose</summary>
    protected abstract bool IsWinGame ();

    public virtual void OnMove()
    {
    }

    public virtual void OnPieceCleared(GamePiece piece)
    {
        currentScore += piece.score;
        var tempCurrentScore = currentScore;
        hud.Score.AddScore(piece.score, Camera.main.WorldToScreenPoint(piece.transform.position), () => {
            hud.SetScore(tempCurrentScore);
        });
    }

    protected void OnGameWin()
    {
        gameManager.ModelWindow.StartBuild.SetLoadingWindow(true).SetTitle("Updating user data").Show();

        Data.ChangeUserMoney(currentScore, null, (message) => {
            throw new System.Exception(message);
        });

        Data.UpdateScore(Data.GetCurrentLevel(), currentScore, null, (message) => {
            throw new System.Exception(message);
        });

        //check is max level
        if (Data.IsMaxLevel(Data.GetCurrentLevel()))
        {
            gameManager.ModelWindow.Close();
            hud.OnGameWin(currentScore);
        }
        else
        {
            //unclock the next level
            APIAccessObject.Instance.StartCoroutine(Data.UnlockLevel(Data.GetNextLevel(), () => {
                gameManager.ModelWindow.Close();
                hud.OnGameWin(currentScore);
            }, (message) => {
                throw new System.Exception(message);
            }));
        }
    }

    protected void OnGameLose ()
    {
        hud.OnGameLose();
    }

    protected virtual void HandleOnWaitInput()
    {
        if (didEnd == false) return;

        _didWin = IsWinGame();
        if (_didWin)
        {
            OnGameWin();
        }
        else
        {
            OnGameLose();
        }

        gameManager.ChangeGameState(GameManager.GameState.EndGame);
    }

    public class Builder : NormalLevelBuilder<Builder>
    {
        public Builder (Level levelObject)
        {
            StartBuild(levelObject);
        }
    }

    /// <summary>build level without add level component</summary>
    public static Builder StartBuild (Level level)
    {
        return new Builder (level);
    }

    public void OnNotify(object key, object data)
    {
        if (key.GetType() == typeof(GameManager.GameState))
        {
            switch ((GameManager.GameState)key)
            {
                case GameManager.GameState.WaitInput:
                    HandleOnWaitInput();
                    break;
                default:
                    break;
            }
        }
    }
}
