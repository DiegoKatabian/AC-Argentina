using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


//force having an image component
[RequireComponent(typeof(Image))]

public class ImageColorChangingBehaviour : MonoBehaviour
{
    public Evento triggeringEvent = Evento.OnInputRequestSteal;
    public Color colorA, colorB;
    public float duration = 0.2f;
    Image _image;

    void Start()
    {
        EventManager.Instance.Subscribe(triggeringEvent, ChangeColor);
        _image = GetComponent<Image>();
    }

    private void ChangeColor(object[] parameters)
    {
        Debug.Log("changing color");
        StartCoroutine(AlphaLerpUtility.LerpColorCoroutine(_image, colorA, colorB, duration/2));
        StartCoroutine(CoroutineUtilities.DelayedAction(duration, ChangeColor, _image, colorB, colorA, duration / 2));

    }


    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(triggeringEvent, ChangeColor);
        }
    }
}
