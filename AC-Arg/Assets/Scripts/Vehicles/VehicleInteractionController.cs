using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Climbing
{
    public class VehicleInteractionController : MonoBehaviour
    {
        //private ThirdPersonController playerController;
        //private VehicleController vehicleController;

        //private void Start()
        //{
        //    playerController = GetComponent<ThirdPersonController>();
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Vehicle"))
        //    {
        //        Debug.Log("OnTriggerEnter: tag was vehicle");
        //        vehicleController = other.GetComponentInParent<VehicleController>();

        //        if (vehicleController != null)
        //        {
        //            playerController.EnableVehicleInteraction();
        //            Debug.Log("OnTriggerEnter: tell player controller to enable vehicle interaction");
        //        }
        //    }
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    // Verificar si el jugador sali� del contacto con un veh�culo
        //    if (other.CompareTag("Vehicle"))
        //    {
        //        // Desactivar la interacci�n con el veh�culo
        //        playerController.DisableVehicleInteraction();
        //        vehicleController = null;
        //    }
        //}

        //private void Update()
        //{
        //    if (playerController.IsOnVehicle())
        //    {
        //        if (vehicleController != null)
        //        {
        //            Debug.Log("aviso al thirdpersoncontroller cual es la velocity del coche");
        //            playerController.ApplyVehicleVelocity(vehicleController.GetVehicleVelocity());
        //        }
        //    }
        //}
    }
}
