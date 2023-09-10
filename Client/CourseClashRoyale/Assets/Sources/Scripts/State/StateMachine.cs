using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState _currentState;
    private readonly Dictionary<Type, IState> _states = new();

    public void Register(IState state)
    {
        _states[state.GetType()] = state;
    }

    public void Change<T>() where T : IState
    {
        Type type = typeof(T);

        if (_currentState?.GetType() == type)
        {
            return;
        }

        if (_states.TryGetValue(type, out IState state))
        {
            _currentState?.ExitState();
            _currentState = state;
        }

        _currentState = state;
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState?.UpdateState();
    }
}
