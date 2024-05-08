using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour, ICrashable, IPedestrian
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


    public GameObject interactionMarker;    
    public bool canInteract = true;

    public ParticleSystem bloodParticles;
    public float timeUntilTriggerParticles = 1f;

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

        if (isWalking)
        {
            StartCoroutine(RandomWalk());
        }

    }

    public IEnumerator RandomWalk()
    {
        float randomWaitTime = UnityEngine.Random.Range(0f, 1f);
        isWalking = false;
        //Debug.Log("i waited " + randomWaitTime + "before walking");
        yield return new WaitForSeconds(randomWaitTime);
        isWalking = true;
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
            //Debug.Log("pedestrian hit player");
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
        //Debug.Log("pedestrian on crash");
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

    public void GetAssassinated(GameObject assassin)
    {
        Debug.Log("pedestrian: me asesinaron");
        healthComponent.TakeDamage(100);
        PedestrianManager.Instance.TriggerPedestrianAlarm(this);
        StartCoroutine(CoroutineUtilities.DelayedAction(timeUntilTriggerParticles, TriggerParticles));
    }

    public void TriggerParticles(object[] obj)
    {
        bloodParticles.Play();
    }

    public void GetStolen()
    {
        Debug.Log("pedestrian: me robaron!");
    }

    public void SetInteractionMarkerActive(bool active)
    {
        interactionMarker.SetActive(active);
    }

    public bool CanInteract()
    {
        return canInteract;
    }
}
