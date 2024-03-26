using System.Collections;
using UnityEngine;

namespace TrafficSimulation
{
    public class BusStop : MonoBehaviour
    {
        bool playerIsInside = false;
        bool busIsInside = false;

        private void Start()
        {
            BusManager.Instance.AddBusStop(this);
            EventManager.Subscribe(Evento.OnInputRequestBusStop, OnStopRequested);
        }

        private void OnStopRequested(object[] parameters)
        {
            if (playerIsInside)
            {
                Debug.Log("on stop requested");
                BusManager.Instance.StopRequestAccepted();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log("el player entro a la parada");
                playerIsInside = true;
            }

            if (other.CompareTag("AutonomousVehicle") && !busIsInside)
            {
                //Debug.Log("entro el bondi a la parada");
                Bus bus = other.GetComponent<Bus>();
                bus.TriggerStopChance();
                busIsInside = true;

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log("el player salio de la parada");
                playerIsInside = false;
            }
            if (other.CompareTag("AutonomousVehicle"))
            {
                busIsInside = false;
            }
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded)
            {
                EventManager.Unsubscribe(Evento.OnInputRequestBusStop, OnStopRequested);
            }
        }
    }
}
