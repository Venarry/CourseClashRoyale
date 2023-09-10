using System;
using UnityEngine;

public class TargetProvider
{
    private ITarget _target;

    public Vector3 TargetPosition =>
        _target.Transform.position;

    public float Radius => _target.Radius;
    public bool HaveTarget => _target != null;

    public void SetTarget(ITarget target)
    {
        if(_target != null)
        {
            _target.Destroyed -= OnTargetDestroy;
        }

        _target = target;
        _target.Destroyed += OnTargetDestroy;
    }

    private void OnTargetDestroy(ITarget target)
    {
        _target.Destroyed -= OnTargetDestroy;
        _target = null;
    }

    public void AttackTarget(int damage)
    {
        _target?.TakeDamage(damage);
    }
}
