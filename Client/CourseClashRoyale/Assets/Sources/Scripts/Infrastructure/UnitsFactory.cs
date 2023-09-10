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

    public UnitView CreateWarrior(
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

        float attackCooldown = 0.7f;

        UnitView unitView = Object.Instantiate(_unitViewPrefab, position, Quaternion.identity);
        TargetProvider targetProvider = new();

        BaseUnitAnimator baseUnitAnimator = unitView.GetComponent<BaseUnitAnimator>();

        NavMeshAgent navMeshAgent = unitView.GetComponent<NavMeshAgent>();
        UnitAttack unitAttack = unitView.GetComponent<UnitAttack>();
        unitAttack.Init(targetProvider, baseUnitAnimator, damage, attackCooldown);

        StateMachine stateMachine = unitView.gameObject.AddComponent<StateMachine>();

        HealthModel healthModel = new(targetHealth);
        HealthPresenter healthPresenter = new(healthModel);

        HealthView healthView = unitView.GetComponent<HealthView>();
        healthView.Init(healthPresenter, progressBarTarget, barCanHide: true);

        UnitWalkState walkState = new(
            stateMachine, 
            targetProvider,
            navMeshAgent, 
            _buildingsProvider,
            baseUnitAnimator,
            unitView.transform,
            agroRadius: 5f,
            attackDistance: 2f,
            isFriendly);

        UnitAttackState attackState = new(
            stateMachine, 
            unitAttack.transform, 
            unitAttack, 
            targetProvider,
            disAttackRange: 2.5f);

        stateMachine.Register(walkState);
        stateMachine.Register(attackState);
        stateMachine.Change<UnitWalkState>();

        unitView.Init(isFriendly);

        return unitView;
    }
}
