using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    string[] attackAnimations = new string[] { "Attack1", "Attack2", "Attack3" };

    public EnemyAttack(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }

    public void OnEnter()
    {
        ObjectEnabler.EnableObject(_me.attackingMarker, true);
        _me.StartAttack();
        _me.navMeshAgent.SetDestination(_me.transform.position); //me quedo en el lugar
        _me.navMeshAgent.isStopped = true;
        //_me.animator.CrossFade("Attack", 0.2f);

        int randomAttack = Random.Range(0, attackAnimations.Length);
        Debug.Log("random attack: " + randomAttack);
        _me.animator.CrossFade(attackAnimations[randomAttack], 0.2f);


    }

    public void OnExit()
    {
        ObjectEnabler.EnableObject(_me.attackingMarker, false);
        _me.finishedAttacking = false;
    }

    public void OnUpdate()
    {
        if (_me.isHurting)
        {
            _fsm.ChangeState(State.EnemyHurt);
        }


        if (_me.finishedAttacking)
        {
            _fsm.ChangeState(State.EnemyReadyToAttack);
        }

        //deberia salir de attack una vez que termina su primer ataque
    }
}
