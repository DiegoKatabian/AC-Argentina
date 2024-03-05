using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Evento //LOS EVENTOS SE AGREGAN AL FINAL. NO EN EL MEDIO, PORQUE ARRUINAN LA NUMERACION
{
    OnPlayerPressedEsc,
    OnPlayerPressedE,
    OnPlayerPressedR,
    OnPlayerStopsVehicle,
    OnPlayerHealthUpdate,
    OnPlayerDied,
    OnStealthUpdate //0 es un string
}

public class EventManager
{
    public delegate void EventReceiver(params object[] parameters);

    static Dictionary<Evento, EventReceiver> _events = new Dictionary<Evento, EventReceiver>();

    public static void Subscribe(Evento evento, EventReceiver metodo)
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

    public static void Unsubscribe(Evento evento, EventReceiver metodo)
    {
        if (_events.ContainsKey(evento))
        {
            _events[evento] -= metodo;
        }
    }

    public static void Trigger(Evento evento, params object[] parameters)
    {
        //Debug.Log("event manager - trigger");
        if (_events.ContainsKey(evento))
        {
            //Debug.Log("tengo al evento en dict, lo disparo");
            _events[evento](parameters);
        }
    }
}
