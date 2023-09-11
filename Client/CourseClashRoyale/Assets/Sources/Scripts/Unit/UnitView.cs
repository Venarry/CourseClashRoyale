using System;
using UnityEngine;

[RequireComponent(typeof(HealthView))]
public class UnitView : MonoBehaviour, ITarget
{
    [SerializeField] private float _radius = 1f;
    private HealthView _healthView;

    public event Action<ITarget> Destroyed;

    public TargetType Type { get; private set; }
    public float Radius => _radius;
    public bool IsFriendly { get; private set; }
    public Transform Transform => transform;

    public void Init(bool isEnemy, TargetType type)
    {
        IsFriendly = isEnemy;
        Type = type;
    }

    private void Awake()
    {
        _healthView = GetComponent<HealthView>();
    }

    private void OnEnable()
    {
        _healthView.HealthOver += OnHealthOver;
    }

    private void OnDisable()
    {
        _healthView.HealthOver -= OnHealthOver;
    }

    private void OnHealthOver()
    {
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public void TakeDamage(int value)
    {
        _healthView.TakeDamage(value);
    }
}
