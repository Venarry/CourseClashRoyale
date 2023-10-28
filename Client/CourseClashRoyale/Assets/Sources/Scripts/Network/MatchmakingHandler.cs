using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchmakingHandler : MonoBehaviour
{
//#if UNITY_SERVER
    private const string NameGameMap = "GameMap";

    private NetworkManager _networkManager;

    private readonly List<ulong> _players = new();

    private void Awake()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
    }

    private void OnEnable()
    {
        if (_networkManager.IsServer == false)
            return;

        Debug.Log("server");
        _networkManager.OnClientConnectedCallback += OnClientConnect;
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            _networkManager.Shutdown();
        }
    }

    private void OnClientConnect(ulong id)
    {
        if (_players.Contains(id))
            return;

        //_networkManager.ConnectedClients[id].PlayerObject.ne

        Debug.Log("added");
        _players.Add(id);
        TryStartMatch();
    }

    private void OnClientDisconnect(ulong id)
    {
        if (_players.Contains(id) == false)
            return;

        Debug.Log("leaved");
        _players.Add(id);
    }

    private void TryStartMatch()
    {
        if (_players.Count < 2)
            return;

        StartCoroutine(CreateGameScene());
    }

    private IEnumerator CreateGameScene()
    {
        //yield return SceneManager.LoadSceneAsync(NameGameMap, LoadSceneMode.Additive);
        yield return _networkManager.SceneManager.LoadScene(NameGameMap, LoadSceneMode.Additive);
    }

//#endif
}
