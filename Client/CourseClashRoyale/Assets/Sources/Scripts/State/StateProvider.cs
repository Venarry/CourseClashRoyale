using System;
using System.Collections.Generic;

public class StateProvider
{
    private readonly Dictionary<Type, IState> _states = new();

    public void Register(IState state)
    {
        _states[state.GetType()] = state;
    }

    public T GetState<T>() where T : IState =>
        (T)_states[typeof(T)];
}
