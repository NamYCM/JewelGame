using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelEditor : MonoBehaviour
{
    [SerializeField] InputField score1Star;
    [SerializeField] InputField score2Star;
    [SerializeField] InputField score3Star;

    [SerializeField] Dropdown levelType;

    UILevelSetting levelSetting;

    public UILevelSetting LevelSetting => levelSetting;

    private void Awake() {
        levelType.onValueChanged.AddListener((int i) => OnLevelTypeDropdownChange(i));

        //set level type
        SwitchLevelType(levelType.options[levelType.value].text);

        //set default value for scores
        score1Star.text = levelSetting.Star1Score.ToString();
        score2Star.text = levelSetting.Star2Score.ToString();
        score3Star.text = levelSetting.Star3Score.ToString();

        //add event change value of scores
        score1Star.onValueChanged.AddListener(s => OnStar1ScoreChange(s));
        score2Star.onValueChanged.AddListener(s => OnStar2ScoreChange(s));
        score3Star.onValueChanged.AddListener(s => OnStar3ScoreChange(s));
    }

    private void SwitchLevelType (string levelType)
    {
        if (levelSetting)
            levelSetting.TurnOffSetting();

        switch (levelType)
        {
            case "Move Level":
                levelSetting = GetComponentInChildren<UILevelMoveSetting>(true);
                break;
            case "Timer Level":
                levelSetting = GetComponentInChildren<UILevelTimerSetting>(true);
                break;
            default:
                break;
        }

        //reload score star data
        score1Star.text = levelSetting.Star1Score.ToString();
        score2Star.text = levelSetting.Star1Score.ToString();
        score3Star.text = levelSetting.Star3Score.ToString();

        //reload specify level data
        levelSetting.TurnOnSetting();
    }
    private void SwitchLevelType (LevelType levelType)
    {
        if (levelSetting)
            levelSetting.TurnOffSetting();

        switch (levelType)
        {
            case LevelType.MOVES:
                levelSetting = GetComponentInChildren<UILevelMoveSetting>(true);
                break;
            case LevelType.TIMER:
                levelSetting = GetComponentInChildren<UILevelTimerSetting>(true);
                break;
            default:
                throw new System.InvalidCastException($"{levelType} is not supported");
        }

        //reload score star data
        score1Star.text = levelSetting.Star1Score.ToString();
        score2Star.text = levelSetting.Star1Score.ToString();
        score3Star.text = levelSetting.Star3Score.ToString();

        //reload specify level data
        levelSetting.TurnOnSetting();
    }

    private void OnLevelTypeDropdownChange (int value)
    {
        SwitchLevelType(levelType.options[value].text);
    }

    private void OnStar1ScoreChange(string value)
    {
        levelSetting.Star1Score = int.Parse(value);
    }

    private void OnStar2ScoreChange(string value)
    {
        levelSetting.Star2Score = int.Parse(value);
    }

    private void OnStar3ScoreChange(string value)
    {
        levelSetting.Star3Score = int.Parse(value);
    }

    public void ResetLevelInfor ()
    {
        score1Star.text = "0";
        score2Star.text = "0";
        score3Star.text = "0";

        levelSetting.ResetTargetLevel();
    }

    private void ReloadUI ()
    {
        score1Star.text = levelSetting.Star1Score.ToString();
        score2Star.text = levelSetting.Star2Score.ToString();
        score3Star.text = levelSetting.Star3Score.ToString();
    }

    private void SetLevelTypeUI (LevelType newType)
    {
        int newValue = 0;

        switch (newType)
        {
            case LevelType.MOVES:
                for (var count = 0; count < levelType.options.Count; count++)
                {
                    if (levelType.options[count].text == "Move Level")
                    {
                        newValue = count;
                        break;
                    }
                }
                break;
            case LevelType.TIMER:
                for (var count = 0; count < levelType.options.Count; count++)
                {
                    if (levelType.options[count].text == "Timer Level")
                    {
                        newValue = count;
                        break;
                    }
                }
                break;
            default:
                throw new System.InvalidCastException($"{newType} is not supported");
        }

        levelType.value = newValue;
    }

    public void SetLevel (BuildingMap mapData)
    {
        SwitchLevelType(mapData.StarScore.levelType);
        levelSetting.SetLevelData(mapData);

        ReloadUI();
    }
}
