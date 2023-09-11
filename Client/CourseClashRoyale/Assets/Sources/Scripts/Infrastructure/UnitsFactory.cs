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

        UnitView unitView = Object.Instantiate(_unitViewPrefab, position, Quaternion.identity);
        TargetProvider targetProvider = new();

        BaseUnitAnimator baseUnitAnimator = unitView.GetComponent<BaseUnitAnimator>();

        NavMeshAgent navMeshAgent = unitView.GetComponent<NavMeshAgent>();
        UnitAnimationAttack unitAttack = unitView.GetComponent<UnitAnimationAttack>();
        unitAttack.Init(targetProvider, baseUnitAnimator, damage);

        StateMachine stateMachine = unitView.gameObject.AddComponent<StateMachine>();

        HealthModel healthModel = new(targetHealth);
        HealthPresenter healthPresenter = new(healthModel);

        HealthView healthView = unitView.GetComponent<HealthView>();
        healthView.Init(healthPresenter, progressBarTarget, barCanHide: true);

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
            attackDistance: 1f,
            isFriendly);

        UnitAttackState attackState = new(
            stateMachine, 
            unitAttack.transform, 
            unitAttack, 
            targetProvider,
            disAttackRange: 1.5f);

        stateMachine.Register(targetChaseState);
        stateMachine.Register(attackState);
        stateMachine.Change<UnitTargetChaseState>();

        unitView.Init(isFriendly, type: TargetType.Ground);

        return unitView;
    }
}
