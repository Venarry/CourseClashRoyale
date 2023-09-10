using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    private TargetProvider _targetProvider;
    private BaseUnitAnimator _animator;
    private int _damage;
    private float _cooldown;
    private float _currentTime;
    private bool _canAttack = true;

    public void Init(
        TargetProvider targetProvider,
        BaseUnitAnimator animator,
        int damage, 
        float cooldown)
    {
        _targetProvider = targetProvider;
        _animator = animator;
        _damage = damage;
        _cooldown = cooldown;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
    }

    public void TryAttack()
    {
        /*if (_currentTime < _cooldown)
        {
            return;
        }*/
        if (_canAttack == false)
            return;

        _animator.MakeAttack();
        _canAttack = false;
        //_currentTime = 0;
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
