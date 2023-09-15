using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingsProvider
{
    private readonly List<ITarget> _friendlyTowers = new();
    private readonly List<ITarget> _enemyTowers = new();

    public event Action<ITarget> FriendlyTowerDestroyed;

    public void Add(TowerView tower, bool isFriendly)
    {
        List<ITarget> towers = isFriendly ? _friendlyTowers : _enemyTowers;
        towers.Add(tower);
        tower.Destroyed += OnTowerDestroy;
    }

    private void OnTowerDestroy(ITarget tower)
    {
        tower.Destroyed -= OnTowerDestroy;

        if (tower.IsFriendly == true)
        {
            _friendlyTowers.Remove(tower);
            FriendlyTowerDestroyed?.Invoke(tower);
        }
        else
        {
            _enemyTowers.Remove(tower);
        }
    }

    public bool TryFindNearestBuilding(Vector3 position, bool isFriendly, out ITarget target)
    {
        target = null;

        List<ITarget> towers = isFriendly ? _friendlyTowers : _enemyTowers;

        if (towers.Count <= 0)
            return false;

        if(towers.Count == 1)
        {
            target = towers[0];
            return true;
        }

        int nearestDistanceTowerIndex = 0;

        for (int i = 1; i < towers.Count; i++)
        {
            float currentTowerDistance = Vector3.Distance(towers[i].Transform.position, position);
            float nearestTowerDistnace = Vector3.Distance(towers[nearestDistanceTowerIndex].Transform.position, position);

            if (currentTowerDistance < nearestTowerDistnace)
            {
                nearestDistanceTowerIndex = i;
            }
        }

        target = towers[nearestDistanceTowerIndex];
        return true;
    }
}
