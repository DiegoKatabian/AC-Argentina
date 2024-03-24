using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianIdle : IState
{
    private Pedestrian _me;
    FiniteStateMachine _fsm;
    public PedestrianIdle(Pedestrian pedestrian, FiniteStateMachine fsm)
    {
        _me = pedestrian;
        _fsm = fsm;
    }
    public void OnEnter()
    {
        Debug.Log("ped: entro a idle");
        _me.animator.CrossFade("Idle", 0.1f);

    }

    public void OnExit()
    {
        Debug.Log("ped: salgo de idle");
    }

    public void OnUpdate()
    {
        if (_me.isWalking)
        {
            _fsm.ChangeState(State.PedestrianWalk);
        }

        if (_me.isShoved)
        {
            _fsm.ChangeState(State.PedestrianShove);
        }

        if (_me.isCrashed)
        {
            _fsm.ChangeState(State.PedestrianCrash);
        }

        if (_me.isDead)
        {
            _fsm.ChangeState(State.PedestrianDie);
        }
    }
}
