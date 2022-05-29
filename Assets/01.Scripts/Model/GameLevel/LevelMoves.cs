using UnityEngine;

public class LevelMoves : Level
{
    public int numMoves;
    public int targetScore;

    private int _movesUsed = 0;
    
    private void Awake() {
        Type = LevelType.MOVES;
    }

    public override void Init()
    {
        base.Init();
    }

    public override void StartLevel()
    {
        base.StartLevel();

        hud.SetLevelType(Type);
        hud.SetTarget(targetScore);
        hud.SetRemaining(numMoves);
    }

    public override void OnMove()
    {
        base.OnMove();

        _movesUsed++;

        hud.SetRemaining(numMoves - _movesUsed);

        if (numMoves - _movesUsed != 0) return;
        
        EndGame();

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

    /// <summary>add level component and build level</summary>
    public static Builder New (GameObject levelObject)
    {
        return new Builder(levelObject);
    }
    
    /// <summary>build level without add level component</summary>
    public static Builder StartBuild (LevelMoves level)
    {
        return new Builder (level);
    }

    public new class Builder : LevelMoveBuilder<Builder>
    {
        public Builder (GameObject levelObject)
        {
            AddLevelComponent(levelObject, LevelType.MOVES);
        }
        public Builder (LevelMoves levelObject)
        {
            StartBuild(levelObject);
        }
    }
}
