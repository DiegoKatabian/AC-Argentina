using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public float moveSpeed = 5;
    //public float attackRecoveryTime = 2; //este es un menitra y no se usa
    public float attackDamage = 1;
    public NavMeshAgent navMeshAgent;
    protected EnemyFSM _fsm;
    public Animator animator;

    internal bool isMyTurnToAttack = false; //true solo cuando sea mi turno de atacar
    public bool isAttacking = false; //true durante attack state
    public bool finishedAttacking = false; //solo true cuando salgo de attack state
    public bool isHurting = false; //true durante hurt state
    public bool finishedHurting = false; //solo true cuando salgo de hurt state
    public GameObject isAttackingMarker; //el cosito rojo en la cabeza del enemy, indica que esta isAttacking
    public GameObject isCurrentEnemyMarker; //el cosito blanco en la cabeza del enemy, indica que es el current enemy
    [HideInInspector] public bool isRotating;
    [HideInInspector] public float rotationTime = 1;

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
        Debug.Log("base enemy on death");
    }

    public virtual void StartHurt()
    {
        Debug.Log("enemy: i was hit");
        
    }
}
