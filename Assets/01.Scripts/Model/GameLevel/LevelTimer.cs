using UnityEngine;

public class LevelTimer : Level
{
    public int timeInSeconds;
    public int targetScore;

    private float timer;
    private bool timeOut = false;

    private void Awake() {
        Type = LevelType.TIMER;
    }

    public override void Init ()
	{
        base.Init();
	}

    public override void StartLevel()
    {
        base.StartLevel();

        hud.SetLevelType(Type);
        hud.SetTarget(targetScore);
        hud.SetRemaining($"{timeInSeconds / 60}:{timeInSeconds % 60:00}");
    }

    // TODO convert this into a Coroutine for efficiency
    private void Update()
    {
        if (timeOut) { return; }

        timer += Time.deltaTime;
        hud.SetRemaining(
            $"{(int) Mathf.Max((timeInSeconds - timer) / 60, 0)}:{(int) Mathf.Max((timeInSeconds - timer) % 60, 0):00}");

        if (timeInSeconds - timer <= 0)
        {
            EndGame();

            if (gameManager.CurrentState != GameManager.GameState.WaitInput) return;

            if (currentScore >= targetScore)
            {
                OnGameWin();
                // GameWin();
            }
            else
            {
                OnGameLose();
                // EndGame();
                // GameLose();
            }

            gameManager.ChangeGameState(GameManager.GameState.EndGame);
        }
    }

    protected override bool IsWinGame()
    {
        if (currentScore >= targetScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>add level component and start build</summary>
    public static Builder New (GameObject levelObject)
    {
        return new Builder (levelObject);
    }

    /// <summary>build level without add level component</summary>
    public static Builder StartBuild (LevelTimer level)
    {
        return new Builder (level);
    }

    public new class Builder : LevelTimerBuilder<Builder>
    {
        public Builder (GameObject levelObject)
        {
            AddLevelComponent(levelObject, LevelType.TIMER);
        }
        public Builder (LevelTimer levelObject)
        {
            StartBuild(levelObject);
        }
    }
}
