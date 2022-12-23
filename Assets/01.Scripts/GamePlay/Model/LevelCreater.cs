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
        switch (mapData.StarScore.levelType)
        {
            case LevelType.MOVES:
                level = LevelMoves.New(gameObject).SetMoveAmount(((BuildingMoveMap)mapData).LevelData.moveAmount)
                    .SetTargetScore(((BuildingMoveMap)mapData).LevelData.targetScore).Build();
                break;
            case LevelType.TIMER:
                level = LevelTimer.New(gameObject).SetTime(((BuildingTimeLevel)mapData).LevelData.timeInSecond)
                    .SetTargetScore(((BuildingTimeLevel)mapData).LevelData.targetScore).Build();
                break;
            case LevelType.OBSTACLE:
                return;
                // level = gameObject.AddComponent<LevelObstacles>();
                // break;
            default:
                return;
                // break;
        }

        level = Level.StartBuild(level).SetScore1Star(mapData.StarScore.star1Score)
                                        .SetScore2Star(mapData.StarScore.star2Score)
                                        .SetScore3Star(mapData.StarScore.star3Score).Build();
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
