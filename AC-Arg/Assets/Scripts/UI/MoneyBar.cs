using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBar : MonoBehaviour
{
    public TMPro.TextMeshProUGUI moneyText;
    public string coinName;

    public void Start()
    {
        EventManager.Subscribe(Evento.OnMoneyUpdate, UpdateMoneyStatus);
    }

    private void UpdateMoneyStatus(object[] parameters)
    {
        moneyText.text = (int)parameters[0] + coinName;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnMoneyUpdate, UpdateMoneyStatus);
        }
    }
}
