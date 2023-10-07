using UnityEngine;
using System;

public class UserInfo : MonoBehaviour
{
    public static UserInfo Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int Id { get; private set; }

    public void Set(int id)
    {
        if(id < 0)
            throw new IndexOutOfRangeException();

        Id = id;
    }
}
