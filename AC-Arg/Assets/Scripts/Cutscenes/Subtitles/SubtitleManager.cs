using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SubtitleLine
{
    public CharacterName characterName;
    public string text;
}

public class SubtitleManager : MonoBehaviour
{
    public SubtitleSetSO[] subtitleSets;
    private SubtitleSetSO currentSubtitleSet;

    Queue<SubtitleLine> subtitlesQueue = new Queue<SubtitleLine>();

    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnCutsceneStart, OnCutsceneStart);
    }

    private void OnCutsceneStart(object[] parameters)
    {
        //clear the current queue and current set before setting it
        subtitlesQueue.Clear();
        currentSubtitleSet = null;

        currentSubtitleSet = (SubtitleSetSO)parameters[0];
        foreach (SubtitleLine s in currentSubtitleSet.subtitles)
        {
            subtitlesQueue.Enqueue(s);
        }
        Debug.Log("subtitle manager: cargo el current subtitle set: " + currentSubtitleSet);
    }

    public SubtitleLine GetNextSubtitle()
    {
        SubtitleLine emptySubtitle = new SubtitleLine();
        emptySubtitle.characterName = CharacterName.None;
        emptySubtitle.text = "";

        if (currentSubtitleSet == null)
        {
            Debug.LogError("subtitle manager: no hay current subtitle set");
            return emptySubtitle;
        }

        //return the next subtitle in queue
        if (subtitlesQueue.Count > 0)
        {
            Debug.Log("toma papu, te dejo aca el subtitle que toca ahora");
            return subtitlesQueue.Dequeue();
        }
        else
        {
            Debug.Log("pues ya no hay");
            return emptySubtitle;
        }
    }

    public void TIMELINE_TriggerSubtitle() //disparado por la timeline
    {
        Debug.Log("subtitle manager: trigger subtitle");
        EventManager.Instance.Trigger(Evento.OnSubtitle, GetNextSubtitle());
    }

    public void TIMELINE_ClearSubtitles()
    {
        Debug.Log("subtitle manager: clear subtitles");
        EventManager.Instance.Trigger(Evento.OnSubtitleClear);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnCutsceneStart, OnCutsceneStart);
        }
    }
}
