using UnityEngine;

public class UnitAnimationAttack : MonoBehaviour
{
    private TargetProvider _targetProvider;
    private BaseUnitAnimator _animator;
    private int _damage;
    private bool _canAttack = true;

    public void Init(
        TargetProvider targetProvider,
        BaseUnitAnimator animator,
        int damage)
    {
        _targetProvider = targetProvider;
        _animator = animator;
        _damage = damage;
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
        _targetProvider.AttackTarget(_damage);
    }

    public void AllowAttack()
    {
        _canAttack = true;
    }
}
