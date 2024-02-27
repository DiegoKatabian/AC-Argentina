using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 6;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        EventManager.Subscribe(Evento.OnPlayerPressedE, TakeDamage);
    }

    private void TakeDamage(object[] parameters)
    {
        Debug.Log("take damage = 1");
        TakeDamage(1);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        EventManager.Trigger(Evento.OnPlayerHealthUpdate, currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("you died");
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnPlayerPressedE, TakeDamage);
        }
    }
}