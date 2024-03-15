using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthManager : Singleton<StealthManager>
{
    public List<StealthStatusSO> stealthStatuses = new List<StealthStatusSO>();
    public float blendDelay = 2.5f;
    StealthStatusSO currentStatus;

    public void SetStealthStatus(StealthStatusSO newStatus)
    {
        currentStatus = newStatus;
        EventManager.Trigger(Evento.OnStealthUpdate, newStatus.name);
    }
    public void SetStealthStatus(string statusName)
    {
        foreach (StealthStatusSO status in stealthStatuses)
        {
            if (status.name == statusName)
            {
                currentStatus = status;
                EventManager.Trigger(Evento.OnStealthUpdate, statusName);
                return;
            }
        }
    }
    public void SetStealthStatus(StealthStatus stealthStatus)
    {
        foreach (StealthStatusSO status in stealthStatuses)
        {
            if (status.status == stealthStatus)
            {
                currentStatus = status;
                EventManager.Trigger(Evento.OnStealthUpdate, status.name);
                return;
            }
        }
    }
    internal void EnterBlendZone(BlendZone blendZone)
    {
        if (currentStatus.status == StealthStatus.Alert)
        {
            Debug.Log("estas en alerta, no podes entrar a blendear");
            return;
        }
        Debug.Log("entraste a la zona de blend, arranca el timer");
        StartCoroutine(BlendDelay());
    }

    //a coroutine that waits for blendDelay seconds and then activates the blend
    public IEnumerator BlendDelay()
    {
        Debug.Log("esperando...");
        yield return new WaitForSeconds(blendDelay);
        ActivateBlend();
    }

    public void ActivateBlend()
    {
        Debug.Log("blend activado");
        SetStealthStatus(StealthStatus.Anonymous);
    }

    internal void ExitBlendZone()
    {
        //cancel the blend delay coroutine
        StopAllCoroutines();
        Debug.Log("te saliste de la zona de blend, timers cancelados");
    }
}
