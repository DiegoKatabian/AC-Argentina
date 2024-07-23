using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
    Enemy _me;

    public bool isBoss = false;
    public override void Start()
    {
        base.Start();
        _me = GetComponent<Enemy>();
    }

    override public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (isBoss)
        {
            EventManager.Instance.Trigger(Evento.OnVaruzhanHealthUpdate, currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    } 

    public override void Die()
    {
        Debug.Log(gameObject.name + " died");
        if (isBoss)
        {
            EventManager.Instance.Trigger(Evento.OnVaruzhanDie);
        }
        _me.OnDeath();
    }
}
