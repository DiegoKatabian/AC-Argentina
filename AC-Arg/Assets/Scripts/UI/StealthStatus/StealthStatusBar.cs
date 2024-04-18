using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StealthStatusBar : MonoBehaviour
{
    public TextMeshProUGUI stealthStatusText;

    void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnStealthUpdate, UpdateStealthStatus);
    }

    private void UpdateStealthStatus(object[] parameters)
    {
        //get parameter 0 that is stelathstatusso
        StealthStatusSO status = (StealthStatusSO)parameters[0];

        //set text and color from status, not from stealthmanager

        stealthStatusText.text = status.statusName;
        stealthStatusText.color = status.statusColor;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnStealthUpdate, UpdateStealthStatus);
        }
    }
}
