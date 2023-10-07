using System.Collections;
using System.Collections.Generic;
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
}
