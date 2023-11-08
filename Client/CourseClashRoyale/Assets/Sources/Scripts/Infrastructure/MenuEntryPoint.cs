using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MenuCardsView _cardsView;
    [SerializeField] private AuthorizationView _authentifiactionView;
    [SerializeField] private NetworkManager _networkManager;

    private SceneLoader _sceneLoader;
    private UserDataProvider _userDataProvider;

    private void Awake()
    {
        _userDataProvider = new();
        CardsDataSource cardsDataSource = new();
        CardFactory cardFactory = new(cardsDataSource);

        _authentifiactionView.Init(_userDataProvider);

        _cardsView.Init(cardFactory, _userDataProvider);

        _sceneLoader = FindObjectOfType<SceneLoader>() 
            ?? new GameObject(nameof(SceneLoader))
            .AddComponent<SceneLoader>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _sceneLoader.LoadScene("GameMap", _userDataProvider.Decks[0].C);
        }

        /*if (Input.GetKeyDown(KeyCode.S))
        {
            _networkManager.StartServer();
            SceneManager.LoadScene("GameMap");
        }*/

        if (Input.GetKeyDown(KeyCode.C))
        {
            _networkManager.StartClient();
            //SceneManager.LoadScene("OnlineScene");
        }
    }
}
