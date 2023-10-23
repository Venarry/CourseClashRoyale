using UnityEngine;

public class GameDeckFactory
{
    private readonly GameDeckView _prefab =
        Resources.Load<GameDeckView>(FilesPath.GameDeckView);
    private readonly CardFactory _cardFactory;
    private readonly UnitsFactory _unitsFactory;

    public GameDeckFactory(CardFactory cardFactory, UnitsFactory unitsFactory)
    {
        _cardFactory = cardFactory;
        _unitsFactory = unitsFactory;
    }

    public GameDeckView Create(
        Card[] cards,
        int cardsOnTable,
        ManaModel manaModel,
        Transform canvas)
    {
        GameDeckView gameDeckView = Object.Instantiate(_prefab, canvas);

        GameDeckModel gameDeckModel = new(cardsOnTable);
        GameDeckPresenter gameDeckPresenter = new(
            gameDeckView,
            gameDeckModel,
            manaModel);

        gameDeckView.Init(gameDeckPresenter, _cardFactory, _unitsFactory, cardsOnTable);
        gameDeckModel.InitCards(cards);

        return gameDeckView;
    }
}
