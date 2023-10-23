using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MatchmakingHandler : MonoBehaviour
{
//#if UNITY_SERVER
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

        Debug.Log("added");
        _players.Add(id);
    }

    private void OnClientDisconnect(ulong id)
    {
        if (_players.Contains(id) == false)
            return;

        Debug.Log("leaved");
        _players.Add(id);
    }

//#endif
}
