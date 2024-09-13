using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapZoneTrigger : AreaTriggers
{
    [SerializeField] protected Evento eventToTriggerOnExit;

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("sali de la leapzone");
            EventManager.Instance.Trigger(eventToTriggerOnExit);
        }
    }
}
