using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPositionTrigger : AreaTriggers
{
    [SerializeField] Transform spawnPoint;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.Trigger(eventToTrigger, spawnPoint.position);
        }
    }
}
