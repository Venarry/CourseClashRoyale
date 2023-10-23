using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitsFactory
{
    private readonly UnitView _barbarianPrefab = Resources.Load<UnitView>(FilesPath.Barbarian);
    private readonly UnitView _dragonInfernoPrefab = Resources.Load<UnitView>(FilesPath.DragonInferno);
    private readonly BuildingsProvider _buildingsProvider;
    private readonly Transform _rotationTarget;

    private readonly Dictionary<
        int,
        Action<Vector3, bool, int>> _units = new();

    private int _reduceMultiplier => GameConfig.LevelStrengthReduceMultiplier;

    public UnitsFactory(BuildingsProvider buildingsProvider, Transform rotationTarget)
    {
        _buildingsProvider = buildingsProvider;
        _rotationTarget = rotationTarget;

        _units.Add(1, CreateBarbarian);
        _units.Add(2, CreateBarbarianStack);
        _units.Add(3, CreateDragonInferno);

        _units.Add(4, CreateBarbarian);
        _units.Add(5, CreateBarbarianStack);
        _units.Add(6, CreateDragonInferno);
        _units.Add(7, CreateBarbarian);
        _units.Add(8, CreateBarbarianStack);
        _units.Add(9, CreateDragonInferno);
        _units.Add(10, CreateDragonInferno);
    }

    public void Create(int id, Vector3 position, bool isFriendly, int level)
    {
        _units[id](position, isFriendly, level);
    }

    public void CreateBarbarianStack(Vector3 position,
        bool isFriendly,
        int level)
    {
        float spawnOffset = 0.5f;

        CreateBarbarian(
            position + new Vector3(spawnOffset, 0, spawnOffset),
            isFriendly,
            level);

        CreateBarbarian(
            position + new Vector3(spawnOffset, 0, -spawnOffset),
            isFriendly,
            level);

        CreateBarbarian(
            position + new Vector3(-spawnOffset, 0, spawnOffset),
            isFriendly,
            level);

        CreateBarbarian(
            position + new Vector3(-spawnOffset, 0, -spawnOffset),
            isFriendly,
            level);
    }

    public void CreateBarbarian(
        Vector3 position,
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

        UnitView unitView = UnityEngine.Object.Instantiate(_barbarianPrefab, position, Quaternion.identity);
        BuildAttackUnit(
            unitView,
            targetDamage,
            attackRange,
            disAttackRange,
            agroRadius,
            targetHealth,
            isFriendly,
            UnitType.GroundUnit,
            new UnitType[] { UnitType.GroundUnit, UnitType.Tower });
    }

    public void CreateDragonInferno(
        Vector3 position,
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

        UnitView unitView = UnityEngine.Object.Instantiate(_dragonInfernoPrefab, position, Quaternion.identity);
        BuildAttackUnit(
            unitView,
            targetDamage,
            attackRange,
            disAttackRange,
            agroRadius,
            targetHealth,
            isFriendly,
            UnitType.AirUnit,
            new UnitType[] { UnitType.GroundUnit, UnitType.Tower, UnitType.AirUnit });
    }

    private void BuildAttackUnit(UnitView unitView,
        int damage,
        float attackRange,
        float disAttackRange,
        float agroRadius,
        int health,
        bool isFriendly,
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
        healthView.Init(healthPresenter, _rotationTarget, barCanHide: true, barColor);

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
