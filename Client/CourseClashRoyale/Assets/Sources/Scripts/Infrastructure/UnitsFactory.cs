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
        int damage = baseDamage + (baseDamage * level / reduceMultiplier);

        float attackDistance = 0.5f;
        float disAttackRange = 1f;

        UnitView unitView = Object.Instantiate(_unitViewPrefab, position, Quaternion.identity);
        TargetProvider targetProvider = new(unitView);

        BaseUnitAnimator baseUnitAnimator = unitView.GetComponent<BaseUnitAnimator>();

        NavMeshAgent navMeshAgent = unitView.GetComponent<NavMeshAgent>();
        UnitAnimationAttack unitAttack = unitView.GetComponent<UnitAnimationAttack>();
        unitAttack.Init(targetProvider, baseUnitAnimator, damage, disAttackRange);

        StateMachine stateMachine = unitView.gameObject.AddComponent<StateMachine>();

        HealthModel healthModel = new(targetHealth);
        HealthPresenter healthPresenter = new(healthModel);

        HealthView healthView = unitView.GetComponent<HealthView>();
        Color barColor = isFriendly ? GameConfig.FriendlyBarColor : GameConfig.EnemyBarColor;
        healthView.Init(healthPresenter, progressBarTarget, barCanHide: true, barColor);

        AvailableTargetsProvider availableTargetsProvider = new();
        availableTargetsProvider.Register(TargetType.Ground, true);

        UnitTargetChaseState targetChaseState = new(
            stateMachine, 
            targetProvider,
            navMeshAgent, 
            _buildingsProvider,
            baseUnitAnimator,
            availableTargetsProvider,
            unitView.transform,
            agroRadius: 5f,
            attackDistance,
            isFriendly);

        UnitAttackState attackState = new(
            stateMachine, 
            unitAttack.transform, 
            unitAttack, 
            targetProvider,
            disAttackRange);

        stateMachine.Register(targetChaseState);
        stateMachine.Register(attackState);
        stateMachine.Change<UnitTargetChaseState>();

        unitView.Init(isFriendly, type: TargetType.Ground);

        return unitView;
    }
}
