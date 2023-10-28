using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserDataProvider
{
    private readonly UserData _userData = new();

    public event Action DataChanged;

    public int Money => _userData.Money;
    public Card[] AvailableCards => _userData.AvailableCards.ToArray();
    public Deck[] Decks => _userData.Decks.ToArray();
    public Card[] ActiveDeckCards => _userData.Decks[0].C;

    public void SetUserData(UserData userData)
    {
        _userData.Money = userData.Money;
        _userData.AvailableCards = userData.AvailableCards;
        _userData.Decks = userData.Decks;

        DataChanged?.Invoke();
    }

    public void LoadData()
    {
        if(UserAuthorizedSettings.Instance.IsAuthorized == true)
        {
            WWWConnection.TryGetGameData(
                UserAuthorizedSettings.Instance.Id,
                OnDataGetSucces);
        }
        else
        {
            if (PlayerPrefs.HasKey("Save"))
            {
                LoadDataFromJsom(PlayerPrefs.GetString("Save"));
            }
        }
    }

    private void OnDataGetSucces(string data)
    {
        LoadDataFromJsom(data);
    }

    private void LoadDataFromJsom(string data)
    {
        UserData userData = JsonUtility.FromJson<UserData>(data);
        SetUserData(userData);
    }
}
