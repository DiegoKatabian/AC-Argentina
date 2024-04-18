using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthManager : Singleton<StealthManager>
{
    public List<StealthStatusSO> stealthStatuses = new List<StealthStatusSO>();
    public float blendDelay = 2.5f;
    public StealthStatusSO currentStatus;
    BlendZone currentBlendZone;

    public override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnPlayerInsideCarUpdate, PlayerInsideCarUpdate);

    }

    private void PlayerInsideCarUpdate(object[] parameters)
    {
 
        if ((bool)parameters[0])
        {
            Debug.Log("player inside car");
            StartCoroutine(CoroutineUtilities.DelayedAction(blendDelay, SetStealthStatus, "Hidden"));
        }
        else
        {
            Debug.Log("player out of car");
            SetStealthStatus(StealthStatus.Visible);
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
                EventManager.Instance.Trigger(Evento.OnStealthUpdate, currentStatus);
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
                EventManager.Instance.Trigger(Evento.OnStealthUpdate, currentStatus);
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
                EventManager.Instance.Trigger(Evento.OnStealthUpdate, currentStatus);
                PrintStealthStatus();
                return;
            }
        }
    }

    public void PrintStealthStatus()
    {
        //Debug.Log("Stealth Status: " + currentStatus.statusName);
    }

    internal void EnterBlendZone(BlendZone blendZone)
    {
        if (currentStatus.status == StealthStatus.Alert)
        {
            //Debug.Log("estas en alerta, no podes entrar a blendear");
            return;
        }
        //Debug.Log("entraste a la zona de blend, arranca el timer");

        EventManager.Instance.Trigger(Evento.OnEnterBlendZoneConfirmed);
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
        EventManager.Instance.Trigger(Evento.OnActivateBlendZone);
    }
    internal void ExitBlendZone()
    {
        //Debug.Log("te saliste de la zona de blend, timers cancelados");
        StopAllCoroutines();
        if (currentStatus.status == StealthStatus.Hidden)
        {
            SetStealthStatus(StealthStatus.Visible);
        }
    }

    //unsubscribe on destroy
    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            StopAllCoroutines();
            EventManager.Instance.Unsubscribe(Evento.OnPlayerInsideCarUpdate, PlayerInsideCarUpdate);
        }
    }
}
