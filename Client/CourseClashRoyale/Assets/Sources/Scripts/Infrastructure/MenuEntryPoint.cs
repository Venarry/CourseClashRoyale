using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MenuCardsView _cardsView;

    private void Awake()
    {
        CardsDataSource cardsDataSource = new();
        CardFactory cardFactory = new(cardsDataSource);

        _cardsView.Init(cardFactory);
    }
}
