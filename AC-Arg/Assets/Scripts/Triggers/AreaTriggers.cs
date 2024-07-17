using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggers : MonoBehaviour
{
    [SerializeField] protected Evento eventToTrigger;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.Trigger(eventToTrigger);
        }
    }
}
