using UnityEngine;

public class TowersFactory
{
    private readonly TowerView _towerViewPrefab = Resources.Load<TowerView>(FilesPath.MainTower);
    private BuildingsProvider _buildingsProvider;

    public void Init(BuildingsProvider buildingsProvider)
    {
        _buildingsProvider = buildingsProvider;
    }

    public TowerView CreateMainTower(
        Vector3 position,
        Quaternion rotation,
        Transform progressBarTarget,
        bool isFriendly)
    {
        TowerView mainTower = Object.Instantiate(_towerViewPrefab, position, rotation);

        HealthModel healthModel = new(maxValue: 100);
        HealthPresenter healthPresenter = new(healthModel);

        HealthView healthView = mainTower.GetComponent<HealthView>();

        bool barCanHide = isFriendly;
        Color barColor = isFriendly ? GameConfig.FriendlyBarColor : GameConfig.EnemyBarColor;
        healthView.Init(healthPresenter, progressBarTarget, barCanHide, barColor);

        mainTower.Init(isFriendly, type: TargetType.Ground);
        _buildingsProvider.Add(mainTower, isFriendly);

        return mainTower;
    }
}
