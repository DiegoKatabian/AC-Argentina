using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : Singleton<CutsceneManager>
{
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

    private IEnumerator PlayCutsceneEvent(CutsceneEvent cutsceneEvent)
    {
        yield return new WaitForSeconds(cutsceneEvent.Delay);
        cutsceneEvent.Execute();
    }
}
