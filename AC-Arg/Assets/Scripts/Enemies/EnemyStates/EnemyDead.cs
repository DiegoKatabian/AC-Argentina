using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;
    float destroyTime = 20;

    public EnemyDead(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }

    public void OnEnter()
    {
        //Debug.Log("entro a die");

        _me.navMeshAgent.SetDestination(_me.transform.position); //me quedo en el lugar
        _me.navMeshAgent.isStopped = true;
        _me.animator.CrossFade("Die", 0.05f);
        _me.isHurting = false;
    }

    public void OnExit()
    {
        Debug.Log("salgo de die");
    }

    public void OnUpdate()
    {
        Debug.Log("pasa el tiempo...");
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {
            Debug.Log("me deshago del cadaver...");

            _me.gameObject.SetActive(false);
        }
    }
}
