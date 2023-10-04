using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WWWConnection : MonoBehaviour
{
    public static WWWConnection Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Post(string uri,
        Dictionary<string, string> data,
        Action<string> success = null,
        Action<string> error = null) =>
        StartCoroutine(MakePostRequest(uri, data, success, error));

    private IEnumerator MakePostRequest(
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
