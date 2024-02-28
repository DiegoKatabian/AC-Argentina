using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState
{

    FiniteStateMachine _fsm;
    Enemy _me;

    public EnemyAttack(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }

    public void OnEnter()
    {
        Debug.Log("entro a attack");    
    }

    public void OnExit()
    {
        Debug.Log("salgo de attack");
    }

    public void OnUpdate()
    {
        if (!_me.playerDetection.isPlayerInMeleeRange)
        {
            _fsm.ChangeState(State.EnemyChase);
        }

        _me.TryAttack();
    }
}
