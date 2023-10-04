using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitTargetChaseState : IState
{
    private readonly StateMachine _unitStateMachine;
    private readonly TargetProvider _targetProvider;
    private readonly NavMeshAgent _agent;
    private readonly UnitAttackAnimator _unitAnimator;
    private readonly Transform _transform;
    private readonly TargetFinder _targetFinder;
    private readonly float _attackDistance;

    public UnitTargetChaseState(
        StateMachine unitStateMachine,
        TargetProvider targetProvider,
        NavMeshAgent navMeshAgent, 
        UnitAttackAnimator unitAnimator,
        Transform transform,
        TargetFinder targetFinder,
        float attackDistance)
    {
        _unitStateMachine = unitStateMachine;
        _targetProvider = targetProvider;
        _agent = navMeshAgent;
        _unitAnimator = unitAnimator;
        _transform = transform;
        _targetFinder = targetFinder;
        _attackDistance = attackDistance;
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
            TryFindTower();
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
        if (_targetFinder.TryFindNearestTower() == false)
        {
            return;
        }

        ActivateUnit();
    }

    private void TryFindNewTarget()
    {
        if (_targetFinder.TryFindNearestUnit() == false)
            return;

        ActivateUnit();
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

    private void ActivateUnit()
    {
        _agent.isStopped = false;
        _unitAnimator.SetWalkState(true);
        _agent.SetDestination(_targetProvider.Position);
    }
}
