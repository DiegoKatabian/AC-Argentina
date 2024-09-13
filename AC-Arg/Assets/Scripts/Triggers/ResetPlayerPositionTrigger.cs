using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPositionTrigger : AreaTriggers
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnManager.Instance.TriggerPlayerResetPosition();
        }
    }
}
