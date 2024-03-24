using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianDie : IState
{
    private Pedestrian _me;
    FiniteStateMachine _fsm;
    public PedestrianDie(Pedestrian pedestrian, FiniteStateMachine fsm)
    {
        _me = pedestrian;
        _fsm = fsm;
    }
    public void OnEnter()
    {
        Debug.Log("ped: entro a die");
        _me.animator.CrossFade("Die", 0.1f);
    }

    public void OnExit()
    {
        Debug.Log("ped: salgo de die");
    }

    public void OnUpdate()
    {
    }
}