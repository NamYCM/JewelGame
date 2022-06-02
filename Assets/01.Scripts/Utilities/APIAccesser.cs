using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_WEBGL
using Newtonsoft.Json.Utilities;
#endif

public static class APIAccesser
{
    static APIAccesser ()
    {
#if UNITY_WEBGL
        AotHelper.EnsureList<GridPlay.PiecePosition>();
        AotHelper.EnsureList<BuildingMap.ConnectColumn>();
        AotHelper.EnsureList<BuildingMap.ConnectData>();
        AotHelper.EnsureList<BuildingMap.GridData>();
#endif
    }

#region  LoginAPIs
    /// <summary>Sign in</summary>
    public static IEnumerator LoginCoroutine(DataUser user, Action<DataUser> onSuccessfulSignIn, Action<string> onFailedSignIn)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(UrlUtility.SignInUrl(user.username, user.password)))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    DataUser userData = JsonConvert.DeserializeObject<DataUser>(body);
                    onSuccessfulSignIn?.Invoke(userData);
                }
                else
                {
                    Debug.LogWarning("Login failed!!\n" + body);
                    onFailedSignIn?.Invoke(body);
                }

            }
        }
    }

    public static IEnumerator LoginAdminCoroutine(string username, string password, Action<DataUser> onSuccessfulSignIn, Action<string> onFailedSignIn)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(UrlUtility.SignInAdminUrl(username, password)))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    DataUser userData = JsonConvert.DeserializeObject<DataUser>(body);
                    onSuccessfulSignIn?.Invoke(userData);
                }
                else
                {
                    Debug.LogWarning("Login failed!!\n" + body);
                    onFailedSignIn?.Invoke(body);
                }
            }
        }
    }

    /// <summary>sign up</summary>
    public static IEnumerator SignUpCoroutine(DataUser user, Action onSuccessfulSignUp, Action<string> onFailedSignUp)
    {
        string bodyJsonString = JsonConvert.SerializeObject(user);;
        string url = UrlUtility.SIGN_UP_URL;

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            onSuccessfulSignUp?.Invoke();
        }
        else
        {
            string message = request.downloadHandler.text;
            onFailedSignUp?.Invoke(message);
        }
    }

    public static IEnumerator LoadAllMapCoroutine (string bodyJsonString)
    {
        if (bodyJsonString == null) yield break;

        Debug.Log(bodyJsonString);

        string url = UrlUtility.LOAD_ALL_MAP_URL;

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            Debug.Log("Load succesful");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    public static IEnumerator ResetPasswordAdmin (string email, Action onSucessfull, Action<string> onFailed)
    {
        var request = new UnityWebRequest(UrlUtility.ResetPasswordAdminUrl(email), "PUT");
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            onSucessfull?.Invoke();
        }
        else
        {
            string message = request.downloadHandler.text;
            onFailed?.Invoke(message);
        }
    }
#endregion

#region UserAPIs
    public static IEnumerator GetUserCoroutine(DataUser user, Action<DataUser> onSuccessfulGet, Action<string> onFailedGet)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(UrlUtility.GetUserInforUrl(user.username)))
        {
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    DataUser userData = JsonConvert.DeserializeObject<DataUser>(body);
                    onSuccessfulGet?.Invoke(userData);
                }
                else
                {
                    Debug.LogWarning("Get user infor failed!!\n" + body);
                    onFailedGet?.Invoke(body);
                }

            }
        }
    }

    public static IEnumerator UpdateUserLevelDataCoroutine (int level, DataUser data, Action onSucessfulUpdate, Action<string> onFailedUpdate)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(UrlUtility.GetUpdateLevelDataUrl(level), JsonConvert.SerializeObject(data)))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;
                if (www.responseCode == 200)
                {
                    onSucessfulUpdate?.Invoke();
                }
                else
                {
                    onFailedUpdate?.Invoke(body);
                }
            }
        }
    }

    public static IEnumerator UpdateUserCurrentLevelCoroutine (DataUser data, Action onSucessfulUpdate, Action<string> onFailedUpdate)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(UrlUtility.UPDATE_CURRENT_LEVEL_USER_URL, JsonConvert.SerializeObject(data)))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    onSucessfulUpdate?.Invoke();
                }
                else
                {
                    onFailedUpdate?.Invoke(body);
                }
            }
        }
    }

    public static IEnumerator ChangeUserMoneyCoroutine (int changeMoney, DataUser data, Action<ulong> onSucessfulUpdate, Action<string> onFailedUpdate)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(UrlUtility.GetChangeMoneyUserUrl(changeMoney), JsonConvert.SerializeObject(data)))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    Debug.Log(body);
                    onSucessfulUpdate?.Invoke(ulong.Parse(body));
                }
                else
                {
                    onFailedUpdate?.Invoke(body);
                }
            }
        }
    }

    public static IEnumerator BuyItem (PieceType type, DataUser data)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(UrlUtility.GetBuyItemUrl(type), JsonConvert.SerializeObject(data)))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                if (www.responseCode != 200)
                {
                    throw new Exception("something wrong in add special item \n" + www.error);
                }
            }
        }
    }
    public static IEnumerator UseItem (PieceType type, DataUser data)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(UrlUtility.GetUseItemUrl(type), JsonConvert.SerializeObject(data)))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                if (www.responseCode != 200)
                {
                    throw new Exception("something wrong in add special item \n" + www.error);
                }
            }
        }
    }
#endregion

#region MapAPIs
    public static IEnumerator AddMapCoroutine (MapLevelData levelData, Action onSucessfulAdd, Action<string> onFailedAdd)
    {
        string bodyJsonString = JsonConvert.SerializeObject(levelData, Formatting.None, new JsonSerializerSettings()
        {
            //inside the quaternion cause Self referencing loop
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        string url = UrlUtility.ADD_MAP_URL;

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", Data.GetBearer());

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            Debug.Log("Add succesful");
            onSucessfulAdd?.Invoke();
        }
        else
        {
            string body = request.downloadHandler.text;
            Debug.Log(body);
            onFailedAdd?.Invoke(body);
        }
    }

    public static IEnumerator UpdateMapCoroutine (KeyValuePair<int, MapLevelData> levelData, Action onSucessfulAdd, Action<string> onFailedAdd)
    {
        //because the KeyValuePair is not correct format when serialize to json
        Dictionary<int, MapLevelData> dic = new Dictionary<int, MapLevelData>();
        dic.Add(levelData.Key, levelData.Value);

        string bodyJsonString = JsonConvert.SerializeObject(dic, Formatting.None, new JsonSerializerSettings()
        {
            //inside the quaternion cause Self referencing loop
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        using (UnityWebRequest www = UnityWebRequest.Put(UrlUtility.UPDATE_MAP_URL, bodyJsonString))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    Debug.Log("update succesful");
                    onSucessfulAdd?.Invoke();
                }
                else
                {
                    Debug.LogWarning("Update map failed!!\n" + body);
                    onFailedAdd?.Invoke(body);
                }
            }
        }
    }

    public static IEnumerator GetAllMapCoroutine (Action<LevelData> onSucessfulGet, Action<string> onFailedGet)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(UrlUtility.GET_ALL_MAP_URL))
        {
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.LogError("something wrong in send www request \n" + www.error);
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    LevelData levelData = JsonConvert.DeserializeObject<LevelData>(body);
                    Debug.Log("get all map succesful");
                    onSucessfulGet?.Invoke(levelData);
                }
                else
                {
                    Debug.LogWarning("Update level data of user failed!!\n" + body);
                    onFailedGet?.Invoke(body);
                }

            }
        }
    }

    public static IEnumerator GetCurrentVersionOfMapCoroutine (Action<LevelData> onSucessfulGet, Action<string> onFailedGet)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(UrlUtility.GET_CURRENT_VERSION_OF_MAP_URL))
        {
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    LevelData levelData = JsonConvert.DeserializeObject<LevelData>(body);
                    onSucessfulGet?.Invoke(levelData);
                }
                else
                {
                    Debug.LogWarning("get level data  failed!!\n" + body);
                    onFailedGet?.Invoke(body);
                }
            }
        }
    }
#endregion

#region ShopAPIs
    public static IEnumerator LoadAllItemsCoroutine (DataShop dataShop)
    {
        string bodyJsonString = JsonConvert.SerializeObject(dataShop);

        Debug.Log(bodyJsonString);

        string url = UrlUtility.ALL_ITEM_URL;

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            Debug.Log("Load succesful");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
    }

    public static IEnumerator GetAllItemsCoroutine (Action<DataShop> onSucessfulGet, Action<string> onFailedGet)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(UrlUtility.ALL_ITEM_URL))
        {
            www.SetRequestHeader("Authorization", Data.GetBearer());

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                throw new Exception("something wrong in send www request \n" + www.error);
            }
            else
            {
                string body = www.downloadHandler.text;

                if (www.responseCode == 200)
                {
                    var levelData = JsonConvert.DeserializeObject<DataShop>(body);
                    onSucessfulGet?.Invoke(levelData);
                }
                else
                {
                    Debug.LogWarning("Get items of shop failed!!\n" + body);
                    onFailedGet?.Invoke(body);
                }

            }
        }
    }
#endregion

#region AdminAPIs
    public static IEnumerator SendVerifyGmail (Action onSucessfull, Action<string> onFailed)
    {
        var request = new UnityWebRequest(UrlUtility.SEND_VERIFY_GMAIL_URL, "PUT");
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", Data.GetBearer());

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            onSucessfull?.Invoke();
        }
        else
        {
            string message = request.downloadHandler.text;
            onFailed?.Invoke(message);
        }
    }

    public static IEnumerator SignUpAdminCoroutine (DataAdmin admin, Action onSucessfull, Action<string> onFailed)
    {
        string bodyJsonString = JsonConvert.SerializeObject(admin);

        string url = UrlUtility.SIGN_UP_ADMIN_URL;

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", Data.GetBearer());

        yield return request.SendWebRequest();

        if (request.responseCode == 200)
        {
            onSucessfull?.Invoke();
        }
        else
        {
            string body = request.downloadHandler.text;
            onFailed?.Invoke(body);
        }
    }
#endregion
}