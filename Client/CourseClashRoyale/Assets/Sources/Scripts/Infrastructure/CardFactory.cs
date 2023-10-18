using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardFactory
{
    private readonly CardView _prefab = Resources.Load<CardView>(FilesPath.CardView);
    private readonly CardsDataSource _cardsDataSource;

    public CardFactory(CardsDataSource cardsDataSource)
    {
        _cardsDataSource = cardsDataSource;
    }

    public CardView CreateMenuCard(int id, Transform parent)
    {
        CardView cardView = Object.Instantiate(_prefab, parent);
        cardView.SetSpawnPrice(_cardsDataSource.GetPrice(id));

        return cardView;
    }

    public CardView CreateGameCard(
        Card card,
        GameDeckView gameDeckView,
        Transform parent)
    {
        CardView cardView = Object.Instantiate(_prefab, parent);
        cardView.SetSpawnPrice(_cardsDataSource.GetPrice(card.Id));
        cardView.AddComponent<GameCardHandler>().Init(card, gameDeckView);

        return cardView;
    }
}
