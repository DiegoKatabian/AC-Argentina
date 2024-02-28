using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PlayerDetection playerDetection;
    public float moveSpeed = 5;
    public float attackRecoveryTime = 2;
    public float attackDamage = 1;

    public virtual void TryAttack()
    {
        Debug.Log("base enemy try attack");
    }
}
