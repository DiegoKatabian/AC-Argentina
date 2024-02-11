using System;
using System.Collections;
using System.Collections.Generic;
using TrafficSimulation;
using UnityEngine;

namespace Climbing
{
    public class VehicleInteractionController : MonoBehaviour
    {
        private ThirdPersonController playerController;
        [HideInInspector] public bool insideBox = false;
        [HideInInspector] public bool insideCar = false;
        Transform player; //el player general, no el playermodel
        Transform originalParent; //la escena
        VehicleAI vehicle;

        //escena
        //  player
        //      playermodel
        //  auto

        //se convierte en

        //escena
        //  auto
        //      player
        //          playermodel

        private void Start()
        {
            //yo = transform; jajaj es que este script esta pegado al objeto PlayerModel. Player a secas es el padre.
            player = transform.parent;
            originalParent = player.parent; //originalparent seria la escena, el padre de Player, no de playermodel
            playerController = GetComponent<ThirdPersonController>();
            EventManager.Subscribe(Evento.OnPlayerPressedE, TryInteract);
        }

        private void TryInteract(object[] parameters)
        {
            Debug.Log("tratando de interactuar...");

            if (!vehicle.canInteract)
            {
                Debug.Log("las puertas estan cerradas!");
                return;
            }
            else
            {
                Debug.Log("ok, podes subir o bajar, ni idea que querias hacer pero podes");
            }

            if (insideBox || insideCar)
            {
                InteractWithVehicle();
            }
        }

        private void InteractWithVehicle()
        {
            //Debug.Log("Interact with vehicle");
            player.parent = vehicle.transform;

            if (!insideCar)
            {
                GetInsideCar();
            }
            else
            {
                GetOutOfCar();
            }
        }

        void GetOutOfCar()
        {
            //Debug.Log("me bajo del coche");
            player.parent = originalParent;
            insideBox = false;
            insideCar = false;
            playerController.EnableController();
            playerController.characterAnimation.switchCameras.FreeLookCam();
            playerController.characterAnimation.EnableMesh(true);
        }

        void GetInsideCar()
        {
            //Debug.Log("me subo al coche");
            player.parent = vehicle.transform;
            insideBox = false;
            insideCar = true;
            playerController.DisableController();
            playerController.characterAnimation.switchCameras.VehicleCam(vehicle.transform);
            playerController.characterAnimation.EnableMesh(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnterVehicleBox") && !insideBox)
            {
                vehicle = other.transform.GetComponentInParent<VehicleAI>();
                if (vehicle != null)
                {
                    Debug.Log("obtuve vehicleAI " + vehicle.gameObject.name);
                }
                else
                {
                    Debug.Log("che no pude agarrar el vehicleAI");
                }


                //Debug.Log("OnTriggerEnter: Entraste a la caja");
                insideBox = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EnterVehicleBox") && insideBox)
            {
                //cuando salgo del trigger, el player vuelve a ser hijo de la escena, como era originalmente.
                //vehicle = null;
                //Debug.Log("OnTriggerExit: Saliste de la caja");
                insideBox = false;
            }
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded)
            {
                EventManager.Unsubscribe(Evento.OnPlayerPressedE, TryInteract);
            }
        }
    }
}
