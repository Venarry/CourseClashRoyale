using UnityEngine;

public class AttackByTimerInitiator : MonoBehaviour, IAttack
{
    private IAttack _attack;
    private float _cooldown;
    private float _time;

    public bool IsReady => _time >= _cooldown;

    public void Init(IAttack attack, float cooldown)
    {
        _attack = attack;
        _cooldown = cooldown;
    }

    private void Update()
    {
        _time += Time.deltaTime;
    }

    public void TryAttack()
    {
        if(IsReady == true)
        {
            _attack.TryAttack();
            _time = 0;
        }
    }
}
