using System;
using System.Collections.Generic;
using UnityEngine;

public class StateManager<T> where T : Enum
{
    private IState<T> _currentState;
    private Dictionary<T, IState<T>> _states;

    public StateManager(Dictionary<T, IState<T>> states)
    {
        _states = states ?? throw new ArgumentNullException(nameof(states));
    }

    public void SetState(T stateId)
    {
        if (_currentState != null)
        {
            _currentState.OnExitState();
        }

        if (_states.TryGetValue(stateId, out var newState))
        {
            _currentState = newState;
            _currentState.OnEnterState();
        }
        else
        {
            Debug.LogError($"State with ID {stateId} not found!");
        }
    }

    public void UpdateState()
    {
        if (_currentState == null) return;

        _currentState.OnUpdateState();

        // 检查是否需要切换状态
        if (_currentState.TransitionState(out var nextStateId))
        {
            SetState(nextStateId);
        }
    }
}
