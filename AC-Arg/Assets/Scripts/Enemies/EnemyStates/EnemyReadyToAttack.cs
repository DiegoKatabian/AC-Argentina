using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReadyToAttack : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    float timer = 0;
    float initialAttackCooldown; //cuanto espera desde que entra a este state hasta q ataca

    public EnemyReadyToAttack(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
        initialAttackCooldown = _me.initialAttackCooldown;
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
        //si soy blockeador, me paso directo a blockstate

        if (_me.isBlocker)
        {
            _fsm.ChangeState(State.EnemyBlock);
        }


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

        // Verifica si el enemigo está demasiado cerca del jugador
        if (EnemyManager.Instance.GetDistanceToPlayer(_me.transform.position) < _me.minimumDistanceToPlayer)
        {
            Vector3 directionAwayFromPlayer = -EnemyManager.Instance.GetDirectionToPlayer(_me.transform.position);
            directionAwayFromPlayer.Normalize();
            Vector3 destination = _me.transform.position + directionAwayFromPlayer * _me.minimumDistanceToPlayer;
            _me.navMeshAgent.SetDestination(destination);
            _me.navMeshAgent.isStopped = false;
        }
        else
        {
            _me.navMeshAgent.isStopped = true;
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
