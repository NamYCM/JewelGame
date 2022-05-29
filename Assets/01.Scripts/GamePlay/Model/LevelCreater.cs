using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreater : MonoBehaviour
{
    BuildingMap mapData;
    Level level;

    public Level Level => level;

    void GenerateLevel()
    {
        switch (mapData.StarScore.LevelType)
        {
            case LevelType.MOVES:
                level = LevelMoves.New(gameObject).SetMoveAmount(((BuildingMoveMap)mapData).LevelData.MoveAmount)
                    .SetTargetScore(((BuildingMoveMap)mapData).LevelData.TargetScore).Build();
                break;
            case LevelType.TIMER:
                level = LevelTimer.New(gameObject).SetTime(((BuildingTimeLevel)mapData).LevelData.TimeInSecond)
                    .SetTargetScore(((BuildingTimeLevel)mapData).LevelData.TargetScore).Build();
                break;
            case LevelType.OBSTACLE:
                return;
                // level = gameObject.AddComponent<LevelObstacles>();
                // break;
            default:
                return;
                // break;
        }

        level = Level.StartBuild(level).SetScore1Star(mapData.StarScore.Star1Score)
                                        .SetScore2Star(mapData.StarScore.Star2Score)
                                        .SetScore3Star(mapData.StarScore.Star3Score).Build();
    }

    public void InitLevel (BuildingMap map)
    {
        this.mapData = map;

        GenerateLevel();

        level.Init();
    }

    public void StartLevel ()
    {
        level.StartLevel();
    }
}
