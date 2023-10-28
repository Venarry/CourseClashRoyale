using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WWWConnection : MonoBehaviour
{
    private static WWWConnection _instance;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void Post(string uri,
        Dictionary<string, string> data,
        Action<string> success = null,
        Action<string> error = null)
    {
        _instance.StartCoroutine(MakePostRequest(uri, data, success, error));
    }

    public static void TryGetGameData(int userId, Action<string> callBack)
    {
        Dictionary<string, string> data = new()
            {
                { "tableName", "clashroyaledata" },
                { "id", userId.ToString() },
            };

        WWWConnection.Post(
            URLProvider.DataProviderFile,
            data,
            callBack);
    }

    private static IEnumerator MakePostRequest(
        string uri,
        Dictionary<string, string> data,
        Action<string> success = null,
        Action<string> error = null)
    {
        using(UnityWebRequest request = UnityWebRequest.Post(uri, data))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.Success)
            {
                success?.Invoke(request.downloadHandler.text);
            }
            else
            {
                error?.Invoke(request.error);
            }
        }
    }
}
