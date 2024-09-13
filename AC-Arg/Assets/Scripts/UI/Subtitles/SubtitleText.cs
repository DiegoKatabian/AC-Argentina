using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SubtitleText : MonoBehaviour
{
    public TextMeshProUGUI subtitleText; // Asume que tienes un componente UI Text para mostrar los subtítulos

    SubtitleLine currentSubtitle;
    
    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnSubtitle, OnSubtitle);
        EventManager.Instance.Subscribe(Evento.OnSubtitleClear, OnSubtitleClear);
    }

    private void OnSubtitle(object[] parameters)
    {
        //make sure the parameter exists and is the correct type before using it
        if (parameters.Length == 0 || !(parameters[0] is SubtitleLine))
        {
            Debug.LogError("SubtitleText: OnSubtitle event received with invalid parameters");
            return;
        }

        currentSubtitle = (SubtitleLine)parameters[0];
        StopAllCoroutines();
        StartCoroutine(DisplaySubtitleCoroutine(currentSubtitle));
    }

    public IEnumerator DisplaySubtitleCoroutine(SubtitleLine sub)
    {
        DisplaySubtitle(sub);
        yield return new WaitForSeconds(4);
        ClearSubtitles();
    }

    public void DisplaySubtitle(SubtitleLine sub)
    {
        if (sub.characterName != CharacterName.None)
        {
            subtitleText.text = sub.characterName.ToString() + ": " + sub.text;
        }
        else
        {
            subtitleText.text = sub.text;
        }
    }

    private void OnSubtitleClear(object[] parameters)
    {
        ClearSubtitles();
    }

    public void ClearSubtitles()
    {
        subtitleText.text = "";
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnSubtitle, OnSubtitle);
            EventManager.Instance.Unsubscribe(Evento.OnSubtitleClear, OnSubtitleClear);
        }
    }
}
