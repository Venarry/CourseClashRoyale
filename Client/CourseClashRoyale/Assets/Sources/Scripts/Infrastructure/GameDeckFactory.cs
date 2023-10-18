using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDeckFactory
{
    private readonly GameDeckView _prefab =
        Resources.Load<GameDeckView>(FilesPath.GameDeckView);
    private readonly CardFactory _cardFactory;

    public GameDeckFactory(CardFactory cardFactory)
    {
        _cardFactory = cardFactory;
    }

    public GameDeckView Create(Card[] cards, int cardsOnTable, Transform canvas)
    {
        GameDeckView gameDeckView = Object.Instantiate(_prefab, canvas);

        GameDeckModel gameDeckModel = new(cardsOnTable);
        GameDeckPresenter gameDeckPresenter = new(gameDeckView, gameDeckModel);

        gameDeckView.Init(_cardFactory, cardsOnTable);
        gameDeckModel.InitCards(cards);

        return gameDeckView;
    }

    internal void Create(Card[] cards, object cardsInTable, Transform transform)
    {
        throw new System.NotImplementedException();
    }
}
