using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoleadorasWeaponAnimatorController : MonoBehaviour
{
    [SerializeField] float delayBeforeHidingWeapon = 0.5f;
    [SerializeField] GameObject boleadoras;

    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnBoleadorasStart, OnBoleadorasStart);
    }

    private void OnBoleadorasStart(object[] parameters)
    {
        //Debug.Log("hago aparecer las boleadoras");
        boleadoras.SetActive(true);
        StartCoroutine(CoroutineUtilities.DelayedAction(delayBeforeHidingWeapon, HideBoleadoras));
    }

    private void HideBoleadoras(object[] obj)
    {
        //Debug.Log("guardo las boleadoras");
        boleadoras.SetActive(false);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnAssassinationStart, OnBoleadorasStart);
        }
    }
}
