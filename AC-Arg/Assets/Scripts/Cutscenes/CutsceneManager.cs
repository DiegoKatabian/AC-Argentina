using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayCutscene(Cutscene cutscene)
    {
        if (cutscene == null)
        {
            Debug.LogWarning("Cutscene is null!");
            return;
        }

        foreach (var cutsceneEvent in cutscene.Events)
        {
            StartCoroutine(PlayCutsceneEvent(cutsceneEvent));
        }
    }

    private IEnumerator PlayCutsceneEvent(ICutsceneEvent cutsceneEvent)
    {
        yield return new WaitForSeconds(cutsceneEvent.Delay);
        cutsceneEvent.Execute();
    }
}
