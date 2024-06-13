using System.Collections;
using System.Collections.Generic;
using TrafficSimulation;
using UnityEngine;

public class Bus : VehicleAI
{
    public bool willStop = false;
    public float stoppingTime = 4f;
    public float stopChance = 0.5f;
    float currentStopChance;


    public override void Start()
    {
        base.Start();
        currentStopChance = stopChance;
        //Debug.Log("try to add bus");
        if (!BusManager.Instance)
        {
            Debug.Log("bus manager not found");
            return;
        }
        BusManager.Instance.AddBus(this);
        EventManager.Instance.Subscribe(Evento.OnInputRequestBusStop, OnStopRequested);
    }

    private void OnStopRequested(object[] parameters)
    {
        //Debug.Log("on stop requested");
        if (playerIsInside)
        {
            Debug.Log("PLAYER IS INSIDE, PIDIENDO PARADA POR FAVOR");
            GetReadyToStop();
        }
    }

    public override void GetReadyToStop()
    {
        Debug.Log("Bus: ready to stop");
        currentStopChance = 1f;
    }

    public override void TriggerStopChance()
    {
        //Debug.Log("bus: trigger stop chance");
        willStop = (currentStopChance >= Random.Range(0f, 1f));

        if (willStop)
        {
            //Debug.Log("yes");
            TriggerBusStop();
        }
        else
        {
            //Debug.Log("no");
        }
    }

    public void TriggerBusStop()
    {
        //Debug.Log("bus: bus stop triggered");
        vehicleStatus = Status.STOP;
        StartCoroutine(Resume());
    }

    public IEnumerator Resume()
    {
        yield return new WaitForSeconds(stoppingTime);
        //Debug.Log("bus: resume");
        vehicleStatus = Status.GO;
        currentStopChance = stopChance;
    }

    public override void OnPlayerHopOff()
    {
        base.OnPlayerHopOff();
        Debug.Log("bus hop off");
        //playerIsInside = false;
    }

    public override void OnPlayerHopOn()
    {
        base.OnPlayerHopOn();
        Debug.Log("bus hop on");

        //playerIsInside = true;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnInputRequestBusStop, OnStopRequested);
        }
    }
}
