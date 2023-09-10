using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitWalkState : IState
{
    private readonly Collider[] _colliders = new Collider[32];
    private readonly StateMachine _unitStateMachine;
    private readonly TargetProvider _targetProvider;
    private readonly NavMeshAgent _agent;
    private readonly BuildingsProvider _buildingsHolder;
    private readonly BaseUnitAnimator _unitAnimator;
    private readonly Transform _transform;
    private readonly bool _isFriendly;
    private readonly float _agroRadius;
    private readonly float _attackDistance;
    private ITarget _target;

    public UnitWalkState(
        StateMachine unitStateMachine,
        TargetProvider targetProvider,
        NavMeshAgent navMeshAgent, 
        BuildingsProvider buildingsHolder,
        BaseUnitAnimator unitAnimator,
        Transform transform,
        float agroRadius,
        float attackDistance,
        bool isFriendly)
    {
        _unitStateMachine = unitStateMachine;
        _targetProvider = targetProvider;
        _agent = navMeshAgent;
        _buildingsHolder = buildingsHolder;
        _unitAnimator = unitAnimator;
        _transform = transform;
        _agroRadius = agroRadius;
        _attackDistance = attackDistance;
        _isFriendly = isFriendly;
    }

    public void EnterState()
    {
        TryFindTower();
    }

    public void UpdateState()
    {
        TryFindNewTarget();

        if (_targetProvider.HaveTarget == false)
        {
            _unitAnimator.SetWalkState(false);
            return;
        }

        _agent.SetDestination(_target.Transform.position);
        TryStartAttack();
    }

    public void ExitState()
    {
        _agent.isStopped = true;
        _unitAnimator.SetWalkState(false);
    }

    private void TryFindTower()
    {
        if (_buildingsHolder
            .TryFindNearestBuilding(_transform.position, _isFriendly == false, out _target) == false)
        {
            return;
        }

        _agent.isStopped = false;
        _unitAnimator.SetWalkState(true);
        _agent.stoppingDistance = _attackDistance;

        _targetProvider.SetTarget(_target);
        _agent.SetDestination(_target.Transform.position);
    }

    private void TryFindNewTarget()
    {
        int collidesCount = Physics.OverlapSphereNonAlloc(
            _transform.position, _agroRadius, _colliders);

        for (int i = 0; i < collidesCount; i++)
        {
            if (_colliders[i].TryGetComponent(out ITarget target))
            {
                if (target == _target)
                    continue;

                if (target.IsFriendly == _isFriendly)
                    continue;

                _target = target;
                _targetProvider.SetTarget(_target);
            }
        }
    }

    private void TryStartAttack()
    {
        if(Vector3.Distance(_transform.position, _target.Transform.position) <= 
            _attackDistance + _target.Radius)
        {
            _agent.isStopped = true;
            _targetProvider.SetTarget(_target);
            _unitStateMachine.Change<UnitAttackState>();
        }
    }
}
