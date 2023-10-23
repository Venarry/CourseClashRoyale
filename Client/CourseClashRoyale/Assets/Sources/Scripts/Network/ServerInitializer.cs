using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerInitializer : MonoBehaviour
{
#if UNITY_SERVER || UNITY_EDITOR
    [SerializeField] private string _onlineScene;
    [SerializeField] private NetworkManager _networkManager;

#if UNITY_SERVER

    private void Start()
    {
        if (_networkManager.StartServer())
        {
            SceneManager.LoadScene(_onlineScene);

        }
    }
#endif
#endif
}
