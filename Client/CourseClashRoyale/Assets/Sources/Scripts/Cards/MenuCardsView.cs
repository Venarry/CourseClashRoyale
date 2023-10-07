using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCardsView : MonoBehaviour
{
    [SerializeField] private Transform _deckParent;
    [SerializeField] private Transform _availableCardsParent;
    [SerializeField] private UserDataProvider _userDataProvider;

    public void ShowCards()
    {
        Card[] cards = _userDataProvider.AvailableCards;


    }
}
