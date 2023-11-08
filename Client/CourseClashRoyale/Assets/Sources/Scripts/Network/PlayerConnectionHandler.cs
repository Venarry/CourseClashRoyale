using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerConnectionHandler : NetworkBehaviour
{
    private GameEntryPoint _gameEntryPoint;

    private void Start()
    {
        if (IsOwner == false)
            return;

        _gameEntryPoint = FindObjectOfType<GameEntryPoint>();
        Debug.Log(_gameEntryPoint);
        BuildLevelServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void BuildLevelServerRpc()
    {
        Debug.Log("spawn");
        Debug.Log(_gameEntryPoint);
        _gameEntryPoint.CreateTowers();
    }
}
