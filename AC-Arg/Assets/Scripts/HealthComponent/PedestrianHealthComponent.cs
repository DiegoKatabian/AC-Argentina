using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianHealthComponent : HealthComponent
{

    Pedestrian _me;
    public override void Start()
    {
        base.Start();
        _me = GetComponent<Pedestrian>();
    }

    public override void Die()
    {
        _me.OnDeath();
    }
}