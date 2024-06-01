using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingEnemy : UnarmedEnemy
{
    //public override void OnDeath()
    //{
    //    base.OnDeath();
    //    EventManager.Instance.Trigger(Evento.OnVaruzhanDeath);
    //}
    public float blockDuration = 4;

    public override void Start()
    {
        _fsm = new EnemyFSM();
        _fsm.AddState(State.EnemyIdle, new EnemyIdle(_fsm, this));
        _fsm.AddState(State.EnemyPatrol, new EnemyPatrol(_fsm, this, waypoints));
        _fsm.AddState(State.EnemyChase, new EnemyChase(_fsm, this));
        _fsm.AddState(State.EnemyAttack, new EnemyAttack(_fsm, this));
        _fsm.AddState(State.EnemyReadyToAttack, new EnemyReadyToAttack(_fsm, this));
        _fsm.AddState(State.EnemyHurt, new EnemyHurt(_fsm, this));
        _fsm.AddState(State.EnemyKnockedOut, new EnemyKnockedOut(_fsm, this));
        _fsm.AddState(State.EnemyDead, new EnemyDead(_fsm, this));
        _fsm.AddState(State.EnemyBlock, new EnemyBlock(_fsm, this));
        _fsm.ChangeState(State.EnemyIdle);
        EnemyManager.Instance.RegisterEnemy(this, _fsm);
        isDead = false;
    }
}
