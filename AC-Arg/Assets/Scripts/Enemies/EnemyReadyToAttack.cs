using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReadyToAttack : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    float timer = 0;
    float stateChangeTimeThreshold = 1.5f;

    public EnemyReadyToAttack(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }


    public void OnEnter()
    {
        //Debug.Log("entro a ready to attack");
        _me.navMeshAgent.SetDestination(_me.transform.position); //me quedo en el lugar
        _me.navMeshAgent.isStopped = true;
        _me.animator.CrossFade("ReadyToAttack", 0.2f);

        timer = 0;
    }

    public void OnExit()
    {
        //Debug.Log("salgo de ready to attack");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= stateChangeTimeThreshold)
        {
            if (!_me.playerDetection.isPlayerInMeleeRange)
            {
                _fsm.ChangeState(State.EnemyChase);
            }

            if (_me.isHurting)
            {
                _fsm.ChangeState(State.EnemyHurt);
            }

            if (EnemyManager.Instance.CanIAttackPlayerMisterEnemyManager(_me))
            {
                _fsm.ChangeState(State.EnemyAttack);
            }
        }
    }
}
