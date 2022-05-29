using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelTimerSetting : UILevelSetting
{
    [SerializeField] InputField targetScore;
    [SerializeField] InputField timeInSecond;

    public int TargetScore => int.Parse(targetScore.text);
    public int TimeInSecond => int.Parse(timeInSecond.text);

    private void Awake() {
        type = LevelType.TIMER;
    }

    public override void TurnOnSetting()
    {
        base.TurnOnSetting();

        //set default value
        if(string.IsNullOrEmpty(targetScore.text.Trim())) targetScore.text = "0";
        if(string.IsNullOrEmpty(timeInSecond.text.Trim())) timeInSecond.text = "0";
    }

    public override void ResetTargetLevel()
    {
        targetScore.text = "0";
        timeInSecond.text = "0";
    }

    public override void SetLevelData(BuildingMap mapData)
    {
        base.SetLevelData(mapData);
        
        targetScore.text = ((BuildingTimeLevel)mapData).LevelData.TargetScore.ToString();
        timeInSecond.text = ((BuildingTimeLevel)mapData).LevelData.TimeInSecond.ToString();
    }
}
