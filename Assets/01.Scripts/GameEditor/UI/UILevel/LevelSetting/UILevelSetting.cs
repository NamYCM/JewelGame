using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UILevelSetting : MonoBehaviour
{
    protected LevelType type;

    public int Star1Score;
    public int Star2Score;
    public int Star3Score;

    public LevelType Type => type;

    public virtual void TurnOnSetting()
    {
        gameObject.SetActive(true);
    }

    public void TurnOffSetting()
    {
        gameObject.SetActive(false);
    }

    public abstract void ResetTargetLevel ();

    public virtual void SetLevelData (BuildingMap mapData)
    {
        Star1Score = mapData.StarScore.Star1Score;
        Star2Score = mapData.StarScore.Star2Score;
        Star3Score = mapData.StarScore.Star3Score;
    }
}
