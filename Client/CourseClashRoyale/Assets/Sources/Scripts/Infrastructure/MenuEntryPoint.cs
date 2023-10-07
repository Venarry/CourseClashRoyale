using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private MenuCardsView _cardsView;
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        CardsDataSource cardsDataSource = new();
        CardFactory cardFactory = new(cardsDataSource);

        _cardsView.Init(cardFactory);

        _sceneLoader = FindObjectOfType<SceneLoader>() 
            ?? new GameObject(nameof(SceneLoader))
            .AddComponent<SceneLoader>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _sceneLoader.LoadScene("GameMap", "deck");
        }
    }
}
