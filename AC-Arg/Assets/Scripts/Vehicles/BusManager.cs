using System;
using System.Collections;
using System.Collections.Generic;
using TrafficSimulation;
using UnityEngine;

public class BusManager : Singleton<BusManager>
{
    List<BusStop> busStops = new List<BusStop>();
    List<Bus> allBuses = new List<Bus>();

    internal void AddBusStop(BusStop busStop)
    {
        busStops.Add(busStop);
    }

    internal void AddBus(Bus bus)
    {
        //Debug.Log("bus added");
        allBuses.Add(bus);
    }

    internal void StopRequestAccepted()
    {
        //Debug.Log("BusManager: get all buses ready to stop");

        EventManager.Trigger(Evento.OnPlayerStopsVehicle);
        foreach (Bus bus in allBuses)
        {
            bus.GetReadyToStop();
        }
    }
}
