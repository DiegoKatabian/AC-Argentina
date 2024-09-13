using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneScreen : MonoBehaviour
{
    public TextMeshProUGUI[] texts;
    public Image[] images;
    public float timeToFade = 5;
    public bool isOneTimeOnly = true;
    bool hasBeenTriggered = false;

    public void SIGNAL_StartCutsceneScreen() //es disparado por el timeline
    {
        if (isOneTimeOnly && hasBeenTriggered) return;

        foreach (TextMeshProUGUI text in texts)
        {
            //fade in de todas las imagenes dadas
            text.gameObject.SetActive(true);
            StartCoroutine(AlphaLerpUtility.LerpAlpha(text, 0, text.color.a, timeToFade));
        }

        foreach (Image image in images)
        {
            //fade in de todas las imagenes dadas
            image.gameObject.SetActive(true);
            StartCoroutine(AlphaLerpUtility.LerpAlpha(image, 0, image.color.a, timeToFade));
        }
        hasBeenTriggered = true;
    }

    public void SIGNAL_EndCutsceneScreen()
    {
        foreach (TextMeshProUGUI text in texts)
        {
            //fade out de todas las imagenes dadas
            StartCoroutine(AlphaLerpUtility.LerpAlpha(text, text.color.a, 0, timeToFade));
        }

        foreach (Image image in images)
        {
            //fade out de todas las imagenes dadas
            StartCoroutine(AlphaLerpUtility.LerpAlpha(image, image.color.a, 0, timeToFade));
        }
    }
}
