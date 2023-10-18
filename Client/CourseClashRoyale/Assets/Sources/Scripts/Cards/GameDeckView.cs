using System.Collections.Generic;
using UnityEngine;
using System;

public class GameDeckView : MonoBehaviour
{
    [SerializeField] private Transform _cardParent;

    private CardFactory _cardFactory;
    private Card _activeCard;
    private int _cardsOnTable;
    private readonly Dictionary<Card, CardView> _cards = new();

    public void Init(CardFactory cardFactory, int cardsOnTable)
    {
        _cardFactory = cardFactory;
        _cardsOnTable = cardsOnTable;
    }

    public void SetActiveCard(Card card)
    {
        if(_cards.ContainsKey(card) == false)
            throw new ArgumentNullException(nameof(card));

        if(_activeCard != null)
        {
            _cards[_activeCard].gameObject.transform.localScale = Vector3.one;
        }

        _activeCard = card;

        float selectedSize = 1.2f;
        _cards[_activeCard].gameObject.transform.localScale =
            new(selectedSize, selectedSize, selectedSize);
    }

    public void OnCardsInit(Card[] cards)
    {
        foreach (Card card in cards)
        {
            TryCreateCard(card);
        }
    }

    public void OnCardAdd(Card card)
    {
        if (_cards.ContainsKey(card))
            return;

        TryCreateCard(card);
    }

    public void OnCardRemove(Card card)
    {
        if (_cards.ContainsKey(card) == false)
            return;

        _cards.Remove(card);
    }

    private void TryCreateCard(Card card)
    {
        if (_cards.Count >= _cardsOnTable)
            return;

        CardView cardView = _cardFactory.CreateGameCard(card, this, _cardParent);
        _cards.Add(card, cardView);
    }
}
