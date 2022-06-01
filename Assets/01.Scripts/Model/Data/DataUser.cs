using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

[Serializable]
public class DataUser
{
    public string token;
    public int currentLevel = 1;
    public string password;
    public string username;
    public Dictionary<int, DataUserLevel> levelDatas;

    public ulong money;
    public Dictionary<PieceType, uint>  specialPiecesAmount;

    //use for json
    public DataUser() { }
    public DataUser(string username, string password)
    {
        this.username = username;
        this.password = password;

        // create level data for all level
        levelDatas = new Dictionary<int, DataUserLevel>();

        //TODO get data from firestore
        // for (int count = 0; count < Data.MaxLevel(); count ++)
        // {
        //    levelDatas.Add(count + 1, new DataUserLevel(count + 1));
        // }

        specialPiecesAmount = new Dictionary<PieceType, uint>();
        specialPiecesAmount.Add(PieceType.COLUMN_CLEAR, 0);
        specialPiecesAmount.Add(PieceType.ROW_CLEAR, 0);
        specialPiecesAmount.Add(PieceType.RAINBOW, 0);
        specialPiecesAmount.Add(PieceType.BOMB, 0);
    }

    //use for update data to server
    public DataUser(string username, string password, int level, DataUserLevel levelData)
    {
        this.username = username;
        this.password = password;

        levelDatas = new Dictionary<int, DataUserLevel>();
        levelDatas.Add(level, levelData);
    }
    public DataUser(string username, string password, int currentLevel)
    {
        this.username = username;
        this.password = password;
        this.currentLevel = currentLevel;
    }

    private DataUserLevel GetLevelData (int level)
    {
        return levelDatas[level];
        // return levelDatas[level - 1];
    }

    public int MapAmount ()
    {
        return levelDatas.Count;
        // return levelDatas.Count;
    }

    public bool IsLock (int level)
    {
        return GetLevelData(level).state == DataUserLevel.LevelState.Locked;
    }
    public bool IsAbleToOpen (int level)
    {
        return GetLevelData(level).state == DataUserLevel.LevelState.AbleToOpen;
    }
    public bool IsOpened (int level)
    {
        return GetLevelData(level).state == DataUserLevel.LevelState.Opened;
    }

    public int GetScore (int level)
    {
        return GetLevelData(level).score;
    }

    public void UpdateScore (int level, int score, Action onSuccessfulUpdate, Action<string> onFailedUpdate)
    {
        var newLevelData = new DataUserLevel(levelDatas[level]);
        newLevelData.score = score;

        var newUserData = new DataUser(username, password, level, newLevelData);

        APIAccessObject.Instance.StartCoroutine(APIAccesser.UpdateUserLevelDataCoroutine(level, newUserData, () => {
            GetLevelData(level).score = score;
            onSuccessfulUpdate?.Invoke();
        }, onFailedUpdate));
    }

    public void ChangeMoney (int changeMoney, Action onSuccessfulUpdate, Action<string> onFailedUpdate)
    {
        APIAccessObject.Instance.StartCoroutine(APIAccesser.ChangeUserMoneyCoroutine(changeMoney, this, (newMoney) => {
            money = newMoney;
            onSuccessfulUpdate?.Invoke();
        }, onFailedUpdate));
    }

    bool IsSpecialItem (PieceType type)
    {
        return type == PieceType.ROW_CLEAR || type == PieceType.COLUMN_CLEAR || type == PieceType.RAINBOW
            || type == PieceType.BOMB;
    }

    public bool CanBuyItem (UIShopItem item)
    {
        return money >= item.Price;
    }

    public void ReduceMoneyInLocal (uint amount)
    {
        if (money < amount) throw new Exception($"money is smaller price");
        money -= amount;
    }

    public void AddItemInLocal (UIShopItem item)
    {
        specialPiecesAmount[item.SpecialType]++;
    }

    public void ReduceItemInLocal (PieceType specialType)
    {
        if (specialPiecesAmount[specialType] == 0) throw new Exception($"amount of {specialType} is 0");
        if (!IsSpecialItem(specialType)) throw new Exception($"{specialType} is not a special piece");

        specialPiecesAmount[specialType]--;
    }

    public void BuyItem (UIShopItem item)
    {
        if (!IsSpecialItem(item.SpecialType)) throw new Exception($"{item.SpecialType} is not a special piece");
        if (!CanBuyItem(item)) throw new Exception("money is enough");

        APIAccessObject.Instance.StartCoroutine(APIAccesser.BuyItem(item.SpecialType, this));
    }

    public void UseItem (PieceType specialType)
    {
        if (specialPiecesAmount[specialType] == 0) throw new Exception($"amount of {specialType} is 0");
        if (!IsSpecialItem(specialType)) throw new Exception($"{specialType} is not a special piece");

        APIAccessObject.Instance.StartCoroutine(APIAccesser.UseItem(specialType, this));
    }

    /// <summary>open level which has able to open state</summary>
    public IEnumerator OpenLevel (int level, Action onSuccessfulOpen, Action<string> onFailedOpen)
    {
        if (GetLevelData(level).state != DataUserLevel.LevelState.AbleToOpen)
        {
            throw new System.InvalidOperationException($"state of level {level} is {GetLevelData(level).state}, can not open");
        }

        var newLevelData = new DataUserLevel(levelDatas[level]);
        newLevelData.state = DataUserLevel.LevelState.Opened;

        var newUserData = new DataUser(username, password, level, newLevelData);

        yield return APIAccesser.UpdateUserLevelDataCoroutine(level, newUserData, () => {
            GetLevelData(level).state = DataUserLevel.LevelState.Opened;
            onSuccessfulOpen?.Invoke();
        }, onFailedOpen);
    }

    /// <summary>unlock from locked level to able to open level</summary>
    public IEnumerator UnlockLevel (int level, Action onSuccessfulUnlock, Action<string> onFailedUnlock)
    {
        if (GetLevelData(level).state != DataUserLevel.LevelState.Locked)
        {
            onSuccessfulUnlock?.Invoke();
            yield break;
        }

        //update to server
        var newLevelData = new DataUserLevel(levelDatas[level]);
        newLevelData.state = DataUserLevel.LevelState.AbleToOpen;

        var newUserData = new DataUser(username, password, level, newLevelData);

        yield return APIAccesser.UpdateUserLevelDataCoroutine(level, newUserData, () => {
            GetLevelData(level).state = DataUserLevel.LevelState.AbleToOpen;
            onSuccessfulUnlock?.Invoke();
        }, onFailedUnlock);
    }

    /// <summary>unlock from locked level to able to open level</summary>
    public IEnumerator UpdateCurrentLevel (int level, Action onSuccessfulUpdate, Action<string> onFailedUpdate)
    {
        var newUserData = new DataUser(username, password, level);
        newUserData.currentLevel = level;

        yield return APIAccesser.UpdateUserCurrentLevelCoroutine(newUserData, () => {
            currentLevel = level;
            onSuccessfulUpdate?.Invoke();
        }, onFailedUpdate);
    }

    public uint GetAmountOf(PieceType specialType)
    {
        if (!specialPiecesAmount.ContainsKey(specialType))
        {
            throw new Exception($"{specialType} is not a special type");
        }

        return specialPiecesAmount[specialType];
    }
}

[Serializable]
public class DataUserLevel
{
    public enum LevelState
    {
        Locked,
        Opened,
        AbleToOpen
    }

    //TODO remove this attribute
    public int levelNumber;
    public LevelState state;
    public int score = 0;

    //for json convert
    public DataUserLevel ()
    {
    }

    public DataUserLevel (int level)
    {
        if (level == 0)
            throw new Exception("level start to 1");
        levelNumber = level;
        state = levelNumber != 1 ? LevelState.Locked : LevelState.AbleToOpen;
        score = 0;
    }

    public DataUserLevel (DataUserLevel data)
    {
        if (data.levelNumber == 0)
            throw new Exception("level start to 1");
        levelNumber = data.levelNumber;
        state = data.state;
        score = data.score;
    }
}

[Serializable]
public class DataAdmin
{
    public string password;
    public string username;
    public string code;

    public DataAdmin () {}

    public DataAdmin (string username, string password, string code)
    {
        this.username = username;
        this.password = password;
        this.code = code;
    }
}