using System.Collections;
using UnityEngine;

namespace TrafficSimulation
{
    public class BusStop : MonoBehaviour
    {
        public float stoppingTime = 6f;
        public float stopChance = 0.5f;
        bool willStop = true;
        bool playerIsInside = false;
        float currentStopChance = 0.5f;

        private void Start()
        {
            currentStopChance = stopChance;
            //Debug.Log("current stop chance: " + currentStopChance);
            EventManager.Subscribe(Evento.OnPlayerPressedR, OnStopRequested);
        }

        private void OnStopRequested(object[] parameters)
        {
            if (playerIsInside)
            {
                Debug.Log("busstop: stop requested");
                Debug.Log("current stop chance: " + currentStopChance);

                currentStopChance = 1;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AutonomousVehicle"))
            {
                willStop = (currentStopChance >= Random.Range(0f, 1f));
                Debug.Log("current stop chance: " + currentStopChance);

                Debug.Log(gameObject.name + " - willstop: " + willStop);

                if (willStop) 
                { 
                    TriggerBusStop(other.gameObject);
                }
            }

            if (other.CompareTag("Player"))
            {
                Debug.Log("el player entro a la parada");
                playerIsInside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("el player salio de la parada");
                playerIsInside = false;
            }
        }

        private void TriggerBusStop(GameObject vehicle)
        {
            VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();
            vehicleAI.vehicleStatus = Status.STOP;
            StartCoroutine(Resume(vehicleAI));
        }

        public IEnumerator Resume(VehicleAI vehicle) 
        {
            yield return new WaitForSeconds(stoppingTime);

            VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();
            vehicleAI.vehicleStatus = Status.GO;
            currentStopChance = stopChance;
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
