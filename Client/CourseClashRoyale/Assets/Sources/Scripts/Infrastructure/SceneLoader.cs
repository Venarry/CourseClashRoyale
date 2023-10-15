using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string sceneName, string payload)
    {
        StartCoroutine(Load(sceneName, payload));
    }

    private IEnumerator Load(string sceneName, string payload)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
        FindObjectOfType<Bootstrapper>().Init(payload);
    }
}
