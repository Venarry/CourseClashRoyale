using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkInitializer : MonoBehaviour
{
#if UNITY_SERVER || UNITY_EDITOR
    [SerializeField] private string _onlineScene;

#if UNITY_SERVER
    private void Awake()
    {
        if (NetworkManager.Singleton.StartServer())
        {
            SceneManager.LoadScene(_onlineScene);
        }
    }

#endif
#endif
}
