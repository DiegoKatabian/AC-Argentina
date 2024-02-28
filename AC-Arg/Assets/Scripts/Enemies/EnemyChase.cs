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
        Debug.Log("entro a chase");
    }

    public void OnExit()
    {
        Debug.Log("salgo de chase");
    }

    public void OnUpdate()
    {
        if (_me.playerDetection.isPlayerInMeleeRange)
        {
            _fsm.ChangeState(State.EnemyAttack);
        }
        
        if (!_me.playerDetection.isPlayerInFOV)
        {
            _fsm.ChangeState(State.EnemyIdle);
        }

        ChasePlayer();
    }

    void ChasePlayer()
    {
        //_me.navMeshAgent.SetDestination(_me.playerDetection.playerTransform.position);
        _me.transform.position = Vector3.MoveTowards(_me.transform.position, EnemyManager.Instance.player.transform.position, _me.moveSpeed * Time.deltaTime);
    }
}
