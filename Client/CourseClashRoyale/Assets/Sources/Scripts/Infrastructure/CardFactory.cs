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

    public CardView CreateMenuCard(Card card, Transform parent)
    {
        CardView cardView = CreateBaseCard(card, parent);

        return cardView;
    }

    public CardView CreateGameCard(
        Card card,
        GameDeckView gameDeckView,
        Transform parent)
    {
        CardView cardView = CreateBaseCard(card, parent);
        cardView.AddComponent<GameCardHandler>().Init(card, gameDeckView);

        return cardView;
    }

    private CardView CreateBaseCard(
        Card card,
        Transform parent)
    {
        CardView cardView = Object.Instantiate(_prefab, parent);
        cardView.SetSpawnPrice(_cardsDataSource.GetPrice(card.Id));
        //cardView.SetMainImage(_cardsDataSource.GetIcon(card.Id));
        cardView.SetLevel(card.L);

        return cardView;
    }
}
