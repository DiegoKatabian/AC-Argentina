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
        _me.animator.CrossFade("Idle", 0.2f);

    }

    public void OnExit()
    {
        //Debug.Log("salgo de idle");
    }

    public void OnUpdate()
    {

        if (_me.isHurting)
        {
            _fsm.ChangeState(State.EnemyHurt);
        }

        if (_me.playerDetection.isPlayerInFOV &&
            StealthManager.Instance.currentStatus.status != StealthStatus.Hidden)
        {
            _fsm.ChangeState(State.EnemyChase);
        }
    }

}
