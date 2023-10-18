using System.Collections.Generic;
using UnityEngine;

public class GameDeckView : MonoBehaviour
{
    [SerializeField] private Transform _cardParent;

    private CardFactory _cardFactory;
    private int _cardsOnTable;
    private readonly Dictionary<Card, CardView> _cards = new();

    public void Init(CardFactory cardFactory, int cardsOnTable)
    {
        _cardFactory = cardFactory;
        _cardsOnTable = cardsOnTable;
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

        CardView cardView = _cardFactory.CreateMenuCard(card.Id, _cardParent);
        _cards.Add(card, cardView);
    }
}
