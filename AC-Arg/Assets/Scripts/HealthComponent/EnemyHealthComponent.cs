using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthComponent : HealthComponent
{
    Enemy _me;
    public override void Start()
    {
        base.Start();
        _me = GetComponent<Enemy>();
    }

    public override void Die()
    {
        Debug.Log(gameObject.name + " died");
        _me.OnDeath();
    }
}
