using Unity.Netcode;
using UnityEngine;

public class NetworkSceneBuilder : NetworkBehaviour
{
    private Transform _mainTowerPoint;
    private UserMapClickHandler _userMapClickHandler;
    private Camera _camera;
    private Canvas _canvas;

    private TowersFactory _towersFactory;
    private ProjectilesFactory _projectilesFactory;

    public void Init(
        Transform mainTowerPoint,
        UserMapClickHandler userMapClickHandler,
        Camera camera,
        Canvas canvas)
    {
        _mainTowerPoint = mainTowerPoint;
        _userMapClickHandler = userMapClickHandler;
        _camera = camera;
        _canvas = canvas;
    }

    public void Build()
    {
        UserDataProvider userDataProvider = new();
        userDataProvider.LoadData();

        Card[] cards = userDataProvider.ActiveDeckCards;

        CardsDataSource cardsDataSource = new();
        BuildingsProvider buildingsProvider = new();

        CardFactory cardFactory = new(cardsDataSource);
        UnitsFactory unitsFactory = new(buildingsProvider, _camera.transform);
        GameDeckFactory gameDeckFactory = new(cardFactory, unitsFactory);
        _projectilesFactory = new();

        ManaModel manaModel = new(10, 8);

        GameDeckView gameDeckView = gameDeckFactory.Create(
            cards,
            GameConfig.CardsOnTable,
        manaModel,
            _canvas.transform);

        _towersFactory = new(buildingsProvider);

        _userMapClickHandler.Init(_camera, gameDeckView);
    }

    public void CreateTowers()
    {
        Debug.Log("Server");
        TowerView mainTower = _towersFactory.CreateTower(
            _projectilesFactory,
            _mainTowerPoint.position,
            _mainTowerPoint.rotation,
            _camera.transform,
            isFriendly: true,
            isMainTower: true);

        mainTower.GetComponent<NetworkObject>().Spawn();

        TowerView leftTower = _towersFactory.CreateTower(
            _projectilesFactory,
            _mainTowerPoint.position + new Vector3(-5, 0, 3),
            _mainTowerPoint.rotation,
            _camera.transform,
            isFriendly: true,
            isMainTower: false);

        leftTower.GetComponent<NetworkObject>().Spawn();

        TowerView rightTower = _towersFactory.CreateTower(
            _projectilesFactory,
            _mainTowerPoint.position + new Vector3(5, 0, 3),
            _mainTowerPoint.rotation,
            _camera.transform,
            isFriendly: true,
            isMainTower: false);

        rightTower.GetComponent<NetworkObject>().Spawn();
    }
}
