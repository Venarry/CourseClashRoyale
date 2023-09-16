using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFindTargetState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly TargetProvider _targetProvider;
    private readonly TargetFinder _targetFinder;
    private readonly Transform _transform;
    private readonly float _attackDistance;

    public TowerFindTargetState(
        StateMachine stateMachine, 
        TargetProvider targetProvider, 
        TargetFinder targetFinder, 
        Transform transform, 
        float attackDistance)
    {
        _stateMachine = stateMachine;
        _targetProvider = targetProvider;
        _targetFinder = targetFinder;
        _transform = transform;
        _attackDistance = attackDistance;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        _targetFinder.TryFindNearestUnit();

        if (_targetProvider.HaveTarget == false)
        {
            return;
        }

        TryStartAttack();
    }

    public void Exit()
    {
        
    }

    private void TryStartAttack()
    {
        if (Vector3.Distance(_transform.position, _targetProvider.Position) <=
            _attackDistance + _targetProvider.OurRadius)
        {
            _stateMachine.Change<AttackState<TowerFindTargetState>>();
        }
    }
}
