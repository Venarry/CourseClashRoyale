using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackState : IState
{
    private StateMachine _stateMachine;
    private TargetProvider _targetProvider;
    private Transform _transform;
    private int _damage;
    private float _cooldown;
    private float _attakDistance;

    public TowerAttackState(
        StateMachine stateMachine,
        TargetProvider targetProvider,
        Transform transform,
        int damage, 
        float cooldown, 
        float attakDistance)
    {
        _stateMachine = stateMachine;
        _targetProvider = targetProvider;
        _transform = transform;
        _damage = damage;
        _cooldown = cooldown;
        _attakDistance = attakDistance;
    }

    public void EnterState()
    {
    }

    public void UpdateState()
    {
        if(Vector3.Distance(_transform.position, _targetProvider.Position) >
            _attakDistance + _targetProvider.OurRadius)
        {
            _stateMachine.Change<TowerWaitState>();
            return;
        }


    }

    public void ExitState()
    {
    }
}
