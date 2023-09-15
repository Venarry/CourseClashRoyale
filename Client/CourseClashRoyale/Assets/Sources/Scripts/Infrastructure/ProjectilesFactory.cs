using UnityEngine;

public class ProjectilesFactory
{
    private readonly MoveToProjectile _towerProjectilePrefab = Resources
        .Load<MoveToProjectile>(FilesPath.TowerProjectile);

    public MoveToProjectile CreateTowerProjectile(
        Vector3 position, 
        Quaternion rotation,
        ITarget target,
        int damage)
    {
        MoveToProjectile projectile = Object.Instantiate(_towerProjectilePrefab, position, rotation);
        projectile.Init(target, damage);

        return projectile;
    }
}
