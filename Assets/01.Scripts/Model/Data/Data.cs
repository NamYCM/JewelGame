using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    // public static DataObject data;
    private static DataUser user;
    private static LevelData level;
    private static DataShop shop;

    static Data ()
    {
        //load data
        // if (data == null) LoadDataObject();

        //to load select scene without login
        // if (user == null)
        // {
        //     Debug.Log("1");
        //     user = new DataUser("username", "password");
        //     APIAccesser.LoginCoroutine(user, null, () => {
        //         APIAccesser.SignUpCoroutine(user);
        //         });
        // }
        // if (user == null) user = new DataUser("username", "password");
    }

    // private static void CreateDataObject ()
    // {
    //     data = ScriptableObject.CreateInstance<DataObject>();
    //     data.InitDataObject();
    // }

    // private static void LoadDataObject ()
    // {
    //     data = Resources.Load<DataObject>(FileUtility.GetDataPath());

    //     if (data == null)
    //     {
    //         //does not exists data in project
    //         //because the data object was deleted
    //         CreateDataObject();
    //     }
    // }

    public static IEnumerator AddLevel (BuildingMap map, Action onSuccessfulGet, Action<string> onFailedGet)
    {
        // data.AddLevel(map);
        yield return APIAccesser.AddMapCoroutine(new MapLevelData(map), onSuccessfulGet, onFailedGet);
    }

    public static IEnumerator UpdateLevel (KeyValuePair<int, BuildingMap> map, Action onSuccessful, Action<string> onFailed)
    {
        // data.AddLevel(map.Value);
        yield return APIAccesser.UpdateMapCoroutine(new KeyValuePair<int, MapLevelData>(map.Key, new MapLevelData(map.Value)), onSuccessful, onFailed);
    }

    public static int MaxLevel ()
    {
        // return user.MapAmount();
        // return data.MapAmount();
        return level.maps.Count;
    }

    public static bool IsLock (int level)
    {
        return user.IsLock(level);
        // return data.IsLock(level);
    }
    public static bool IsAbleToOpen (int level)
    {
        return user.IsAbleToOpen(level);
        // return data.IsAbleToOpen(level);
    }
    public static bool IsOpened (int level)
    {
        return user.IsOpened(level);
        // return data.IsOpened(level);
    }

    public static BuildingMap GetMapInfor (int levelNumber)
    {
        return level.maps[levelNumber].ConvertToBuildingMap();
        // return data.GetMapInfor(levelNumber);
    }

    // public static IEnumerator UpdateScore (int level, int score, Action onSuccessfulUpdate, Action<string> onFailedUpdate)
    // {
    //     if (user.IsLock(level))
    //         throw new System.NotSupportedException($"level {level} is locked, can not update score");

    //     if (user.GetScore(level) > score)
    //     {
    //         onSuccessfulUpdate?.Invoke();
    //         yield break;
    //     }

    //     yield return user.UpdateScore(level, score, onSuccessfulUpdate, onFailedUpdate);
    // }
    public static void UpdateScore (int level, int score, Action onSuccessfulUpdate, Action<string> onFailedUpdate)
    {
        if (user.IsLock(level))
            throw new System.NotSupportedException($"level {level} is locked, can not update score");

        if (user.GetScore(level) > score)
        {
            onSuccessfulUpdate?.Invoke();
            return;
            // yield break;
        }

        user.UpdateScore(level, score, onSuccessfulUpdate, onFailedUpdate);
    }

    public static void ChangeUserMoney (int changeMoney, Action onSuccessfulUpdate, Action<string> onFailedUpdate)
    {
        user.ChangeMoney(changeMoney, onSuccessfulUpdate, onFailedUpdate);
    }

    public static int GetScore (int level)
    {
        return user.GetScore(level);
        // return data.GetScore(level);
    }

    public static IEnumerator UpdateCurrentLevel (int level, Action onSuccessfulUpdate, Action<string> onFailedUpdate)
    {
        if (level < 0 || level > MaxLevel()) throw new System.IndexOutOfRangeException($"level {level} is invalute");
        yield return user.UpdateCurrentLevel(level, onSuccessfulUpdate, onFailedUpdate);
    }

    public static int GetCurrentLevel ()
    {
        return user.currentLevel;
        // return data.CurrentLevel;
    }

    public static int GetNextLevel ()
    {
        int currentLevel = Data.GetCurrentLevel();

        if (Data.IsMaxLevel(currentLevel))
            throw new IndexOutOfRangeException($"level {currentLevel} is max level");
        else
        {
            return currentLevel + 1;
        }
    }

    public static bool IsMaxLevel (int level)
    {
        return level == Data.MaxLevel();
    }

    public static IEnumerator OpenLevel (int level, Action onSuccessfulOpen, Action<string> onFailedOpen)
    {
        yield return user.OpenLevel(level, onSuccessfulOpen, onFailedOpen);
    }

    public static IEnumerator UnlockLevel (int level, Action onSuccessfulUnlock, Action<string> onFailedUnlock)
    {
        yield return user.UnlockLevel(level, onSuccessfulUnlock, onFailedUnlock);
    }

    public static ulong GetMoneyOfUser()
    {
        return user.money;
    }

    public static uint GetAmountOfSpecialPiece (PieceType specialType)
    {
        return user.GetAmountOf(specialType);
    }

    public static void BuyItemForUser (UIShopItem item)
    {
        user.BuyItem(item);
    }
    public static void UseItemOfUser (PieceType specialType)
    {
        user.UseItem(specialType);
    }

    public static void ReduceMoneyInLocal (uint amount)
    {
        user.ReduceMoneyInLocal(amount);
    }

    public static void AddItemInLocal (UIShopItem item)
    {
        user.AddItemInLocal(item);
    }

    public static void ReduceItemInLocal (PieceType specialType)
    {
        user.ReduceItemInLocal(specialType);
    }

    public static void ReloadUserDataFromFirestore ()
    {
        if (user == null) return;

        APIAccessObject.Instance.StartCoroutine(APIAccesser.GetUserCoroutine(user, (userData) => {
            Data.InitUserData(userData);
        }, (message) => {
            Debug.Log(user.username + " " + user.password);
            throw new Exception("error in reload user data \n" + message);
        }));
    }

    //shop data
    public static Dictionary<PieceType, DataShop.ShopItem> GetItemsInShop ()
    {
        return shop.shopItems;
    }

    public static void InitUserData (DataUser dataUser)
    {
        user = dataUser;
    }

    // public static IEnumerator InitLevelData (LevelData levelData, Action<LevelData> onSuccessfulInit, Action<string> onFailedInit)
    public static void InitLevelData (Action onSuccessfulInit, Action<string> onFailedInit)
    {
        // yield return levelData.LoadData((data) => {
        //     level = data;
        //     onSuccessfulInit?.Invoke(data);
        // }, onFailedInit);
        APIAccessObject.Instance.StartCoroutine(new LevelData().LoadData((data) => {
            level = data;
            onSuccessfulInit?.Invoke();
        }, onFailedInit));
        // yield return new LevelData().LoadData((data) => {
        //     level = data;
        //     onSuccessfulInit?.Invoke();
        // }, onFailedInit);
    }

    public static void InitLevelDataFromFirebase (Action onSuccessfulInit, Action<string> onFailedInit)
    {
        // yield return levelData.LoadData((data) => {
        //     level = data;
        //     onSuccessfulInit?.Invoke(data);
        // }, onFailedInit);
        APIAccessObject.Instance.StartCoroutine(new LevelData().LoadDataFromFirebase((data) => {
            level = data;
            onSuccessfulInit?.Invoke();
        }, onFailedInit));
        // yield return new LevelData().LoadData((data) => {
        //     level = data;
        //     onSuccessfulInit?.Invoke();
        // }, onFailedInit);
    }

    public static void InitShopData (DataShop dataShop)
    {
        shop = dataShop;
    }
}
