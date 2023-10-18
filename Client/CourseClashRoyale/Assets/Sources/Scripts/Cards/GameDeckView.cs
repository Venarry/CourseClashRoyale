using System.Collections.Generic;
using UnityEngine;

public class GameDeckView : MonoBehaviour
{
    [SerializeField] private Transform _cardParent;

    private CardFactory _cardFactory;
    private readonly Dictionary<Card, CardView> _cards = new();

    public void Init(CardFactory cardFactory)
    {
        _cardFactory = cardFactory;
    }

    public void OnCardsInit(Card[] cards)
    {
        foreach (Card card in cards)
        {
            CreateCard(card);
        }
    }

    public void OnCardAdd(Card card)
    {
        if (_cards.ContainsKey(card))
            return;

        CreateCard(card);
    }

    public void OnCardRemove(Card card)
    {
        if (_cards.ContainsKey(card) == false)
            return;

        _cards.Remove(card);
    }

    private void CreateCard(Card card)
    {
        CardView cardView = _cardFactory.CreateMenuCard(card.Id, _cardParent);
        _cards.Add(card, cardView);
    }
}
