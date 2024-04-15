using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianCrash : IState
{
    private Pedestrian _me;
    FiniteStateMachine _fsm;
    public PedestrianCrash(Pedestrian pedestrian, FiniteStateMachine fsm)
    {
        _me = pedestrian;
        _fsm = fsm;
    }
    public void OnEnter()
    {
        //Debug.Log("ped: entro a crash");
        _me.animator.CrossFade("Crash", 0.1f);

        Vector3 direction = _me.vehiclePosition - _me.transform.position;
        direction.y = 0;
        _me.transform.rotation = Quaternion.RotateTowards(_me.transform.rotation, Quaternion.LookRotation(direction), 180);
        _me.transform.position = Vector3.MoveTowards(_me.transform.position, _me.transform.position - direction, 5);
    }

    public void OnExit()
    {
        //Debug.Log("ped: salgo de crash");
    }

    public void OnUpdate()
    {
        if (_me.isDead)
        {
            return;
        }

        if (!_me.isCrashed)
        {
            _fsm.ChangeState(State.PedestrianIdle);
        }

    }

}
