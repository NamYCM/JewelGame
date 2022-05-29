using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMoveMap : BuildingMap
{
    [SerializeField] MoveLevelData levelData = new MoveLevelData();

    public MoveLevelData LevelData => levelData;

    public void SetLevelData (MoveLevelData moveLevelData)
    {
        levelData = moveLevelData;
    }

    public override void AddLevel(UILevelSetting levelSetting)
    {
        base.AddLevel(levelSetting);

        // levelData.StarScore = StarScore;

        if (levelSetting.Type != LevelType.MOVES) throw new System.InvalidOperationException("this is not move level");

        levelData.TargetScore = ((UILevelMoveSetting)levelSetting).TargetScore;
        levelData.MoveAmount = ((UILevelMoveSetting)levelSetting).NumMove;
    }

    [System.Serializable]
    public struct MoveLevelData
    {
        // public BuildingMap.StarScoreData StarScore;
        public int TargetScore;
        public int MoveAmount;
    }
}
