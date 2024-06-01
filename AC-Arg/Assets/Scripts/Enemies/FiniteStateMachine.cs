using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    EnemyIdle,
    EnemyChase,
    EnemyReadyToAttack,
    EnemyAttack,
    EnemyHurt,
    EnemyPatrol,
    PedestrianIdle,
    PedestrianWalk,
    PedestrianShove,
    PedestrianCrash,
    PedestrianDie,
    EnemyKnockedOut,
    EnemyDead
}
public class FiniteStateMachine
{
    public IState _currentState;
    protected Dictionary<State, IState> allStates = new Dictionary<State, IState>();

    public void Update()
    {
        _currentState.OnUpdate();
    }
    public virtual void ChangeState(State state)
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
