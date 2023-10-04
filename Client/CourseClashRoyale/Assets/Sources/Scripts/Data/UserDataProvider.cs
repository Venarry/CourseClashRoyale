using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataProvider : MonoBehaviour
{
    public static UserData UserData;
    private static UserDataProvider _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
