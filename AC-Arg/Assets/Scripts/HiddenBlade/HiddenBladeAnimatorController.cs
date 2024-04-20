using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//require animator component
[RequireComponent(typeof(Animator))]
public class HiddenBladeAnimatorController : MonoBehaviour
{
    Animator _anim;
    [SerializeField] float extensionDelay = 0.5f;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        EventManager.Instance.Subscribe(Evento.OnAssassinationStart, OnAssassinationStart);
    }

    private void OnAssassinationStart(object[] parameters)
    {
        Debug.Log("extiendo la blade");
        SetBladeExtended(true);
        StartCoroutine(CoroutineUtilities.DelayedAction(extensionDelay, HideBlade));
    }

    private void HideBlade(object[] obj)
    {
        Debug.Log("guardo la blade");
        SetBladeExtended(false);
    }

    public void SetBladeExtended(bool value)
    {
        _anim.SetBool("isExtended", value);
    }   

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnAssassinationStart, OnAssassinationStart);
        }
    }
}
