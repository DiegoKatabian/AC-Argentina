using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StealthStatusBar : MonoBehaviour
{
    public TextMeshProUGUI stealthStatusText;
    public StealthStatusSO[] stealthStatuses;
    StealthStatusSO currentStealthStatus;

    void Start()
    {
        EventManager.Subscribe(Evento.OnPlayerStealthUpdate, UpdateStealthStatus);
    }

    private void UpdateStealthStatus(object[] parameters)
    {
        string newStatusName = (string)parameters[0];
        currentStealthStatus = Array.Find(stealthStatuses, status => status.statusName == newStatusName);
        stealthStatusText.text = currentStealthStatus.statusName;
        stealthStatusText.color = currentStealthStatus.statusColor;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnPlayerStealthUpdate, UpdateStealthStatus);
        }
    }
}
