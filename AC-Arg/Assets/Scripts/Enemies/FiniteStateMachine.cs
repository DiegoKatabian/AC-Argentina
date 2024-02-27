using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    EnemyIdle,
    EnemyPatrol,
    EnemyChase,
    EnemyWait,
    EnemyAttack
}
public class FiniteStateMachine
{
    IState _currentState;
    Dictionary<State, IState> allStates = new Dictionary<State, IState>();

    public void Update()
    {
        _currentState.OnUpdate();
    }
    public void ChangeState(State state)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }
        _currentState = allStates[state];
        _currentState.OnEnter();
    }
    public void AddState(State key, IState value)
    {
        if (!allStates.ContainsKey(key))
        {
            allStates.Add(key, value);
        }
        else
        {
            allStates[key] = value;
        }
    }
}
