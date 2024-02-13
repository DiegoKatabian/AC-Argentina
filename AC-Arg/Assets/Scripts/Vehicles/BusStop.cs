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
                // Si el objeto que entra es un vehículo autónomo (autobús)
                TriggerBusStop(other.gameObject);
            }
        }

        private void TriggerBusStop(GameObject vehicle)
        {
            // Obtener el componente VehicleAI del vehículo
            VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();

            // Detener el vehículo
            vehicleAI.vehicleStatus = Status.STOP;

            // Invocar el método para continuar después de 5 segundos
            StartCoroutine(Resume(vehicleAI));
        }

        //private void ExitBusStop(GameObject vehicle)
        //{
        //    // Obtener el componente VehicleAI del vehículo
        //    VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();

        //    // Continuar la marcha del vehículo
        //    vehicleAI.vehicleStatus = Status.GO;
        //}

        public IEnumerator Resume(VehicleAI vehicle) 
        {
            yield return new WaitForSeconds(stoppingTime);
            // Obtener el componente VehicleAI del vehículo
            VehicleAI vehicleAI = vehicle.GetComponent<VehicleAI>();

            // Continuar la marcha del vehículo
            vehicleAI.vehicleStatus = Status.GO;

        }
    }
}
