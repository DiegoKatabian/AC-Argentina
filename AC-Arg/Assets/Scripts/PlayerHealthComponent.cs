using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : HealthComponent
{

    public override  void Start()
    {
        base.Start();
        EventManager.Subscribe(Evento.OnPlayerPressedE, TakeDamageOnE);
    }

    public void TakeDamageOnE(object[] parameters)
    {
        Debug.Log("take damage = 1");
        TakeDamage(1);
    }

    public override void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        EventManager.Trigger(Evento.OnPlayerHealthUpdate, currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Debug.Log("you died");
        EventManager.Trigger(Evento.OnPlayerDied);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnPlayerPressedE, TakeDamage);
        }
    }
}

