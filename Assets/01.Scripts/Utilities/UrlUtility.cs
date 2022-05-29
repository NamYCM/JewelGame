using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UrlUtility
{
    // private const string ORIGIN_URL = "https://us-central1-testapi-d3e3a.cloudfunctions.net";
    // local url
    private const string ORIGIN_URL = "http://localhost:5001/testapi-d3e3a/us-central1";

    private const string UPDATE_LEVEL_DATA_URL = ORIGIN_URL + "/user/update-level-data";
    public const string SIGN_IN_URL = ORIGIN_URL + "/user/sign-in";
    public const string SIGN_UP_URL = ORIGIN_URL + "/user/sign-up";
    public const string UPDATE_USER_URL = ORIGIN_URL + "/user/update";
    public const string UPDATE_CURRENT_LEVEL_USER_URL =  ORIGIN_URL + "/user/update-current-level";
    const string CHANGE_MONEY_USER_URL =  ORIGIN_URL + "/user/increte-money";
    const string ADD_SPECIAL_ITEM_USER_URL = ORIGIN_URL + "/user/buy-item";
    const string REMOVE_SPECIAL_ITEM_USER_URL = ORIGIN_URL + "/user/use-item";

    public static string SignInUrl (string username, string password) => ORIGIN_URL + $"/user/sign-in/{username}-{password}";
    public static string SignInAdminUrl (string username, string password) => ORIGIN_URL + $"/user/sign-in-admin/{username}-{password}";
    public static string GetUserInforUrl (string username) => ORIGIN_URL + $"/user/{username}";

    public const string LOAD_ALL_MAP_URL = ORIGIN_URL + "/levelMap/update-all-map";
    public const string ADD_MAP_URL = ORIGIN_URL + "/levelMap/add-map";
    public const string UPDATE_MAP_URL = ORIGIN_URL + "/levelMap/update-map";
    public const string GET_ALL_MAP_URL = ORIGIN_URL + "/levelMap/get-all-map";
    public const string GET_CURRENT_VERSION_OF_MAP_URL = ORIGIN_URL + "/levelMap/get-current-version-of-map";

    public static string GetUpdateLevelDataUrl (int level) => UPDATE_LEVEL_DATA_URL + $"/{level}";

    public static string GetChangeMoneyUserUrl (int changeMoney) => CHANGE_MONEY_USER_URL + $"/{changeMoney}";

    public static string GetBuyItemUrl (PieceType specialType) => ADD_SPECIAL_ITEM_USER_URL + $"/{specialType.ToString()}";
    public static string GetUseItemUrl (PieceType specialType) => REMOVE_SPECIAL_ITEM_USER_URL + $"/{specialType.ToString()}";

    public const string ALL_ITEM_URL = ORIGIN_URL + "/shop/all-items";
}
