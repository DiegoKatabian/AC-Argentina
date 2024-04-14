using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : IState
{
    FiniteStateMachine _fsm;
    Enemy _me;
    Transform[] _waypoints;
    int currentWaypoint;

    public EnemyPatrol(FiniteStateMachine fsm, Enemy enemy, Transform[] waypoints)
    {
        _fsm = fsm;
        _me = enemy;
        _waypoints = waypoints;
    }

    public void OnEnter()
    {
        //get next waypoint, set destination towards it
        Debug.Log("PATROL: enter");
        _me.navMeshAgent.SetDestination(_waypoints[currentWaypoint].position);
        _me.animator.CrossFade("Patrol", 0.2f);
    }

    public void OnExit()
    {
        Debug.Log("PATROL: exit");
    }

    public void OnUpdate()
    {
        if (_me.isHurting)
        {
            _fsm.ChangeState(State.EnemyHurt);
        }

        if (_me.navMeshAgent.remainingDistance <= 0.1f)
        {
            Debug.Log("PATROL: llegue a destino");
            int nextWaypointIndex = (currentWaypoint + 1) % _waypoints.Length;
            currentWaypoint = nextWaypointIndex;
            _me.navMeshAgent.SetDestination(_waypoints[currentWaypoint].position);
        }

        if (StealthManager.Instance.currentStatus.status == StealthStatus.Hidden)
        {
            //Debug.Log("patrol update: el player esta hidden asi que ni pregunto de perseguirlo");
            return;
        }

        if (_me.playerDetection.isPlayerInFOV)
        {
            Debug.Log("patrol update: me paso a chase xq el player esta en fov");
            _fsm.ChangeState(State.EnemyChase);
        }
    }

}
