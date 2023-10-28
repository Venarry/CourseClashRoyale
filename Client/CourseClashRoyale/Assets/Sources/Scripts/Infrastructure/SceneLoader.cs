using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string sceneName, object data)
    {
        StartCoroutine(Load(sceneName, data));
    }

    private IEnumerator Load(string sceneName, object data)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
        //FindObjectOfType<GameEntryPoint>().Init(data);
    }
}
