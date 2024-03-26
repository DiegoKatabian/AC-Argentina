using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianInteractionController : MonoBehaviour
{
    public Pedestrian currentPedestrian;
    public List<Pedestrian> detectedPedestrians = new List<Pedestrian>();
    public bool arePedestriansDetected = false;

    internal void AddPedestrianToDetectedList(Pedestrian detectedPedestrian)
    {
        if (!detectedPedestrians.Contains(detectedPedestrian))
        {
            detectedPedestrians.Add(detectedPedestrian);
            UpdateDetectionStatus(detectedPedestrian);
            //Debug.Log("Enemy Detected");
        }
    }

    private void UpdateDetectionStatus(Pedestrian lastDetectedPedestrian)
    {

        if (detectedPedestrians.Count == 0)
        {
            //Debug.Log("no hay enemies in volume");
            arePedestriansDetected = false;
            SetCurrentPedestrian(null);
        }
        else
        {
            if (!arePedestriansDetected)
            {
                //Debug.Log("primer enemigo detectado");
                arePedestriansDetected = true;

                if (currentPedestrian == null)
                {
                    SetCurrentPedestrian(lastDetectedPedestrian);
                    //Debug.Log("current enemy = " + currentEnemy.name);
                }
            }
        }
    }

    internal void RemovePedestrianFromDetectedList(Pedestrian detectedPedestrian)
    {
        detectedPedestrians.Remove(detectedPedestrian);
        UpdateDetectionStatus(detectedPedestrian);
    }

    public void SetCurrentPedestrian(Pedestrian pedestrian)
    {
        //CurrentPedestrianMarkerToggler(false);
        currentPedestrian = pedestrian;
        //CurrentPedestrianMarkerToggler(true);
    }
}
