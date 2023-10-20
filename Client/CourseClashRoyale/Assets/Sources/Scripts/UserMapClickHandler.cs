using UnityEngine;

public class UserMapClickHandler : MonoBehaviour
{
    private Camera _camera;
    private GameDeckView _gameDeckView;

    public void Init(Camera camera, GameDeckView gameDeckView)
    {
        _camera = camera;
        _gameDeckView = gameDeckView;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySpawnHero();
        }
    }

    private void TrySpawnHero()
    {
        if(Physics.Raycast(
            _camera.transform.position,
            _camera.transform.forward,
            out RaycastHit hitInfo))
        {
            if (hitInfo.collider.TryGetComponent(out SpawnArea spawnArea))
            {
                Debug.Log("TrySpawn");
                _gameDeckView.TrySpawnHero();
            }
        }
    }
}
