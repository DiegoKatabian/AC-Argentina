using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianDetectionCollider : DetectionCollider
{
    public PedestrianInteractionController pedestrianController;
    public bool ignoreTag = true;

    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnPedestrianKilled, RemovePedestrian);
    }


    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IPedestrian>() != null)
        {
            IPedestrian detectedPedestrian = other.GetComponent<IPedestrian>();
            pedestrianController.AddPedestrianToDetectedList(detectedPedestrian);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IPedestrian>() != null)
        {
            IPedestrian detectedPedestrian = other.GetComponent<IPedestrian>();
            pedestrianController.RemovePedestrianFromDetectedList(detectedPedestrian);
        }
    }


    private void RemovePedestrian(object[] parameters)
    {
        pedestrianController.RemovePedestrianFromDetectedList((IPedestrian)parameters[0]);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
            EventManager.Instance.Unsubscribe(Evento.OnPedestrianKilled, RemovePedestrian);
    }

    public override void OnTagDetectedEnter(Collider other)
    {
        if (ignoreTag)
        {
            return;
        }
        else
        {
            Debug.Log("detect� algo pero no me importa mucho la verdad");
        }
    }

    public override void OnTagDetectedExit(Collider other)
    {
        if (ignoreTag)
        {
            return;
        }
        else
        {
            Debug.Log("detect� algo pero no me importa mucho la verdad");
        }
    }
}
