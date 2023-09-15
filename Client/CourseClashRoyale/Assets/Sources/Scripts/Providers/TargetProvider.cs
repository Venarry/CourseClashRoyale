using UnityEngine;

public class TargetProvider
{
    private readonly ITarget _myUnit;

    public TargetProvider(ITarget myUnit)
    {
        _myUnit = myUnit;
    }

    public ITarget Target { get; private set; }

    public Vector3 Position =>
        Target.Transform.position;

    public float OurRadius => Target.Radius + _myUnit.Radius;
    public bool HaveTarget => Target != null;

    public void SetTarget(ITarget target)
    {
        if(Target != null)
        {
            Target.Destroyed -= OnTargetDestroy;
        }

        Target = target;
        Target.Destroyed += OnTargetDestroy;
    }

    private void OnTargetDestroy(ITarget target)
    {
        Target.Destroyed -= OnTargetDestroy;
        Target = null;
    }

    public void AttackTarget(int damage)
    {
        Target?.TakeDamage(damage);
    }
}
