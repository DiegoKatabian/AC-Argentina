using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : HealthComponent
{
    public override  void Start()
    {
        base.Start();
    }

    public override void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        EventManager.Instance.Trigger(Evento.OnPlayerHealthUpdate, currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log("you died");
        EventManager.Instance.Trigger(Evento.OnPlayerDied);
    }

}

