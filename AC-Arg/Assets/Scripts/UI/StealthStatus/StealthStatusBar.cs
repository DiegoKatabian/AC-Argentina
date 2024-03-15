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
        EventManager.Subscribe(Evento.OnStealthUpdate, UpdateStealthStatus);
    }

    private void UpdateStealthStatus(object[] parameters)
    {
        stealthStatusText.text = StealthManager.Instance.currentStatus.statusName;
        stealthStatusText.color = StealthManager.Instance.currentStatus.statusColor;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnStealthUpdate, UpdateStealthStatus);
        }
    }
}
