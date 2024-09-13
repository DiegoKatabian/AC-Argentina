using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianWalk : IState
{
    private Pedestrian _me;
    FiniteStateMachine _fsm;
    PedestrianWaypointCollection _waypointCollection;
    public PedestrianWalk(Pedestrian pedestrian, FiniteStateMachine fsm, PedestrianWaypointCollection pwc)
    {
        _me = pedestrian;
        _fsm = fsm;
        _waypointCollection = pwc;
    }
    public void OnEnter()
    {
        //Debug.Log("ped: entro a walk");
        _me.animator.CrossFade("Walk", 0.1f);
        SetNewDestination();
    }

    public void OnExit()
    {
        //Debug.Log("ped: salgo de walk");
        _me.navMeshAgent.ResetPath();
    }

    public void OnUpdate()
    {
        if (!_me.isWalking)
        {
            _fsm.ChangeState(State.PedestrianIdle);
        }

        if (_me.navMeshAgent.remainingDistance <= _me.navMeshAgent.stoppingDistance)
        {
            if (!_me.navMeshAgent.hasPath || _me.navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                while (_me.navMeshAgent.destination == _me.navMeshAgent.pathEndPosition)
                {
                    SetNewDestination();
                }
            }
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

    public void SetNewDestination()
    {
        if (_waypointCollection != null)
        {
            _me.navMeshAgent.SetDestination(_waypointCollection.GetRandomWaypointPosition());
        }
        else
        {
            _me.navMeshAgent.SetDestination(PedestrianManager.Instance.GetRandomWaypoint());
        }
    }
}
