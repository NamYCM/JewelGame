using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuildLevelUtility
{
    //move level
    public class MoveLevelBuilder : LevelMoveBuilder<MoveLevelBuilder>
    {
        public MoveLevelBuilder (GameObject levelObject)
        {
            AddLevelComponent(levelObject, LevelType.MOVES);
        }
    }

    public static MoveLevelBuilder New (this LevelMoves level, GameObject levelObject)
    {
        return new MoveLevelBuilder(levelObject);
    }

    //time level
    public class TimeLevelBuilder : LevelTimerBuilder<TimeLevelBuilder>
    {
        public TimeLevelBuilder (GameObject levelObject)
        {
            AddLevelComponent(levelObject, LevelType.TIMER);
        }

    }

    public static TimeLevelBuilder New (this LevelTimer level, GameObject levelObject)
    {
        return new TimeLevelBuilder (levelObject);
    }
}
