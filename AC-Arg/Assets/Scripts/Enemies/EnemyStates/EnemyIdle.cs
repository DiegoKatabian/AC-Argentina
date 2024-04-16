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

        if (StealthManager.Instance.currentStatus.status == StealthStatus.Hidden)
        {
            //Debug.Log("idle update: el player esta hidden asi que ni pregunto de perseguirlo");
            return;
        }

        if (_me.playerDetection.isPlayerInFOV)
        {
            //Debug.Log("idle update: el player esta en fov");
            if (_me.chasesPlayerOnlyWhileWarning)
            {
                if (StealthManager.Instance.currentStatus.status == StealthStatus.Warning ||
                                       StealthManager.Instance.currentStatus.status == StealthStatus.Alert)
                {
                    _fsm.ChangeState(State.EnemyChase);
                }
            }
            else
            {
                _fsm.ChangeState(State.EnemyChase);
            }
        }
    }

}
