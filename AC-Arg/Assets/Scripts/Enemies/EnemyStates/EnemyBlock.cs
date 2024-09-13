using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlock : IState
{
    //enemyblock es comom readyto attack, pero que bloquea en de mientras
    //ready to attack -> block -> ready to attack -> attack


    //TODO: ANDA TODO PERO VA A ATACAR DE UNA. ANTES DEBERIA ESPERAR UN TIEMPO = BLOCKDURATION


    FiniteStateMachine _fsm;
    BlockingEnemy _me;

    float _timer;
    float _initialAttackCooldown; //cuanto espera desde que entra a este state hasta q ataca
    float _blockTimer;


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
        _timer = 0;
        _blockTimer = 0;
    }

    public void OnExit()
    {
        //Debug.Log("salgo de block");
        _me.isBlocking = false;

    }

    public void OnUpdate()
    {
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

        //rotate towards player
        Vector3 direction = EnemyManager.Instance.GetDirectionToPlayer(_me.transform.position);
        direction.y = 0;
        _me.transform.rotation = Quaternion.Slerp(_me.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);


        if (_blockTimer < _me.blockDuration)
        {
            _blockTimer += Time.deltaTime;
            return;
        }

        _timer += Time.deltaTime;

        if (_timer >= _initialAttackCooldown)
        {
            if (EnemyManager.Instance.CanIAttackPlayerMisterEnemyManager(_me))
            {
                _fsm.ChangeState(State.EnemyAttack);
            }
        }
    }
}
