using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTutorialTrigger : MonoBehaviour
{
    [SerializeField] GameObject floatingTutorialCanvas;
    [SerializeField] bool usesGameEventAsTurnOffCondition = false;
    [SerializeField] Evento gameEventToTurnOff;
    [SerializeField] bool isOneTimeOnly = false;
    bool wasTriggered = false;


    private void Start()
    {
        if (usesGameEventAsTurnOffCondition)
        {
            EventManager.Instance.Subscribe(gameEventToTurnOff, TurnOffTutorial);

        }
    }

    private void OnDestroy()
    {
        if (usesGameEventAsTurnOffCondition)
        {
            EventManager.Instance.Unsubscribe(gameEventToTurnOff, TurnOffTutorial);
        }
    }

    private void TurnOffTutorial(object[] parameters)
    {
        if (usesGameEventAsTurnOffCondition && wasTriggered)
        {
            floatingTutorialCanvas.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOneTimeOnly && wasTriggered)
            {
                return;
            }
            floatingTutorialCanvas.SetActive(true);
            wasTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingTutorialCanvas.SetActive(false);
        }
    }
}
