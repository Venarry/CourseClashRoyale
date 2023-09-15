using System;
using UnityEngine;

public class AttackState<T> : IState where T : IState
{
    private readonly StateMachine _stateMachine;
    private readonly TargetProvider _targetProvider;
    private readonly IAttack _attack;
    private readonly Transform _transform;
    private readonly Transform _rotateModel;
    private readonly float _disAttackRange;

    public AttackState(
        StateMachine stateMachine,
        TargetProvider targetProvider, 
        IAttack attack,
        Transform transform,
        Transform rotateModel,
        float disAttackRange)
    {
        _stateMachine = stateMachine;
        _targetProvider = targetProvider;
        _attack = attack;
        _transform = transform;
        _rotateModel = rotateModel;
        _disAttackRange = disAttackRange;
    }

    public void Enter()
    {
        
    }

    public void Update()
    {
        if (_targetProvider.HaveTarget == false)
        {
            _stateMachine.Change<T>();
            return;
        }

        _rotateModel.rotation = Quaternion
            .LookRotation(_targetProvider.Position - _rotateModel.position);

        if(Vector3.Distance(_transform.position, _targetProvider.Position) > 
            _disAttackRange + _targetProvider.OurRadius)
        {
            //_stateMachine.Change<UnitTargetChaseState>();
            _stateMachine.Change<T>();
            return;
        }

        _attack.TryAttack();
    }

    public void Exit()
    {

    }
}
