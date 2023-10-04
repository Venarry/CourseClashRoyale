using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder
{
    private readonly Collider[] _colliders = new Collider[32];
    private readonly AvailableTargetsProvider _availableTargetsProvider;
    private readonly TargetProvider _targetProvider;
    private readonly BuildingsProvider _buildingsProvider;
    private readonly Transform _transform;
    private readonly bool _isFriendly;
    private readonly float _agroRadius;

    public TargetFinder(
        AvailableTargetsProvider availableTargetsProvider, 
        TargetProvider targetProvider,
        BuildingsProvider buildingsProvider,
        Transform transform,
        bool isFriendly,
        float agroRadius)
    {
        _availableTargetsProvider = availableTargetsProvider;
        _targetProvider = targetProvider;
        _buildingsProvider = buildingsProvider;
        _transform = transform;
        _isFriendly = isFriendly;
        _agroRadius = agroRadius;
    }

    public bool TryFindNearestUnit()
    {
        int collidesCount = Physics.OverlapSphereNonAlloc(
            _transform.position, _agroRadius, _colliders);

        float minDistance = _agroRadius;
        ITarget bufferTarget = null;

        for (int i = 0; i < collidesCount; i++)
        {
            if (_colliders[i].TryGetComponent(out ITarget target))
            {
                if (target.IsFriendly == _isFriendly)
                    continue;

                if (_availableTargetsProvider.CanInteract(target) == false)
                    continue;

                float currentTargetDistance = Vector3
                    .Distance(_transform.position, target.Transform.position);

                if (currentTargetDistance <= minDistance)
                {
                    bufferTarget = target;
                }
            }
        }

        if (bufferTarget != null)
        {
            if (bufferTarget == _targetProvider.Target)
                return false;

            _targetProvider.SetTarget(bufferTarget);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryFindNearestTower()
    {
        if (_buildingsProvider
            .TryFindNearestBuilding(
            _transform.position, _isFriendly == false, out ITarget target) == false)
        {
            return false;
        }

        if (_availableTargetsProvider.CanInteract(target) == false)
        {
            return false;
        }

        _targetProvider.SetTarget(target);
        return true;
    }
}
