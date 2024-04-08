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
        _me.StartChasingPlayer();
        _me.animator.CrossFade("Chase", 0.2f);
    }

    public void OnExit()
    {
        _me.CancelChasePlayer();
    }

    public void OnUpdate()
    {
        if (_me.isHurting)
        {
            _fsm.ChangeState(State.EnemyHurt);
        }

        if (StealthManager.Instance.currentStatus.status == StealthStatus.Hidden)
        {
            Debug.Log("me paso a idle xq el player esta hidden");
            _fsm.ChangeState(State.EnemyIdle);
        }

        if (!_me.playerDetection.isPlayerInFOV)
        {
            Debug.Log("me paso a idle xq el player no esta en fov");
            _fsm.ChangeState(State.EnemyIdle);
        }


        if (_me.playerDetection.isPlayerInMeleeRange &&
            StealthManager.Instance.currentStatus.status != StealthStatus.Hidden)
        {
            _fsm.ChangeState(State.EnemyReadyToAttack);
        }
    }

}
