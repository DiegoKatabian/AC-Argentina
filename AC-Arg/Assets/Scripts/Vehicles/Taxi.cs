using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TrafficSimulation
{

    public class Taxi : VehicleAI
    {
        public Rigidbody myRB;

        public override void OnPlayerHopOn()
        {
            base.OnPlayerHopOn();
            Debug.Log("taxi on player hop on");
            TaxiManager.instance.StartTrip(this);
            canInteract = false;
        }

        public override void OnPlayerHopOff()
        {
            base.OnPlayerHopOff();
            Debug.Log("taxi on player hop off");

            InduceGo();
        }

        public void Teleport(TaxiDestination taxiDestination)
        {
            Debug.Log("teleport taxi");
            myRB.Move(taxiDestination.position, Quaternion.Euler(taxiDestination.rotation));
        }
    }
}