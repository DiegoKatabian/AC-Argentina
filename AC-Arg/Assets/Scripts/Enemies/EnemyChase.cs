using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    public EnemyChase(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }

    public void OnEnter()
    {
        _me.StartChasingPlayer();
    }

    public void OnExit()
    {
        _me.CancelChasePlayer();
    }

    public void OnUpdate()
    {
        if (_me.playerDetection.isPlayerInMeleeRange)
        {
            _fsm.ChangeState(State.EnemyReadyToAttack);
        }
        
        if (!_me.playerDetection.isPlayerInFOV)
        {
            _fsm.ChangeState(State.EnemyIdle);
        }
    }

}
