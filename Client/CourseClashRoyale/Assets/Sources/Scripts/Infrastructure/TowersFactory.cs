using UnityEngine;

public class TowersFactory
{
    private readonly TowerView _towerViewPrefab = Resources.Load<TowerView>(FilesPath.MainTower);
    private BuildingsProvider _buildingsProvider;

    public void Init(BuildingsProvider buildingsProvider)
    {
        _buildingsProvider = buildingsProvider;
    }

    public TowerView CreateTower(
        ProjectilesFactory projectilesFactory,
        Vector3 position,
        Quaternion rotation,
        Transform progressBarTarget,
        bool isFriendly,
        bool isMainTower)
    {
        int health;
        float attackRange;
        int damage;
        float cooldown;

        if (isMainTower)
        {
            health = 400;
            damage = 60;
            attackRange = 9;
            cooldown = 0.5f;
        }
        else
        {
            health = 300;
            damage = 35;
            attackRange = 8;
            cooldown = 0.6f;
        }

        float disAttackRange = attackRange + 0.5f;

        TowerView tower = Object.Instantiate(_towerViewPrefab, position, rotation);

        AvailableTargetsProvider availableTargetsProvider = new();
        availableTargetsProvider.Register(TargetType.GroundUnit, true);
        availableTargetsProvider.Register(TargetType.AirUnit, true);

        TargetProvider targetProvider = new(tower);

        HealthModel healthModel = new(health);
        HealthPresenter healthPresenter = new(healthModel);

        HealthView healthView = tower.GetComponent<HealthView>();

        bool barCanHide = isFriendly;
        Color barColor = isFriendly ? GameConfig.FriendlyBarColor : GameConfig.EnemyBarColor;
        healthView.Init(healthPresenter, progressBarTarget, barCanHide, barColor);

        TargetFinder targetFinder = new(
            availableTargetsProvider,
            targetProvider,
            _buildingsProvider,
            tower.transform,
            isFriendly,
            attackRange);

        StateMachine stateMachine = tower.GetComponent<StateMachine>();

        TowerFindTargetState towerFindTargetState = new(
            stateMachine, targetProvider, targetFinder, tower.transform, attackRange);

        TowerAttack towerAttack = tower.GetComponent<TowerAttack>();
        towerAttack.Init(targetProvider, projectilesFactory, damage);

        AttackByTimerInitiator attackByTimerInitiator = tower.GetComponent<AttackByTimerInitiator>();
        attackByTimerInitiator.Init(towerAttack, cooldown);

        AttackState<TowerFindTargetState> attackState = new(
            stateMachine,
            targetProvider,
            attackByTimerInitiator,
            tower.transform,
            tower.Head,
            disAttackRange);

        stateMachine.Register(towerFindTargetState);
        stateMachine.Register(attackState);

        tower.Init(isFriendly, type: TargetType.Tower);
        _buildingsProvider.Add(tower, isFriendly);


        if (isMainTower == true)
        {
            TowerInactiveState towerInactiveState = new(
                stateMachine, _buildingsProvider, healthPresenter);

            stateMachine.Register(towerInactiveState);
            stateMachine.Change<TowerInactiveState>();
        }
        else
        {
            stateMachine.Change<TowerFindTargetState>();
        }

        return tower;
    }
}
