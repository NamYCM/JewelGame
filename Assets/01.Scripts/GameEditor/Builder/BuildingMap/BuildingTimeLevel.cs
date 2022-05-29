using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTimeLevel : BuildingMap
{
    [SerializeField] TimeLevelData levelData = new TimeLevelData();

    public TimeLevelData LevelData => levelData;

    public void SetLevelData (TimeLevelData moveLevelData)
    {
        levelData = moveLevelData;
    }

    public override void AddLevel(UILevelSetting levelSetting)
    {
        base.AddLevel(levelSetting);

        if (levelSetting.Type != LevelType.TIMER) throw new System.InvalidOperationException("this is not time level");

        levelData.TargetScore = ((UILevelTimerSetting)levelSetting).TargetScore;
        levelData.TimeInSecond = ((UILevelTimerSetting)levelSetting).TimeInSecond;
    }

    [System.Serializable]
    public struct TimeLevelData
    {
        public int TargetScore;
        public int TimeInSecond;
    }
}
