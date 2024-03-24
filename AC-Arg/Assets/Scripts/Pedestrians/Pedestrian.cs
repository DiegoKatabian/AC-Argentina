using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour, ICrashable
{
    public NavMeshAgent navMeshAgent;
    protected FiniteStateMachine _fsm;
    public Animator animator;
    public HealthComponent healthComponent;
    public bool isWalking = false;
    [HideInInspector] public bool isShoved = false;
    [HideInInspector] public bool isCrashed = false;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public Vector3 playerPosition;
    [HideInInspector] public Vector3 vehiclePosition;
    [HideInInspector] public float _crashForce = 0;

    private void Start()
    {
        //Debug.Log("pedestrian start");
        _fsm = new FiniteStateMachine();
        _fsm.AddState(State.PedestrianIdle, new PedestrianIdle(this, _fsm));
        _fsm.AddState(State.PedestrianWalk, new PedestrianWalk(this, _fsm));
        _fsm.AddState(State.PedestrianShove, new PedestrianShove(this, _fsm));
        _fsm.AddState(State.PedestrianCrash, new PedestrianCrash(this, _fsm));
        _fsm.AddState(State.PedestrianDie, new PedestrianDie(this, _fsm));
        _fsm.ChangeState(State.PedestrianIdle);

        //isWalking = true;
    }

    private void Update()
    {
        _fsm.Update();
    }

    //SHOVED
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("pedestrian hit player");
            playerPosition = other.gameObject.transform.position;
            isShoved = true;
        }

    }
    public void ANIMATION_OnShoveEnd()
    {
        isShoved = false;
    }


    //CRASHED
    public void OnCrash(GameObject vehicle, float crashForce)
    {
        if (isCrashed)
            return;

        isCrashed = true;
        Debug.Log("pedestrian on crash");
        vehiclePosition = vehicle.transform.position;
        _crashForce = crashForce;

    }
    public void ANIMATION_OnCrashEnd()
    {
        healthComponent.TakeDamage(_crashForce);
        isCrashed = false;
    }

    internal void OnDeath()
    {
        isDead = true;
    }

    public void ANIMATION_OnDeathEnd()
    {
        isDead = false;
        Destroy(gameObject);
    }
}
