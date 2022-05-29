using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelBuilderAbstact
{
    protected Level level;

    protected LevelBuilderAbstact () {}

    protected void AddLevelComponent(GameObject levelObject, LevelType type)
    {
        switch (type)
        {
            case LevelType.MOVES:
                level = levelObject.AddComponent<LevelMoves>();
                break;
            case LevelType.TIMER:
                level = levelObject.AddComponent<LevelTimer>();
                break;
            case LevelType.OBSTACLE:
                level = levelObject.AddComponent<LevelObstacles>();
                break;
            default:
                throw new System.InvalidOperationException("invalid level type");
        }
    }

    protected void StartBuild (Level level)
    {
        this.level = level;
    }

    public Level Build ()
    {
        return level;
    }
}

public class NormalLevelBuilder<B> : LevelBuilderAbstact where B : NormalLevelBuilder<B>
{
    protected NormalLevelBuilder () {}

    protected B Self => (B) this;

    public B SetScore1Star (int score) 
    {
        level.Score1Star = score;
        return Self;
    }

    public B SetScore2Star (int score) 
    {
        level.Score2Star = score;
        return Self;
    }

    public B SetScore3Star (int score) 
    {
        level.Score3Star = score;
        return Self;
    }
}

public class LevelMoveBuilder<B> : NormalLevelBuilder<B> where B : LevelMoveBuilder<B>
{
    protected LevelMoveBuilder () {}

    public B SetMoveAmount (int numMove) 
    {
        ((LevelMoves)level).numMoves = numMove;
        return Self;
    }

    public B SetTargetScore (int score) 
    {
        ((LevelMoves)level).targetScore = score;
        return Self;
    }
}

public class LevelTimerBuilder<B> : NormalLevelBuilder<B> where B : LevelTimerBuilder<B>
{
    protected LevelTimerBuilder () {}

    public B SetTime (int timeInSecond)
    {
        ((LevelTimer)level).timeInSeconds = timeInSecond;
        return Self;
    }

    public B SetTargetScore (int score)
    {
        ((LevelTimer)level).targetScore = score;
        return Self;
    }
}
