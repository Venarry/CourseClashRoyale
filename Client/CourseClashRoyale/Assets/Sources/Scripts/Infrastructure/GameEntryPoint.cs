using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private Transform _mainTowerPoint;
    [SerializeField] private Transform _enemyMainTowerPoint;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;

    public void Init(object data)
    {
        Card[] cards = data as Card[];

        CardsDataSource cardsDataSource = new();

        BuildingsProvider buildingsProvider = new();

        CardFactory cardFactory = new(cardsDataSource);
        UnitsFactory unitsFactory = new(buildingsProvider);
        GameDeckFactory gameDeckFactory = new(cardFactory, unitsFactory);
        ProjectilesFactory projectilesFactory = new();

        gameDeckFactory.Create(
            cards,
            GameConfig.CardsOnTable,
            10,
            _canvas.transform);

        TowersFactory towersFactory = new();
        towersFactory.Init(buildingsProvider);

        TowerView mainTower = towersFactory.CreateTower(
            projectilesFactory,
            _mainTowerPoint.position,
            _mainTowerPoint.rotation,
            _camera.transform,
            isFriendly: true,
            isMainTower: true);

        TowerView tower = towersFactory.CreateTower(
            projectilesFactory,
            _mainTowerPoint.position + new Vector3(-5, 0, 3),
            _mainTowerPoint.rotation,
            _camera.transform,
            isFriendly: true,
            isMainTower: false);

        TowerView enemyMainTower = towersFactory.CreateTower(
            projectilesFactory,
            _enemyMainTowerPoint.position,
            _enemyMainTowerPoint.rotation,
            _camera.transform,
            isFriendly: false,
            isMainTower: true);

        unitsFactory.CreateBarbarianStack(
            new Vector3(0, 0, -10), _camera.transform, isFriendly: true, 3);

        unitsFactory.CreateBarbarianStack(
            new Vector3(0, 0, 10), _camera.transform, isFriendly: false, 15);

        unitsFactory.CreateDragonInferno(
            new Vector3(0, 0, -10), _camera.transform, isFriendly: true, 5);

        unitsFactory.CreateDragonInferno(
            new Vector3(0, 0, 10), _camera.transform, isFriendly: false, 15);
    }

    private void Awake()
    {
        if(FindObjectOfType<SceneLoader>() == null)
            Init(null);
    }
}
