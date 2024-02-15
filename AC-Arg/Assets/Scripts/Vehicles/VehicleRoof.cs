using Climbing;
using System.Collections;
using System.Collections.Generic;
using TrafficSimulation;
using UnityEngine;

public class VehicleRoof : MonoBehaviour
{
    public Rigidbody myRB;
    ThirdPersonController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<ThirdPersonController>();
            player.EnterVehicleRoof(myRB);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.ExitVehicleRoof();
        }
    }
}
