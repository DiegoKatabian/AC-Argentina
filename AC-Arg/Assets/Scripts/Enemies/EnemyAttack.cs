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
        //Debug.Log("entro a attack");
        _me.EnableObject(_me.isAttackingMarker, true);

        //set destination to null, stay in place
        _me.navMeshAgent.SetDestination(_me.transform.position);
    }

    public void OnExit()
    {
        //Debug.Log("salgo de attack");
        _me.EnableObject(_me.isAttackingMarker, false);
        _me.finishedAttacking = false;
    }

    public void OnUpdate()
    {
        if (!_me.playerDetection.isPlayerInMeleeRange)
        {
            _fsm.ChangeState(State.EnemyChase);
        }

        if (!_me.isAttacking)
        {
            _me.TryAttack();
        }

        if (_me.finishedAttacking)
        {
            _fsm.ChangeState(State.EnemyIdle);
        }
    }
}
