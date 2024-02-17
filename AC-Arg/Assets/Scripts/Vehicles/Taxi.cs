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
            TaxiManager.instance.StartTrip(this);
            canInteract = false;
        }

        public override void OnPlayerHopOff()
        {
            base.OnPlayerHopOff();
            InduceGo();
        }

        public void Teleport(TaxiDestination taxiDestination)
        {
            myRB.Move(taxiDestination.position, Quaternion.Euler(taxiDestination.rotation));
        }
    }
}