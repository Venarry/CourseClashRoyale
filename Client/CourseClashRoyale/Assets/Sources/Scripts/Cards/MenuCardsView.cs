using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MenuCardsView : MonoBehaviour
{
    private const int DeckSize = 8;

    [SerializeField] private Transform _deckParent;
    [SerializeField] private Transform _availableCardsParent;

    private UserDataProvider _userDataProvider;
    private CardFactory _cardFactory;
    private int _decksCount = 1;
    private int _currentDeckIndex = 0;
    private bool _isInitialized;

    public void Init(CardFactory cardFactory, UserDataProvider userDataProvider)
    {
        gameObject.SetActive(false);

        _cardFactory = cardFactory;
        _userDataProvider = userDataProvider;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _userDataProvider.DataChanged += RefreshCards;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _userDataProvider.DataChanged -= RefreshCards;
    }

    public void RefreshCards()
    {
        DestroyCards();

        Card[] cards = _userDataProvider.AvailableCards;
        Deck[] decks = _userDataProvider.Decks;

        int deckCounter = 0;

        foreach (Card card in cards)
        {
            if (decks[_currentDeckIndex].Contains(card.Id) && deckCounter < DeckSize)
            {
                _cardFactory.CreateMenuCard(card.Id, _deckParent);
                deckCounter++;
            }
            else
            {
                Debug.Log(card.Id);
                _cardFactory.CreateMenuCard(card.Id, _availableCardsParent);
            }
        }
    }

    public void DestroyCards()
    {

    }
}
