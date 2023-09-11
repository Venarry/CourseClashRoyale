using System;
using UnityEngine;

public interface ITarget
{
    public float Radius { get; }
    public bool IsFriendly { get; }
    public TargetType Type { get; }
    public Transform Transform { get; }
    public event Action<ITarget> Destroyed;
    public void TakeDamage(int value);
}
