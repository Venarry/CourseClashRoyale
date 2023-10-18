using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDeckPresenter
{
    private readonly GameDeckView _view;
    private readonly GameDeckModel _model;

    public GameDeckPresenter(GameDeckView gameDeckView, GameDeckModel model)
    {
        _view = gameDeckView;
        _model = model;

        _model.CardsInited += _view.OnCardsInit;
        _model.CardAdded += _view.OnCardAdd;
        _model.CardRemoved += _view.OnCardRemove;
    }

    ~ GameDeckPresenter()
    {
        _model.CardsInited -= _view.OnCardsInit;
        _model.CardAdded -= _view.OnCardAdd;
        _model.CardRemoved -= _view.OnCardRemove;
    }
}
