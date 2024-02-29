using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;

    public EnemyIdle(FiniteStateMachine fsm, Enemy enemy)
    {
        _fsm = fsm;
        _me = enemy;
    }


    public void OnEnter()
    {
        //Debug.Log("entro a idle");
        _me.CancelChasePlayer();
    }

    public void OnExit()
    {
        //Debug.Log("salgo de idle");
    }

    public void OnUpdate()
    {
        if (_me.playerDetection.isPlayerInFOV)
        {
            _fsm.ChangeState(State.EnemyChase);
        }
    }

}
