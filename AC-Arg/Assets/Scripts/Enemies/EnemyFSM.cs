using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State
{
    EnemyIdle,
    EnemyChase,
    EnemyReadyToAttack,
    EnemyAttack
}
public class EnemyFSM : FiniteStateMachine
{
    public override void ChangeState(State state)
    {
        base.ChangeState(state);
        //Debug.Log("aviso al enemy manager mi nuevo state");
        EnemyManager.Instance.UpdateEnemyState(this, _currentState);
    }
}
