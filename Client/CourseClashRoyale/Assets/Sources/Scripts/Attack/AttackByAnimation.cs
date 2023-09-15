using UnityEngine;

public class AttackByAnimation : MonoBehaviour, IAttack
{
    private TargetProvider _targetProvider;
    private BaseUnitAnimator _animator;
    private int _damage;
    private float _attackDistance;
    private bool _canAttack = true;

    public void Init(
        TargetProvider targetProvider,
        BaseUnitAnimator animator,
        int damage,
        float attackDistance)
    {
        _targetProvider = targetProvider;
        _animator = animator;
        _damage = damage;
        _attackDistance = attackDistance;
    }

    public void TryAttack()
    {
        if (_canAttack == false)
            return;

        _animator.MakeAttack();
        _canAttack = false;
    }

    public void ApplyDamageOnTarget()
    {
        if (_targetProvider.HaveTarget == false)
            return;

        if(Vector3.Distance(_targetProvider.Position, transform.position) <=
            _attackDistance + _targetProvider.OurRadius)
        {
            _targetProvider.AttackTarget(_damage);
        }
    }

    public void AllowAttack()
    {
        _canAttack = true;
    }
}
