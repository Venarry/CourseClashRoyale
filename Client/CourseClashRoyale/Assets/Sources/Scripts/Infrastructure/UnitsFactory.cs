using UnityEngine;
using UnityEngine.AI;

public class UnitsFactory
{
    private readonly UnitView _unitViewPrefab = Resources.Load<UnitView>(FilesPath.Barbarian);
    private BuildingsProvider _buildingsProvider;

    public void Init(BuildingsProvider buildingsProvider)
    {
        _buildingsProvider = buildingsProvider;
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
        int reduceMultiplier = GameConfig.LevelStrengthReduceMultiplier;

        int baseHealth = 100;
        int targetHealth = baseHealth + (baseHealth * level / reduceMultiplier);

        int baseDamage = 10;
        int targetDamage = baseDamage + (baseDamage * level / reduceMultiplier);

        float agroRadius = 5f;
        float attackRange = 0.5f;
        float disAttackRange = attackRange + 0.5f;

        UnitView unitView = Object.Instantiate(_unitViewPrefab, position, Quaternion.identity);
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

        /*TargetProvider targetProvider = new(unitView);

        BaseUnitAnimator baseUnitAnimator = unitView.GetComponent<BaseUnitAnimator>();

        NavMeshAgent navMeshAgent = unitView.GetComponent<NavMeshAgent>();
        AttackByAnimation unitAttack = unitView.GetComponent<AttackByAnimation>();
        unitAttack.Init(targetProvider, baseUnitAnimator, damage, disAttackRange);

        StateMachine stateMachine = unitView.gameObject.AddComponent<StateMachine>();

        HealthModel healthModel = new(targetHealth);
        HealthPresenter healthPresenter = new(healthModel);

        HealthView healthView = unitView.GetComponent<HealthView>();
        Color barColor = isFriendly ? GameConfig.FriendlyBarColor : GameConfig.EnemyBarColor;
        healthView.Init(healthPresenter, progressBarTarget, barCanHide: true, barColor);

        AvailableTargetsProvider availableTargetsProvider = new();
        availableTargetsProvider.Register(UnitType.GroundUnit, true);
        availableTargetsProvider.Register(UnitType.Tower, true);

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
            attackDistance);

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

        unitView.Init(isFriendly, type: UnitType.GroundUnit);
        */
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

        BaseUnitAnimator baseUnitAnimator = unitView.GetComponent<BaseUnitAnimator>();

        NavMeshAgent navMeshAgent = unitView.GetComponent<NavMeshAgent>();
        AttackByAnimation unitAttack = unitView.GetComponent<AttackByAnimation>();
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
