using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    public EnemyHurt(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }

    public void OnEnter()
    {
        //Debug.Log("entro a hurt");
        _me.navMeshAgent.SetDestination(_me.transform.position); //me quedo en el lugar
        _me.navMeshAgent.isStopped = true;
        _me.animator.CrossFade("Hurt", 0.05f);
        _me.isHurting = false;

    }

    public void OnExit()
    {
        //Debug.Log("salgo de hurt");
        _me.finishedHurting = false;
        EnemyManager.Instance.RotateTowardsPlayer(_me);
    }

    public void OnUpdate()
    {
        if (_me.finishedHurting)
        {
            _fsm.ChangeState(State.EnemyReadyToAttack);
        }

        if (_me.isHurting)
        {
            ResetState();
        }
    }

    public void ResetState()
    {   
        _me.finishedHurting = true;
        _fsm.ChangeState(State.EnemyHurt);
    }
}
