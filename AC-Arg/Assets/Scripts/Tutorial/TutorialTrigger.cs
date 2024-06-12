using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Playables;
using System;

public class TutorialTrigger : MonoBehaviour
{
    public bool isOneTimeOnly = true;
    bool hasBeenTriggered = false;

    public TextMeshProUGUI[] texts; //they will show in sequential order
    public Image[] images; //they will show in sequential order
    public float timeToFade = 2;
    public float displayTime = 2; // time each text/image is displayed before fading out

    public bool isTriggeredByEndOfCutscene = false;
    public CutsceneTrigger cutsceneTrigger;

    private void Start()
    {
        if (isTriggeredByEndOfCutscene)
        {
            EventManager.Instance.Subscribe(Evento.OnCutsceneEnd, OnCutsceneEnd);
        }
    }

    private void OnCutsceneEnd(object[] parameters)
    {
        if (isOneTimeOnly && hasBeenTriggered) return;

        if (parameters.Length > 1 && 
            parameters[1] is CutsceneTrigger && 
            (CutsceneTrigger)parameters[1] == cutsceneTrigger)
        {
            Debug.Log("Triggered by cutscene end");
            hasBeenTriggered = true;
            StartCoroutine(StartTutorial());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasBeenTriggered) return;

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered by player");
            hasBeenTriggered = true;
            StartCoroutine(StartTutorial());
        }
    }

    private IEnumerator StartTutorial()
    {
        for (int i = 0; i < texts.Length && i < images.Length; i++)
        {
            texts[i].gameObject.SetActive(true);
            images[i].gameObject.SetActive(true);

            StartCoroutine(AlphaLerpUtility.LerpAlpha(texts[i], 0, texts[i].color.a, timeToFade));
            StartCoroutine(AlphaLerpUtility.LerpAlpha(images[i], 0, images[i].color.a, timeToFade));
            yield return new WaitForSeconds(timeToFade + displayTime);

            StartCoroutine(AlphaLerpUtility.LerpAlpha(texts[i], texts[i].color.a, 0, timeToFade));
            StartCoroutine(AlphaLerpUtility.LerpAlpha(images[i], images[i].color.a, 0, timeToFade));
            yield return new WaitForSeconds(timeToFade);

            texts[i].gameObject.SetActive(false);
            images[i].gameObject.SetActive(false);
        }
    }

    public void StartTutorialStep()
    {
        if (isOneTimeOnly && hasBeenTriggered) return;

        StartCoroutine(StartTutorial());
    }

    public void EndTutorialStep()
    {
        // This method is not needed in the current setup but can be used for additional logic if required
    }
}
