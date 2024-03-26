using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianWalk : IState
{
    private Pedestrian _me;
    FiniteStateMachine _fsm;
    public PedestrianWalk(Pedestrian pedestrian, FiniteStateMachine fsm)
    {
        _me = pedestrian;
        _fsm = fsm;
    }
    public void OnEnter()
    {
        //Debug.Log("ped: entro a walk");
        _me.animator.CrossFade("Walk", 0.1f);
        _me.navMeshAgent.SetDestination(PedestrianManager.Instance.GetRandomWaypointPosition());
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
                    _me.navMeshAgent.SetDestination(PedestrianManager.Instance.GetRandomWaypointPosition());
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
}
