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

    public virtual void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " died");
        Destroy(gameObject);
    }


}