using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Evento
{
    OnInputRequestPause,
    OnInputRequestInteract,
    OnInputRequestBusStop,
    OnPlayerStopsVehicle,
    OnPlayerHealthUpdate,
    OnPlayerDied,
    OnStealthUpdate, //0 es StealthStatusSO
    OnLeftHandInput,
    OnRightHandInput,
    OnEnemyKilled, //0 es enemy
    OnPlayerInsideCarUpdate, //0 es bool
    OnPedestrianKilled,
    OnInputRequestSteal,
    OnInputRequestAssassinate,
    OnMoneyUpdate, //0 es int currentmoney
    OnInputReleaseSteal,
    OnInputReleaseAssassinate,
    OnInputReleaseBusStop,
    OnInputRequestCrouch,
    OnInputReleaseCrouch,
    OnEnterBlendZoneConfirmed,
    OnActivateBlendZone,
    OnAssassinationStart,
}

public class EventManager : Singleton<EventManager>
{
    public delegate void EventReceiver(params object[] parameters);

    public Dictionary<Evento, EventReceiver> _events = new Dictionary<Evento, EventReceiver>();

    public void Subscribe(Evento evento, EventReceiver metodo)
    {
        if (!_events.ContainsKey(evento))
        {
            _events.Add(evento, metodo);
        }
        else
        {
            _events[evento] += metodo;
        }
    }

    public void Unsubscribe(Evento evento, EventReceiver metodo)
    {
        if (_events.ContainsKey(evento))
        {
            _events[evento] -= metodo;
        }
    }

    public void Trigger(Evento evento, params object[] parameters)
    {
        //Debug.Log("event manager - trigger");
        if (_events.ContainsKey(evento))
        {
            //Debug.Log("tengo al evento en dict, lo disparo");
            _events[evento](parameters);
        }
    }
}
