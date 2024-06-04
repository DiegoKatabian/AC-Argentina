using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggers : MonoBehaviour
{
    public Evento eventToTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.Trigger(eventToTrigger);
        }
    }
}
