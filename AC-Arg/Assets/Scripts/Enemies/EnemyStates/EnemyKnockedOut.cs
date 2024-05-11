using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockedOut : IState
{
    private EnemyFSM _fsm;
    private UnarmedEnemy _enemy;
    private float _knockoutTimer = 0f;

    public EnemyKnockedOut(EnemyFSM fsm, UnarmedEnemy enemy)
    {
        _fsm = fsm;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        _knockoutTimer = 0f;
        _enemy.navMeshAgent.isStopped = true;
        _enemy.animator.SetBool("KnockedOut", true);
    }

    public void OnExit()
    {
        _enemy.animator.SetBool("KnockedOut", false);
        _enemy.isKnockedOut = false;
        _enemy.navMeshAgent.isStopped = false;
    }

    public void OnUpdate()
    {
        _knockoutTimer += Time.deltaTime;
        if (_knockoutTimer >= _enemy.knockoutTime)
        {
            _fsm.ChangeState(State.EnemyIdle);
        }
    }

}
