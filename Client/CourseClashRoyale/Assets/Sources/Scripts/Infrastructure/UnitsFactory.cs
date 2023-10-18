using UnityEngine;
using UnityEngine.AI;

public class UnitsFactory
{
    private readonly UnitView _barbarianPrefab = Resources.Load<UnitView>(FilesPath.Barbarian);
    private readonly UnitView _dragonInfernoPrefab = Resources.Load<UnitView>(FilesPath.DragonInferno);
    private readonly BuildingsProvider _buildingsProvider;

    private int _reduceMultiplier => GameConfig.LevelStrengthReduceMultiplier;

    public UnitsFactory(BuildingsProvider buildingsProvider)
    {
        _buildingsProvider = buildingsProvider;
    }

    public void Create(int id)
    {

    }

    public void CreateBarbarianStack(Vector3 position,
        Transform progressBarTarget,
        bool isFriendly,
        int level)
    {
        float spawnOffset = 0.5f;

        CreateBarbarian(position + new Vector3(spawnOffset, 0, spawnOffset),
            progressBarTarget, isFriendly, level);

        CreateBarbarian(position + new Vector3(spawnOffset, 0, -spawnOffset),
            progressBarTarget, isFriendly, level);

        CreateBarbarian(position + new Vector3(-spawnOffset, 0, spawnOffset),
            progressBarTarget, isFriendly, level);

        CreateBarbarian(position + new Vector3(-spawnOffset, 0, -spawnOffset),
            progressBarTarget, isFriendly, level);
    }

    public UnitView CreateBarbarian(
        Vector3 position,
        Transform progressBarTarget, 
        bool isFriendly,
        int level)
    {
        int baseHealth = 100;
        int targetHealth = baseHealth + (baseHealth * level / _reduceMultiplier);

        int baseDamage = 10;
        int targetDamage = baseDamage + (baseDamage * level / _reduceMultiplier);

        float agroRadius = 5f;
        float attackRange = 0.5f;
        float disAttackRange = attackRange + 0.5f;

        UnitView unitView = Object.Instantiate(_barbarianPrefab, position, Quaternion.identity);
        BuildAttackUnit(
            unitView,
            targetDamage,
            attackRange,
            disAttackRange,
            agroRadius,
            targetHealth,
            isFriendly,
            progressBarTarget,
            UnitType.GroundUnit,
            new UnitType[] { UnitType.GroundUnit, UnitType.Tower });

        return unitView;
    }

    public UnitView CreateDragonInferno(
        Vector3 position,
        Transform progressBarTarget,
        bool isFriendly,
        int level)
    {
        int baseHealth = 350;
        int targetHealth = baseHealth + (baseHealth * level / _reduceMultiplier);

        int baseDamage = 35;
        int targetDamage = baseDamage + (baseDamage * level / _reduceMultiplier);

        float agroRadius = 6f;
        float attackRange = 3f;
        float disAttackRange = attackRange + 0.5f;

        UnitView unitView = Object.Instantiate(_dragonInfernoPrefab, position, Quaternion.identity);
        BuildAttackUnit(
            unitView,
            targetDamage,
            attackRange,
            disAttackRange,
            agroRadius,
            targetHealth,
            isFriendly,
            progressBarTarget,
            UnitType.AirUnit,
            new UnitType[] { UnitType.GroundUnit, UnitType.Tower, UnitType.AirUnit });

        return unitView;
    }

    private void BuildAttackUnit(UnitView unitView,
        int damage,
        float attackRange,
        float disAttackRange,
        float agroRadius,
        int health,
        bool isFriendly,
        Transform progressBarTarget,
        UnitType type,
        UnitType[] availableTargets)
    {
        TargetProvider targetProvider = new(unitView);

        UnitAttackAnimator baseUnitAnimator = unitView.GetComponent<UnitAttackAnimator>();

        NavMeshAgent navMeshAgent = unitView.GetComponent<NavMeshAgent>();
        AttackByAnimationInitiator unitAttack = unitView.GetComponent<AttackByAnimationInitiator>();
        unitAttack.Init(targetProvider, baseUnitAnimator, damage, disAttackRange);

        StateMachine stateMachine = unitView.gameObject.AddComponent<StateMachine>();

        HealthModel healthModel = new(health);
        HealthPresenter healthPresenter = new(healthModel);

        HealthView healthView = unitView.GetComponent<HealthView>();
        Color barColor = isFriendly ? GameConfig.FriendlyBarColor : GameConfig.EnemyBarColor;
        healthView.Init(healthPresenter, progressBarTarget, barCanHide: true, barColor);

        AvailableTargetsProvider availableTargetsProvider = new();

        foreach (var target in availableTargets)
        {
            availableTargetsProvider.Register(target, true);
        }

        TargetFinder targetFinder = new(
            availableTargetsProvider,
            targetProvider,
            _buildingsProvider,
            unitView.transform,
            isFriendly,
            agroRadius);

        UnitTargetChaseState targetChaseState = new(
            stateMachine,
            targetProvider,
            navMeshAgent,
            baseUnitAnimator,
            unitView.transform,
            targetFinder,
            attackRange);

        AttackState<UnitTargetChaseState> attackState = new(
            stateMachine,
            targetProvider,
            unitAttack,
            unitAttack.transform,
            unitAttack.transform,
            disAttackRange);

        stateMachine.Register(targetChaseState);
        stateMachine.Register(attackState);
        stateMachine.Change<UnitTargetChaseState>();

        unitView.Init(isFriendly, type);
    }
}
