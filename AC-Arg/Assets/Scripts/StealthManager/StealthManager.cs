using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthManager : Singleton<StealthManager>
{
    public List<StealthStatusSO> stealthStatuses = new List<StealthStatusSO>();
    public float blendDelay = 2.5f;
    [HideInInspector] public StealthStatusSO currentStatus;
    BlendZone currentBlendZone;

    public override void Awake()
    {
        base.Awake();
        //SetStealthStatus(StealthStatus.Anonymous);
    }

    private void Start()
    {
        EventManager.Subscribe(Evento.OnPlayerInsideCarUpdate, PlayerInsideCarUpdate);
        SetStealthStatus(StealthStatus.Anonymous);

    }

    private void PlayerInsideCarUpdate(object[] parameters)
    {
 
        if ((bool)parameters[0])
        {
            Debug.Log("player inside car");
            StartCoroutine(CoroutineUtilities.DelayedAction(blendDelay, SetStealthStatus, "Hidden"));
            //SetStealthStatus(StealthStatus.Hidden);
        }
        else
        {
            Debug.Log("player out of car");
            SetStealthStatus(StealthStatus.Anonymous);
        }
    }

    public void SetStealthStatus(object[] parameters)
    {
        string statusName = (string)parameters[0];
        foreach (StealthStatusSO status in stealthStatuses)
        {
            if (status.name == statusName)
            {
                currentStatus = status;
                EventManager.Trigger(Evento.OnStealthUpdate, statusName);
                PrintStealthStatus();
                return;
            }
        }
    }


    public void SetStealthStatus(string statusName)
    {
        foreach (StealthStatusSO status in stealthStatuses)
        {
            if (status.name == statusName)
            {
                currentStatus = status;
                EventManager.Trigger(Evento.OnStealthUpdate, statusName);
                PrintStealthStatus();
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
                PrintStealthStatus();
                return;
            }
        }
    }

    public void PrintStealthStatus()
    {
        Debug.Log("Stealth Status: " + currentStatus.statusName);
    }

    internal void EnterBlendZone(BlendZone blendZone)
    {
        if (currentStatus.status == StealthStatus.Alert)
        {
            //Debug.Log("estas en alerta, no podes entrar a blendear");
            return;
        }
        //Debug.Log("entraste a la zona de blend, arranca el timer");

        currentBlendZone = blendZone;
        currentBlendZone.EnterZoneConfirmed();
        StartCoroutine(BlendDelay());
    }
    public IEnumerator BlendDelay()
    {
        //Debug.Log("esperando...");
        float elapsedTime = 0;
        while (elapsedTime < blendDelay)
        {
            if (currentStatus.status == StealthStatus.Alert)
            {
                //Debug.Log("cambio de estado, cancelando blend");
                currentBlendZone.BlendCanceled();
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ActivateBlend();
    }
    public void ActivateBlend()
    {
        Debug.Log("blend activado");
        SetStealthStatus(StealthStatus.Hidden);
    }
    internal void ExitBlendZone()
    {
        //Debug.Log("te saliste de la zona de blend, timers cancelados");
        StopAllCoroutines();
        if (currentStatus.status == StealthStatus.Hidden)
        {
            SetStealthStatus(StealthStatus.Anonymous);
        }
    }

    //unsubscribe on destroy
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            StopAllCoroutines();
            EventManager.Unsubscribe(Evento.OnPlayerInsideCarUpdate, PlayerInsideCarUpdate);
        }
    }
}
