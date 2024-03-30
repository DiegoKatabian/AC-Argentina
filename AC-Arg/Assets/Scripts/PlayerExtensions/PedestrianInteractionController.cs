using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PedestrianInteractionController : MonoBehaviour
{
    public Pedestrian currentPedestrian;
    public List<Pedestrian> detectedPedestrians = new List<Pedestrian>();
    public bool arePedestriansDetected = false;
    public bool isAlreadyInteracting = false;

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
            if (!arePedestriansDetected) //si ya habia pedestrians detected, no se sobreescribe
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
        SetPedestrianMarker(currentPedestrian, false);
        currentPedestrian = pedestrian;
        SetPedestrianMarker(currentPedestrian, true);
    }

    public void SetPedestrianMarker(Pedestrian pedestrian, bool state)
    {
        if (pedestrian != null)
        {
            pedestrian.interactionMarker.SetActive(state);
        }
    }

    public bool IsBehindCurrentPedestrian()
    {
        if (currentPedestrian == null)
        {
            Debug.Log("no hay pedestrians");
            return false;
        }

        Vector3 pedestrianForward = currentPedestrian.transform.forward;
        Vector3 pedestrianToPlayer = transform.position - currentPedestrian.transform.position;
        pedestrianForward.y = 0;
        pedestrianToPlayer.y = 0;
        if (Vector3.Angle(pedestrianForward, pedestrianToPlayer) < 90)
        {
            Debug.Log("not behind pedestrian");
            return false;
        }
        else
        {
            Debug.Log("behind pedestrian");
            return true;
        }
    }
}
