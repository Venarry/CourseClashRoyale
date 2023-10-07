using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataProvider : MonoBehaviour
{
    private static UserDataProvider _instance;
    private readonly UserData _userData = new();

    public event Action DataChanged;

    public int Money => _userData.Money;
    public Card[] AvailableCards => _userData.AvailableCards.ToArray();
    public Deck[] Decks => _userData.Decks.ToArray();

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

    public void SetUserData(UserData userData)
    {
        _userData.Money = userData.Money;
        _userData.AvailableCards = userData.AvailableCards;
        _userData.Decks = userData.Decks;

        DataChanged?.Invoke();
    }
}
