using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTutorialTrigger : MonoBehaviour
{
    [SerializeField] GameObject floatingTutorialCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floatingTutorialCanvas.SetActive(true);
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
