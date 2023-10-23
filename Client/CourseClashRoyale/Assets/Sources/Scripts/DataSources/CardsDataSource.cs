using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsDataSource
{
    private readonly Dictionary<int, Sprite> _cardsIcon;
    private readonly Dictionary<int, int> _cardsPrice;

    public CardsDataSource()
    {
        _cardsIcon = new Dictionary<int, Sprite>();

        _cardsPrice = new Dictionary<int, int>()
        {
            { 1, 1 },
            { 2, 2 },
            { 3, 3 },
            { 4, 4 },
            { 5, 5 },
            { 6, 6 },
            { 7, 7 },
            { 8, 8 },
            { 9, 9 },
            { 10, 10 },
        };
    }

    public int GetPrice(int id)
    {
        return _cardsPrice[id];
    }
}
