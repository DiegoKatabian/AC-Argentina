using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTrigger : MonoBehaviour
{
    //quest triggers are similar to tutorial triggers, 
    //but they always start triggered by the end of a cutscene, 
    //and are always turned off by reaching a collider trigger

    public float timeToFade = 1;
    public TextMeshProUGUI text;
    public Image[] images; 
    public Image shiningIcon;
    public CutsceneTrigger cutsceneTrigger;

    public bool endsByEvent = false;
    public Evento eventThatCompletesQuest;


    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnCutsceneEnd, OnCutsceneEnd);
        if (endsByEvent)
        {
            EventManager.Instance.Subscribe(eventThatCompletesQuest, OnQuestCompleted);
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(Evento.OnCutsceneEnd, OnCutsceneEnd);
        if (endsByEvent)
        {
            EventManager.Instance.Unsubscribe(eventThatCompletesQuest, OnQuestCompleted);
        }
    }

    private void OnQuestCompleted(object[] parameters)
    {
        Debug.Log("hide quest (event)");
        StartCoroutine(EndQuest());
    }

    private void OnCutsceneEnd(object[] parameters)
    {

        if (parameters.Length > 1 &&
            parameters[1] is CutsceneTrigger &&
            (CutsceneTrigger)parameters[1] == cutsceneTrigger)
        {
            Debug.Log("quest started by cutscene end");
            StartCoroutine(StartQuest());
        }
    }

    private IEnumerator StartQuest()
    {
        yield return new WaitForSeconds(timeToFade);
        StartCoroutine(FadeInText(text, timeToFade));
        StartCoroutine(FadeInImage(images, timeToFade));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hide quest");
            StartCoroutine(EndQuest());
        }
    }

    private IEnumerator EndQuest()
    {
        StartCoroutine(FadeInImage(new Image[] { shiningIcon }, timeToFade * 0.5f));
        yield return new WaitForSeconds(timeToFade);
        StartCoroutine(FadeOutText(text, timeToFade));
        StartCoroutine(FadeOutImage(images, timeToFade));
        StartCoroutine(FadeOutImage(new Image[] { shiningIcon }, timeToFade));
    }

    private IEnumerator FadeInText(TextMeshProUGUI text, float time)
    {
        text.gameObject.SetActive(true);
        StartCoroutine(AlphaLerpUtility.LerpAlpha(text, 0, text.color.a, time));
        yield return new WaitForSeconds(time);
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text, float time)
    {
        StartCoroutine(AlphaLerpUtility.LerpAlpha(text, text.color.a, 0, time));
        yield return new WaitForSeconds(time);
        text.gameObject.SetActive(false);
    }

    private IEnumerator FadeInImage(Image[] images, float time)
    {
        foreach (Image image in images)
        {
            image.gameObject.SetActive(true);
            StartCoroutine(AlphaLerpUtility.LerpAlpha(image, 0, image.color.a, time));
        }
        yield return new WaitForSeconds(time);
    }

    private IEnumerator FadeOutImage(Image[] images, float time)
    {
        foreach (Image image in images)
        {
            StartCoroutine(AlphaLerpUtility.LerpAlpha(image, image.color.a, 0, time));
        }

        yield return new WaitForSeconds(time);

        foreach (Image image in images)
        {
            image.gameObject.SetActive(false);
        }
    }



}
