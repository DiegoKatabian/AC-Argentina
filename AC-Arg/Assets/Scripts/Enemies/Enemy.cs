using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public GameObject isAttackingMarker; //el cosito rojo en la cabeza del enemy, indica que esta isAttacking
    public GameObject isCurrentEnemyMarker; //el cosito blanco en la cabeza del enemy, indica que es el current enemy
    public float moveSpeed = 5;
    public float attackDamage = 1;
    public float minimumDistanceToPlayer = 3f;
    public bool isPatroller = false;    //si es idler o patroller

    protected EnemyFSM _fsm;

    [HideInInspector] public bool isMyTurnToAttack = false; //true solo cuando sea mi turno de atacar
    [HideInInspector] public bool isAttacking = false; //true durante attack state
    [HideInInspector] public bool finishedAttacking = false; //solo true cuando salgo de attack state
    [HideInInspector] public bool isHurting = false; //true durante hurt state
    [HideInInspector] public bool finishedHurting = false; //solo true cuando salgo de hurt state
    [HideInInspector] public bool isRotating;
    [HideInInspector] public float rotationTime = 1;
    [HideInInspector] public bool isDead = false;

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
}
