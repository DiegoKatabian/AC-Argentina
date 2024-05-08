using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PedestrianInteractionController : MonoBehaviour
{
    public IPedestrian currentPedestrian;
    public List<IPedestrian> detectedPedestrians = new List<IPedestrian>();
    public bool arePedestriansDetected = false;
    public bool isAlreadyInteracting = false;

    internal void AddPedestrianToDetectedList(IPedestrian detectedPedestrian)
    {
        if (!detectedPedestrians.Contains(detectedPedestrian))
        {
            detectedPedestrians.Add(detectedPedestrian);
            UpdateDetectionStatus(detectedPedestrian);
        }
    }

    private void UpdateDetectionStatus(IPedestrian lastDetectedPedestrian)
    {
        if (detectedPedestrians.Count == 0)
        {
            // No hay peatones detectados
            arePedestriansDetected = false;
            SetCurrentPedestrian(null);
        }
        else
        {
            // Hay peatones detectados
            arePedestriansDetected = true;

            // Si el currentPedestrian es nulo, establece el último peatón detectado como currentPedestrian
            if (currentPedestrian == null)
            {
                SetCurrentPedestrian(lastDetectedPedestrian);
            }
            else
            {
                float distanceToLast = Vector3.Distance(transform.position, lastDetectedPedestrian.gameObject.transform.position);
                float distanceToCurrent = Vector3.Distance(transform.position, currentPedestrian.gameObject.transform.position);

                if (distanceToLast < distanceToCurrent)
                {
                    SetCurrentPedestrian(lastDetectedPedestrian);
                }
            }
        }
    }

    internal void RemovePedestrianFromDetectedList(IPedestrian detectedPedestrian)
    {
        detectedPedestrians.Remove(detectedPedestrian);
        UpdateDetectionStatus(detectedPedestrian);
    }

    public void SetCurrentPedestrian(IPedestrian pedestrian)
    {
        SetPedestrianMarker(currentPedestrian, false);
        currentPedestrian = pedestrian;
        SetPedestrianMarker(currentPedestrian, true);
    }

    public void SetPedestrianMarker(IPedestrian pedestrian, bool state)
    {
        if (pedestrian != null)
        {
            pedestrian.SetInteractionMarkerActive(state);
        }
    }

    public bool IsBehindCurrentPedestrian()
    {
        if (currentPedestrian == null)
        {
            Debug.Log("no hay pedestrians");
            return false;
        }

        Vector3 pedestrianForward = currentPedestrian.gameObject.transform.forward;
        Vector3 pedestrianToPlayer = transform.position - currentPedestrian.gameObject.transform.position;
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
