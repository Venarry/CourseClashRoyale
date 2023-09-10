using System;
using UnityEngine;

public class UnitAttackState : IState
{
    private readonly StateMachine _stateMachine;
    private readonly Transform _transform;
    private readonly UnitAnimationAttack _unitAttack;
    private readonly TargetProvider _targetProvider;
    private readonly float _disAttackRange;

    public UnitAttackState(
        StateMachine stateMachine, 
        Transform transform, 
        UnitAnimationAttack unitAttack,
        TargetProvider targetProvider, 
        float disAttackRange)
    {
        _stateMachine = stateMachine;
        _transform = transform;
        _unitAttack = unitAttack;
        _targetProvider = targetProvider;
        _disAttackRange = disAttackRange;
    }

    public void EnterState()
    {
        
    }

    public void UpdateState()
    {
        if (_targetProvider.HaveTarget == false)
        {
            _stateMachine.Change<UnitWalkState>();
            return;
        }

        _transform.rotation = Quaternion
            .LookRotation(_targetProvider.TargetPosition - _transform.position);

        if(Vector3.Distance(_transform.position, _targetProvider.TargetPosition) >= _disAttackRange)
        {
            _stateMachine.Change<UnitWalkState>();
            return;
        }

        _unitAttack.TryAttack();
    }

    public void ExitState()
    {

    }
}
