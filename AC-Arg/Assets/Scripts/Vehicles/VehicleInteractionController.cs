using System;
using System.Collections;
using System.Collections.Generic;
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
        Transform vehicle;

        //escena
        //  player
        //      playermodel
        //auto

        //se convierte en

        //auto
        //  player
        //      playermodel

        private void Start()
        {
            //yo = transform; jajaj es que este script esta pegado al objeto PlayerModel. Player a secas es el padre.
            player = transform.parent;
            originalParent = player.parent; //originalparent seria la escena, el padre de Player, no de playermodel
            playerController = GetComponent<ThirdPersonController>();
            EventManager.Subscribe(Evento.OnPlayerPressedE, TryInteract);
        }

        //private void Update()
        //{
        //    if (insideCar)
        //    {
        //        player.position = vehicle.position;
        //    }
        //}

        private void TryInteract(object[] parameters)
        {
            Debug.Log("tratando de interactuar...");
            if (insideBox || insideCar)
            {
                InteractWithVehicle();
            }
        }

        private void InteractWithVehicle()
        {
            Debug.Log("Interact with vehicle");
            player.parent = vehicle;

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
            Debug.Log("me bajo del coche");
            player.parent = originalParent;
            insideCar = false;
            playerController.EnableController();
            playerController.characterAnimation.switchCameras.FreeLookCam();
            player.position = vehicle.position;
        }

        void GetInsideCar()
        {
            Debug.Log("me subo al coche");
            player.parent = vehicle;
            insideCar = true;
            playerController.DisableController();
            playerController.characterAnimation.switchCameras.VehicleCam(/* el coche en cuestion*/);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnterVehicleBox") && !insideBox)
            {
                vehicle = other.transform; //el padre de la box collider es el auto en sí.
                Debug.Log("OnTriggerEnter: Entraste a la caja");
                insideBox = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EnterVehicleBox") && insideBox)
            {
                //cuando salgo del trigger, el player vuelve a ser hijo de la escena, como era originalmente.
                //vehicle = null;
                Debug.Log("OnTriggerExit: Saliste de la caja");
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
