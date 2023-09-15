using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitTargetChaseState : IState
{
    private readonly StateMachine _unitStateMachine;
    private readonly TargetProvider _targetProvider;
    private readonly NavMeshAgent _agent;
    private readonly BuildingsProvider _buildingsHolder;
    private readonly BaseUnitAnimator _unitAnimator;
    private readonly Transform _transform;
    private readonly TargetFinder _targetFinder;
    private readonly bool _isFriendly;
    private readonly float _attackDistance;

    public UnitTargetChaseState(
        StateMachine unitStateMachine,
        TargetProvider targetProvider,
        NavMeshAgent navMeshAgent, 
        BuildingsProvider buildingsHolder,
        BaseUnitAnimator unitAnimator,
        Transform transform,
        TargetFinder targetFinder,
        float attackDistance,
        bool isFriendly)
    {
        _unitStateMachine = unitStateMachine;
        _targetProvider = targetProvider;
        _agent = navMeshAgent;
        _buildingsHolder = buildingsHolder;
        _unitAnimator = unitAnimator;
        _transform = transform;
        _targetFinder = targetFinder;
        _attackDistance = attackDistance;
        _isFriendly = isFriendly;
    }

    public void Enter()
    {
        TryFindTower();
    }

    public void Update()
    {
        TryFindNewTarget();

        if (_targetProvider.HaveTarget == false)
        {
            _unitAnimator.SetWalkState(false);
            return;
        }

        _agent.SetDestination(_targetProvider.Position);
        TryStartAttack();
    }

    public void Exit()
    {
        _agent.isStopped = true;
        _unitAnimator.SetWalkState(false);
    }

    private void TryFindTower()
    {
        if (_buildingsHolder
            .TryFindNearestBuilding(_transform.position, _isFriendly == false, out ITarget target) == false)
        {
            return;
        }

        _agent.isStopped = false;
        _unitAnimator.SetWalkState(true);

        _targetProvider.SetTarget(target);
        _agent.SetDestination(target.Transform.position);
    }

    private void TryFindNewTarget()
    {
        _targetFinder.Find();
    }

    private void TryStartAttack()
    {
        if(Vector3.Distance(_transform.position, _targetProvider.Position) <= 
            _attackDistance + _targetProvider.OurRadius)
        {
            _agent.isStopped = true;
            _unitStateMachine.Change<AttackState<UnitTargetChaseState>>();
        }
    }
}
