using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlock : IState
{
    //enemyblock es comom readyto attack, pero que bloquea en de mientras
    //ready to attack -> block -> ready to attack -> attack


    //TODO: ANDA TODO PERO VA A ATACAR DE UNA. ANTES DEBERIA ESPERAR UN TIEMPO = BLOCKDURATION


    FiniteStateMachine _fsm;
    Enemy _me;

    float timer;
    float initialAttackCooldown; //cuanto espera desde que entra a este state hasta q ataca


    public EnemyBlock(FiniteStateMachine fsm, BlockingEnemy blockingEnemy)
    {
        _fsm = fsm;
        _me = blockingEnemy;
    }

    public void OnEnter()
    {
        //Debug.Log("entro a block");
        _me.navMeshAgent.SetDestination(_me.transform.position); //me quedo en el lugar
        _me.navMeshAgent.isStopped = true;
        _me.animator.CrossFade("Block", 0.2f);
        _me.isBlocking = true;
        timer = 0;
    }

    public void OnExit()
    {
        //Debug.Log("salgo de block");
        _me.isBlocking = false;

    }

    public void OnUpdate()
    {
        //if (_me.isHurting)
        //{
        //    _fsm.ChangeState(State.EnemyHurt);
        //}

        //if (StealthManager.Instance.currentStatus.status == StealthStatus.Hidden)
        //{
        //    _fsm.ChangeState(State.EnemyIdle);
        //}

        //if (!_me.playerDetection.isPlayerInMeleeRange)
        //{
        //    _fsm.ChangeState(State.EnemyChase);
        //}

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
