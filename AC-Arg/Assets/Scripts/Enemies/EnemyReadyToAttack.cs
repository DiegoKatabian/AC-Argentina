using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReadyToAttack : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    public EnemyReadyToAttack(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }


    public void OnEnter()
    {
        Debug.Log("entro a ready to attack");
        _me.navMeshAgent.SetDestination(_me.transform.position); //me quedo en el lugar
        _me.navMeshAgent.isStopped = true;
    }

    public void OnExit()
    {
        Debug.Log("salgo de ready to attack");
    }

    public void OnUpdate()
    {
        if (EnemyManager.Instance.CanIAttackPlayerMisterEnemyManager(_me))
        {
            _fsm.ChangeState(State.EnemyAttack);
        }
    }
}
