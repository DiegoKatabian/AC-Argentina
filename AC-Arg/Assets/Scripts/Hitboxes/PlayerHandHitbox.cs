using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandHitbox : Hitbox
{
    public Enemy affectedEnemy;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToHit))
        {
            isTaggedInside = true;
            affectedEnemy = other.GetComponent<Enemy>();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToHit))
        {
            isTaggedInside = false;
            affectedEnemy = null;
        }
    }
}
