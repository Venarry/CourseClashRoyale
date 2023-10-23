using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameDeckModel
{
    private List<Card> _cards;
    private readonly int _cardsOnTable;

    public GameDeckModel(int cardsOnTable)
    {
        _cardsOnTable = cardsOnTable;
    }

    public event Action<Card[]> CardsInited;
    public event Action<Card> CardShifted;
    public event Action<Card> CardRemoved;

    public void InitCards(Card[] cards)
    {
        _cards = cards.ToList();
        Shuffle(_cards);

        CardsInited?.Invoke(_cards.ToArray());
    }

    public void RespawnCard(Card card)
    {
        _cards.Remove(card);
        CardRemoved?.Invoke(card);

        int cardPosition = UnityEngine.Random.Range(_cardsOnTable, _cards.Count);
        _cards.Insert(cardPosition, card);

        CardShifted?.Invoke(_cards.ElementAt(_cardsOnTable - 1));
    }

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
