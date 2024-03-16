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
        public VehicleAI currentInteractableVehicle;

        public float hopAnimationLockTime = 2.5f;

        bool currentVehicleIsTaxi = false;
        public Rigidbody myRB;

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
            player = transform.parent;
            originalParent = player.parent; //originalparent seria la escena, el padre de Player, no de playermodel
            playerController = GetComponent<ThirdPersonController>();
            EventManager.Subscribe(Evento.OnPlayerPressedE, TryInteract);
        }

        private void TryInteract(object[] parameters)
        {
            //Debug.Log("tratando de interactuar...");

            if (currentInteractableVehicle == null)
            {
                //Debug.Log("no hay ningun auto a mano");
                return;
            }

            if (!currentInteractableVehicle.canInteract)
            {
                Debug.Log("las puertas estan cerradas!");
                return;
            }

            if (insideBox || insideCar)
            {
                InteractWithVehicle();
            }
        }

        private void InteractWithVehicle()
        {
            //Debug.Log("Interact with vehicle");
            player.parent = currentInteractableVehicle.transform;

            if (!insideCar)
            {
                GetInsideCar();
            }
            else
            {
                GetOutOfCar();
            }
        }

        void GetInsideCar()
        {
            Debug.Log("me subo al coche");
            player.parent = currentInteractableVehicle.transform;
            insideBox = false;
            playerController.DisableController();
            playerController.characterAnimation.StartEnterVehicleAnimation(currentVehicleIsTaxi);
            playerController.characterAnimation.switchCameras.VehicleCam(currentInteractableVehicle.transform);
            StartCoroutine(WaitUntilEnableMesh(false));
            currentInteractableVehicle.OnPlayerHopOn();
            insideCar = true;
            EventManager.Trigger(Evento.OnPlayerInsideCarUpdate, true);
        }

        void TriggerEnterCarAnimation()
        {
            playerController.DisableController();
            playerController.characterAnimation.StartEnterVehicleAnimation(currentVehicleIsTaxi);
            playerController.characterAnimation.switchCameras.VehicleCam(currentInteractableVehicle.transform);
            StartCoroutine(WaitUntilEnableMesh(false));
        }

        void GetOutOfCar()
        {
            Debug.Log("me bajo del coche");
            player.parent = originalParent;

            insideBox = false;
            insideCar = false;
            playerController.EnableController();
            playerController.characterAnimation.StartExitVehicleAnimation();
            playerController.characterAnimation.switchCameras.FreeLookCam();
            playerController.characterAnimation.EnableMesh(true);
            currentInteractableVehicle.OnPlayerHopOff();
            EventManager.Trigger(Evento.OnPlayerInsideCarUpdate, false);
        }

        public IEnumerator WaitUntilEnableMesh(bool state)
        {
            yield return new WaitForSeconds(hopAnimationLockTime);
            playerController.characterAnimation.EnableMesh(state);
        }

        public IEnumerator WaitUntilEnableController()
        {
            yield return new WaitForSeconds(hopAnimationLockTime);
            playerController.EnableController();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnterVehicleBox") && !insideBox)
            {
                if (other.transform.GetComponentInParent<VehicleAI>() != null)
                {
                    currentInteractableVehicle = other.transform.GetComponentInParent<VehicleAI>();
                    if (other.transform.GetComponentInParent<Taxi>() != null)
                    {
                        currentVehicleIsTaxi = true;
                    }
                }

                //Debug.Log("OnTriggerEnter: Entraste a la caja");
                insideBox = true;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("EnterVehicleBox") && insideBox)
            {
                insideBox = false;
                currentVehicleIsTaxi = false;
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
