using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDeckPresenter
{
    private readonly GameDeckView _view;
    private readonly GameDeckModel _gameDeckModel;
    private readonly ManaModel _manaModel;

    public GameDeckPresenter(GameDeckView gameDeckView, GameDeckModel model, ManaModel manaModel)
    {
        _view = gameDeckView;
        _gameDeckModel = model;
        _manaModel = manaModel;

        _gameDeckModel.CardsInited += _view.OnCardsInit;
        _gameDeckModel.CardAdded += _view.OnCardAdd;
        _gameDeckModel.CardRemoved += _view.OnCardRemove;
    }

    ~ GameDeckPresenter()
    {
        _gameDeckModel.CardsInited -= _view.OnCardsInit;
        _gameDeckModel.CardAdded -= _view.OnCardAdd;
        _gameDeckModel.CardRemoved -= _view.OnCardRemove;
    }

    public bool TryReduceMana(int value) =>
        _manaModel.TryReduce(value);
}
