using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianShove : IState
{
    private Pedestrian _me;
    FiniteStateMachine _fsm;

    public PedestrianShove(Pedestrian pedestrian, FiniteStateMachine fsm)
    {
        _me = pedestrian;
        _fsm = fsm;
    }
    public void OnEnter()
    {
        //Debug.Log("ped: entro a shove");
        _me.animator.CrossFade("Shoved", 0.1f);
        
        Vector3 direction = _me.playerPosition - _me.transform.position;
        direction.y = 0;
        _me.transform.rotation = Quaternion.RotateTowards(_me.transform.rotation, Quaternion.LookRotation(direction), 180);
        _me.transform.position = Vector3.MoveTowards(_me.transform.position, _me.transform.position - direction, 5);
    }

    public void OnExit()
    {
        //Debug.Log("ped: salgo de shove");
    }

    public void OnUpdate()
    {
        if (!_me.isShoved)
        {
            _fsm.ChangeState(State.PedestrianIdle);
        }

        if (_me.isDead)
        {
            _fsm.ChangeState(State.PedestrianDie);
        }
    }

}

