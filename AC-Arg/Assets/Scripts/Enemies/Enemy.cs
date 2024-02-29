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

    public virtual void TryAttack()
    {
        Debug.Log("base enemy try attack");
    }

    public virtual void StartChasingPlayer()
    {
        Debug.Log("base start chasing player");
    }

    public virtual void CancelChasePlayer()
    {
        Debug.Log("base cancel chase player");
    }
}
