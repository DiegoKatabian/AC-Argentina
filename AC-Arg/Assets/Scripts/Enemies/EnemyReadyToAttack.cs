using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReadyToAttack : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    float timer = 0;
    float initialAttackCooldown = 1.5f; //cuanto espera desde que entra a este state hasta q ataca

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
        if (_me.isHurting)
        {
            _fsm.ChangeState(State.EnemyHurt);
        }

        if (StealthManager.Instance.currentStatus.status == StealthStatus.Hidden)
        {
            _fsm.ChangeState(State.EnemyIdle);
        }

        if (!_me.playerDetection.isPlayerInMeleeRange)
        {
            _fsm.ChangeState(State.EnemyChase);
        }

        timer += Time.deltaTime;

        if (timer >= initialAttackCooldown)
        {
            if (EnemyManager.Instance.CanIAttackPlayerMisterEnemyManager(_me))
            {
                _fsm.ChangeState(State.EnemyAttack);
            }
        }
    }
}
