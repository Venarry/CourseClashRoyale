using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Transform _mainTowerPoint;
    [SerializeField] private Transform _enemyMainTowerPoint;
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        BuildingsProvider buildingsProvider = new();

        UnitsFactory unitsFactory = new();
        unitsFactory.Init(buildingsProvider);

        TowersFactory towersFactory = new();
        towersFactory.Init(buildingsProvider);

        TowerView mainTower = towersFactory.CreateMainTower(
            _mainTowerPoint.position, 
            _mainTowerPoint.rotation, 
            _camera.transform,
            isFriendly: true);

        TowerView tower = towersFactory.CreateMainTower(
            _mainTowerPoint.position + new Vector3(5,0,0),
            _mainTowerPoint.rotation,
            _camera.transform,
            isFriendly: true);

        TowerView enemyMainTower = towersFactory.CreateMainTower(
            _enemyMainTowerPoint.position, 
            _enemyMainTowerPoint.rotation, 
            _camera.transform,
            isFriendly: false);

        unitsFactory.CreateWarrior(new Vector3(0,0,-10), _camera.transform, isFriendly: true, 1);
        unitsFactory.CreateWarrior(new Vector3(0,0,10), _camera.transform, isFriendly: false, 15);
    }
}
