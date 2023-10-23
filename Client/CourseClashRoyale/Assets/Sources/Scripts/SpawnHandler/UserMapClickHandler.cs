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
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        float rayDistnace = 100;

        if(Physics.Raycast(
            ray,
            out RaycastHit hitInfo,
            rayDistnace))
        {
            if (hitInfo.collider.TryGetComponent(out SpawnArea spawnArea))
            {
                _gameDeckView.TrySpawnHero(hitInfo.point);
            }
        }
    }
}
