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
        //Debug.Log("entro a chase");
        //invoke chaseplayer repeating
        _me.StartChasingPlayer();
    }

    public void OnExit()
    {
        //Debug.Log("salgo de chase");
    }

    public void OnUpdate()
    {
        //Debug.Log("chase update");
        if (_me.playerDetection.isPlayerInMeleeRange)
        {
            //Debug.Log("che me paso a attack porque lo tengo al lado");
            _fsm.ChangeState(State.EnemyAttack);
        }
        
        if (!_me.playerDetection.isPlayerInFOV)
        {
            //Debug.Log("che me vuelvo a idle xq no veo al player");
            _fsm.ChangeState(State.EnemyIdle);
        }
    }

}
