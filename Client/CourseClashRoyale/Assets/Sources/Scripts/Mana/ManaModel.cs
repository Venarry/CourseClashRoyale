using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaModel
{
    private readonly int _maxMana;
    private int _mana;

    public ManaModel(int maxMana)
    {
        _maxMana = maxMana;
    }

    public bool Enough(int count) =>
        _mana >= count;

    public void Add(int count)
    {
        _mana += count;

        if(_mana > _maxMana)
        {
            _mana = _maxMana;
        }
    }

    public bool TryReduce(int count)
    {
        if(_mana >= count)
        {
            _mana -= count;

            return true;
        }

        return false;
    }
}
