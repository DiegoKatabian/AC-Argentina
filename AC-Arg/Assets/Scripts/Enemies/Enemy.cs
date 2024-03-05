using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public float moveSpeed = 5;
    public float attackRecoveryTime = 2;
    public float attackDamage = 1;
    public NavMeshAgent navMeshAgent;
    protected EnemyFSM _fsm;

    internal bool isMyTurnToAttack = false; //true solo cuando sea mi turno de atacar
    public bool isAttacking = false; //true durante attack state
    public bool finishedAttacking = false; //solo true cuando salgo de attack state
    public GameObject isAttackingMarker; //el cosito en la cabeza del enemy, indica que esta isAttacking

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
}
