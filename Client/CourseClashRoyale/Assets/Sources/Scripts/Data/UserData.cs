using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int Money;
    public Card[] AvailableCards =
        new Card[] 
        { 
            new Card(1), new Card(2), new Card(3), new Card(4), new Card(5), new Card(6), new Card(7), new Card(8), new Card(9), new Card(10) 
        };
    public Deck[] Decks = new Deck[] { new Deck(1, 2, 3, 4, 5, 6, 7, 8, 9, 10) };
}

public class Card
{
    public int Id;
    public int P;
    public int L;

    public Card(int id, int progress = 0, int level = 0)
    {
        Id = id;
        L = level;
    }
}

public class Deck
{
    public int[] C;

    public Deck(params int[] cards)
    {
        C = cards;
    }
}
