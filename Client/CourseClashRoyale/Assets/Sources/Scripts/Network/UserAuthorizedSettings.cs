using UnityEngine;
using System;

public class UserAuthorizedSettings
{
    public static UserAuthorizedSettings Instance 
    { 
        get 
        {
            if(_instance == null)
            {
                _instance = new UserAuthorizedSettings();
            }

            return _instance; 
        }
    }

    private static UserAuthorizedSettings _instance;

    /*private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }*/

    public int Id { get; private set; }
    public bool IsAuthorized { get; private set; }

    public void SetAuthorizedSettings(int id)
    {
        if(id < 0)
            throw new IndexOutOfRangeException();

        Id = id;
        IsAuthorized = true;
    }
}
