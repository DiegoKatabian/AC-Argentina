using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CutsceneScreen : MonoBehaviour
{
    public TextMeshProUGUI[] texts;
    public float timeToFade = 5;


    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnPlayerEnterCutsceneArea, StartCutscene);
    }

    private void StartCutscene(object[] parameters)
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.gameObject.SetActive(true);
            StartCoroutine(AlphaLerpUtility.LerpAlpha(text, 0, 1, timeToFade));
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnPlayerEnterCutsceneArea, StartCutscene);
        }
    }
}
