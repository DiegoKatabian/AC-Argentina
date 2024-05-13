using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IBoleadorable
{
    public PlayerDetection playerDetection;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GameObject attackingMarker; //el cosito rojo en la cabeza del enemy, indica que esta isAttacking
    public GameObject currentEnemyMarker; //el cosito blanco en la cabeza del enemy, indica que es el current enemy
    public float moveSpeed = 5;
    public float attackDamage = 1;
    public float minimumDistanceToPlayer = 3f;
    public float initialAttackCooldown = 1.5f; //cuanto espera hasta hacer el primer ataque
    public float knockoutTime = 20f; //cuanto tiempo queda KO
    public bool isPatroller = false;    //si es idler o patroller
    public bool chasesPlayerOnlyWhileWarning; //si es true, solo persigue al player si esta en warning. false, persigue al player solo con verlo

    protected EnemyFSM _fsm;

    [HideInInspector] public bool isMyTurnToAttack = false; //true solo cuando sea mi turno de atacar
    [HideInInspector] public bool isAttacking = false; //true durante attack state
    [HideInInspector] public bool finishedAttacking = false; //solo true cuando salgo de attack state
    [HideInInspector] public bool isHurting = false; //true durante hurt state
    [HideInInspector] public bool finishedHurting = false; //solo true cuando salgo de hurt state
    [HideInInspector] public bool isRotating;
    [HideInInspector] public float rotationTime = 1;
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isKnockedOut = false;


    public virtual void Start()
    {
        //Debug.Log("registering");
    }
    public virtual void TryAttack()
    {
        //Debug.Log("base enemy try attack");
    }

    public virtual void StartAttack()
    {
        //Debug.Log("base enemy start attack");
    }

    public virtual void StartChasingPlayer()
    {
        //Debug.Log("base start chasing player");
    }

    public virtual void CancelChasePlayer()
    {
        //Debug.Log("base cancel chase player");
    }

    public virtual void OnDeath()
    {
        //Debug.Log("base enemy on death");
    }

    public virtual void StartHurt()
    {
        //Debug.Log("enemy: i was hit");
    }

    public void GetBoleadoraed()
    {
        Debug.Log("me dieron con boleadoras");
        isKnockedOut = true;
        EnemyManager.Instance.TriggerPedestrianAlarm(transform.position);
    }
}
