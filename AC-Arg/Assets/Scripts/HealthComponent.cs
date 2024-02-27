using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 6;
    public float currentHealth;

    public virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(object[] parameters)
    {
        Debug.Log("take damage = 1");
        TakeDamage(1);
    }

    public virtual void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log("you died");
    }


}