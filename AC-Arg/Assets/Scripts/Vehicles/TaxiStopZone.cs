using System.Collections;
using TMPro;
using UnityEngine;

namespace TrafficSimulation
{
    public class TaxiStopZone : MonoBehaviour
    {
        public float stopChance = 0.5f;
        public Taxi thisTaxi;
        public float timeUntilResume = 5;
        float currentStopChance = 0.5f;
        //bool willStop = true;
        bool playerIsInside = false;
        private float waitingBeforeStopTime = 2;
        bool wasRequested = false;

        private void Start()
        {
            currentStopChance = stopChance;
            //Debug.Log("current stop chance: " + currentStopChance);
            EventManager.Subscribe(Evento.OnPlayerPressedR, OnStopRequested);
        }

        private void OnStopRequested(object[] parameters)
        {
            if (playerIsInside && !wasRequested)
            {
                Debug.Log("taxistop: taxi requested");
                //Debug.Log("current stop chance: " + currentStopChance);
                //currentStopChance = 1;
                TriggerTaxiStop(thisTaxi);
                wasRequested = true;
                EventManager.Trigger(Evento.OnPlayerStopsVehicle);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("el player entro a la stop taxi zone");
                playerIsInside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("el player salio de la stop taxi zone");
                playerIsInside = false;
            }
        }

        private void TriggerTaxiStop(Taxi taxi)
        {
            Debug.Log("trigger taxi stop");
            VehicleAI vehicleAI = taxi.GetComponent<VehicleAI>();
            StartCoroutine(WaitALittleBeforeStopping(vehicleAI));
            StartCoroutine(Resume(vehicleAI));
        }

        public IEnumerator WaitALittleBeforeStopping(VehicleAI vehicle)
        {
            Debug.Log("espero un toque antes de frenar...");
            yield return new WaitForSeconds(waitingBeforeStopTime);
            Debug.Log("dale subite pibe");
            vehicle.InduceStop();
        }

        public IEnumerator Resume(VehicleAI vehicle)
        {
            yield return new WaitForSeconds(timeUntilResume);
            Debug.Log("resume taxi");
            vehicle.InduceGo();
            currentStopChance = stopChance;
            wasRequested = false;
            Debug.Log("current stop chance: " + currentStopChance);
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded)
            {
                EventManager.Unsubscribe(Evento.OnPlayerPressedR, OnStopRequested);
            }
        }
    }
}
