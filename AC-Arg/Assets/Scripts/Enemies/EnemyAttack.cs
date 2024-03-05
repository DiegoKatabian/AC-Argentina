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
        ObjectEnabler.EnableObject(_me.isAttackingMarker, true);
        _me.StartAttack();
        _me.navMeshAgent.SetDestination(_me.transform.position); //me quedo en el lugar
        _me.navMeshAgent.isStopped = true;
    }

    public void OnExit()
    {
        ObjectEnabler.EnableObject(_me.isAttackingMarker, false);
        _me.finishedAttacking = false;
    }

    public void OnUpdate()
    {
        if (_me.finishedAttacking)
        {
            _fsm.ChangeState(State.EnemyIdle);
        }

        //deberia salir de attack una vez que termina su primer ataque
    }
}
