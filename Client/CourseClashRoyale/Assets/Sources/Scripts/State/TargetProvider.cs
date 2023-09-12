using UnityEngine;

public class TargetProvider
{
    private ITarget _target;
    private readonly ITarget _myUnit;

    public TargetProvider(ITarget myUnit)
    {
        _myUnit = myUnit;
    }

    public Vector3 Position =>
        _target.Transform.position;

    public float OurRadius => _target.Radius + _myUnit.Radius;
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
