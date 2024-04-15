using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnPlayerHealthUpdate, UpdateHealthBar);
    }

    private void UpdateHealthBar(object[] parameters)
    {
        slider.value = (float)parameters[0] / (float)parameters[1];
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnPlayerHealthUpdate, UpdateHealthBar);
        }
    }
}
