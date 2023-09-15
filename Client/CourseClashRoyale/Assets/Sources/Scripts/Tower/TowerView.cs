using System;
using UnityEngine;

[RequireComponent(typeof(HealthView))]
public class TowerView : MonoBehaviour, ITarget
{
    [field: SerializeField] public Transform Head { get; private set; }

    private HealthView _healthView;
    private bool _isInitialized;

    public event Action<ITarget> Destroyed;

    public TargetType Type { get; private set; }
    public Transform Transform => transform;
    public bool IsFriendly { get; private set; }
    public float Radius => 1f;


    private void Awake()
    {
        _healthView = GetComponent<HealthView>();
    }

    public void Init(bool isEnemy, TargetType type)
    {
        gameObject.SetActive(false);

        Type = type;
        IsFriendly = isEnemy;
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    public void SetHealthBarVisibleState(bool state)
    {
        _healthView.SetHealthBarVisibleState(state);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _healthView.HealthOver += OnHealthOver;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _healthView.HealthOver -= OnHealthOver;
    }

    public void TakeDamage(int value)
    {
        _healthView.TakeDamage(value);
    }

    private void OnHealthOver()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }
}
