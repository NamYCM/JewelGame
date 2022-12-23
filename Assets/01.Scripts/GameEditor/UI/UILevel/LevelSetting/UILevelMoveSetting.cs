using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelMoveSetting : UILevelSetting
{
    [SerializeField] InputField targetScore;
    [SerializeField] InputField numMove;

    public int TargetScore => int.Parse(targetScore.text);
    public int NumMove => int.Parse(numMove.text);

    private void Awake() {
        type = LevelType.MOVES;
    }

    public override void TurnOnSetting()
    {
        base.TurnOnSetting();

        //set default value
        if(string.IsNullOrEmpty(targetScore.text.Trim())) targetScore.text = "0";
        if(string.IsNullOrEmpty(numMove.text.Trim())) numMove.text = "0";
    }

    public override void ResetTargetLevel()
    {
        targetScore.text = "0";
        numMove.text = "0";
    }

    public override void SetLevelData(BuildingMap mapData)
    {
        base.SetLevelData(mapData);

        targetScore.text = ((BuildingMoveMap)mapData).LevelData.targetScore.ToString();
        numMove.text = ((BuildingMoveMap)mapData).LevelData.moveAmount.ToString();
    }
}
