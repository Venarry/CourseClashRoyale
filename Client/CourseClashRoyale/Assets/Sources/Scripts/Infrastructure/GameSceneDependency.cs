using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneDependency : MonoBehaviour
{
    [SerializeField] private Transform _mainTowerPoint;
    [SerializeField] private Transform _enemyMainTowerPoint;
    [SerializeField] private UserMapClickHandler _userMapClickHandler;
    [SerializeField] private Camera _camera;
    [SerializeField] private Canvas _canvas;

    private TowersFactory _towersFactory;
    private ProjectilesFactory _projectilesFactory;

    public void InitSceneBuilder(NetworkSceneBuilder networkSceneBuilder)
    {
        networkSceneBuilder.Init(_mainTowerPoint, _userMapClickHandler, _camera, _canvas);

        /*UserDataProvider userDataProvider = new();
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
        return;

        TowerView enemyMainTower = _towersFactory.CreateTower(
            _projectilesFactory,
            _enemyMainTowerPoint.position,
            _enemyMainTowerPoint.rotation,
            _camera.transform,
            isFriendly: false,
            isMainTower: true);

        unitsFactory.CreateBarbarianStack(
            new Vector3(0, 0, -10), isFriendly: true, 3);

        unitsFactory.CreateBarbarianStack(
            new Vector3(0, 0, 10), isFriendly: false, 15);

        unitsFactory.CreateDragonInferno(
            new Vector3(0, 0, -10), isFriendly: true, 5);

        unitsFactory.CreateDragonInferno(
            new Vector3(0, 0, 10), isFriendly: false, 15);*/
    }

    /*private void Awake()
    {
        if(FindObjectOfType<SceneLoader>() == null)
            Init(null);
    }*/

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
