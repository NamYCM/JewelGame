using UnityEngine;

[System.Serializable]
public class MapData
{
    public enum LevelState
    {
        Locked,
        Opened,
        AbleToOpen
    }

#if UNITY_EDITOR
    [ReadOnlyAttribute, SerializeField]
#endif
    private int levelNumber;
    public int LevelNumber => levelNumber;

    [SerializeField]
    private BuildingMap map;
    public BuildingMap Map => map;

    [SerializeField]
    private LevelState state;
    public LevelState State => state;

    [SerializeField]
    private int score = 0;
    public int Score => score;

    public MapData (BuildingMap map)
    {
        if (!int.TryParse(map.name, out levelNumber))
            throw new System.InvalidCastException("name of map is not correct");

        levelNumber --;
        this.map = map;
        state = levelNumber != 0 ? LevelState.Locked : LevelState.AbleToOpen;
    }

    public bool IsCorrectLevelNumber ()
    {
        if (map == null) return false;

        return levelNumber == int.Parse(map.name) - 1;
    }

    public void UpdateLevelNumber (int level)
    {
        levelNumber = level;
    }

    public void UpdateMap (BuildingMap map)
    {
        this.map = map;
    }

    public void SetScore (int score)
    {
        if (score < 0) return;

        this.score = score;
    }

    public void SetLevelState (LevelState levelState)
    {
        this.state = levelState;
    }
}