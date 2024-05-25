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

        if (_me.isPatroller)
        {
            //Debug.Log("IDLE: me insta paso a patrol");
            _fsm.ChangeState(State.EnemyPatrol);
        }
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

        if (_me.isKnockedOut)
        {
            //Debug.Log("me paso a knocked out");
            _fsm.ChangeState(State.EnemyKnockedOut);
        }

        if (StealthManager.Instance.currentStatus.status != StealthStatus.Hidden)
        {
            LookForPlayer();
        }
    }

    public void LookForPlayer()
    {
        //Debug.Log("se busca player...");

        if (!_me.playerDetection.isPlayerInFOV)
        {
            //Debug.Log("el player no esta en fov");
            return;
        }

        if (_me.isKnockedOut)
        {
            //Debug.Log("no hago nada porque estoy noqueado");
            return;
        }

        if (_me.chasesPlayerOnlyWhileWarning && 
            !(StealthManager.Instance.currentStatus.status == StealthStatus.Warning ||
                                   StealthManager.Instance.currentStatus.status == StealthStatus.Alert))
        {
            //Debug.Log("soy pasivo y no hay alerta");
            return;
        }

        //Debug.Log("persigo");
        _fsm.ChangeState(State.EnemyChase);
    }

}
