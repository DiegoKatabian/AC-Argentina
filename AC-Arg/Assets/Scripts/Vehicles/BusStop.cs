using System.Collections;
using UnityEngine;

namespace TrafficSimulation
{
    public class BusStop : MonoBehaviour
    {
        public float stoppingTime = 6f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AutonomousVehicle"))
            {
                // Si el objeto que entra es un veh�culo aut�nomo (autob�s)
                TriggerBusStop(other.gameObject);
            }
        }

        private void TriggerBusStop(GameObject vehicle)
        {
            // Obtener el componente VehicleAI del veh�culo
            VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();

            // Detener el veh�culo
            vehicleAI.vehicleStatus = Status.STOP;

            // Invocar el m�todo para continuar despu�s de 5 segundos
            StartCoroutine(Resume(vehicleAI));
        }

        //private void ExitBusStop(GameObject vehicle)
        //{
        //    // Obtener el componente VehicleAI del veh�culo
        //    VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();

        //    // Continuar la marcha del veh�culo
        //    vehicleAI.vehicleStatus = Status.GO;
        //}

        public IEnumerator Resume(VehicleAI vehicle) 
        {
            yield return new WaitForSeconds(stoppingTime);
            // Obtener el componente VehicleAI del veh�culo
            VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();

            // Continuar la marcha del veh�culo
            vehicleAI.vehicleStatus = Status.GO;

        }
    }
}
