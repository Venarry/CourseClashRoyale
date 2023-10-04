using UnityEngine;

public class UnitAttackAnimator : MonoBehaviour
{
    private readonly int _animatorWalk = Animator.StringToHash("Walk");
    private readonly int _animatorAttack = Animator.StringToHash("Attack");

    [SerializeField] private Animator _animator;

    public void SetWalkState(bool state)
    {
        _animator.SetBool(_animatorWalk, state);
    }

    public void MakeAttack()
    {
        _animator.SetTrigger(_animatorAttack);
    }
}
