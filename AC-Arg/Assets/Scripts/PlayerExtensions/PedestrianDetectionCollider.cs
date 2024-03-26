using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianDetectionCollider : DetectionCollider
{
    public PedestrianInteractionController pedestrianController;

    private void Start()
    {
        EventManager.Subscribe(Evento.OnPedestrianKilled, RemovePedestrian);
    }

    public override void OnTagDetectedEnter(Collider other)
    {
        if (other.GetComponent<Pedestrian>() != null)
        {
            Pedestrian detectedPedestrian = other.GetComponent<Pedestrian>();
            pedestrianController.AddPedestrianToDetectedList(detectedPedestrian);
        }
    }

    public override void OnTagDetectedExit(Collider other)
    {
        if (other.GetComponent<Pedestrian>() != null)
        {
            Pedestrian detectedPedestrian = other.GetComponent<Pedestrian>();
            pedestrianController.RemovePedestrianFromDetectedList(detectedPedestrian);
        }
    }
    private void RemovePedestrian(object[] parameters)
    {
        pedestrianController.RemovePedestrianFromDetectedList((Pedestrian)parameters[0]);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
            EventManager.Unsubscribe(Evento.OnPedestrianKilled, RemovePedestrian);
    }

}
