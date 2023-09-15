using UnityEngine;

public class TowerAttack : MonoBehaviour, IAttack
{
    [SerializeField] private Transform _shootPoint;

    private int _damage;
    private TargetProvider _targetProvider;
    private ProjectilesFactory _projectilesFactory;

    public void Init(
        TargetProvider targetProvider,
        ProjectilesFactory projectilesFactory,
        int damage)
    {
        _targetProvider = targetProvider;
        _projectilesFactory = projectilesFactory;
        _damage = damage;
    }

    public void TryAttack()
    {
        _projectilesFactory.CreateTowerProjectile(
            _shootPoint.position,
            _shootPoint.rotation,
            _targetProvider.Target,
            _damage);
    }
}
