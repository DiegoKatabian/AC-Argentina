using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : IState
{
    FiniteStateMachine _fsm;
    UnarmedEnemy _enemy;

    public EnemyIdle(FiniteStateMachine fsm, UnarmedEnemy enemy)
    {
        _fsm = fsm;
        _enemy = enemy;
    }


    public void OnEnter()
    {
        Debug.Log("entro a idle");
    }

    public void OnExit()
    {
        Debug.Log("salgo de idle");
    }

    public void OnUpdate()
    {

    }

}
