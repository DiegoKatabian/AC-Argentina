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

        StartCoroutine(PlayCutsceneCoroutine(cutscene));
    }

    private IEnumerator PlayCutsceneCoroutine(Cutscene cutscene)
    {
        float startTime = Time.time;

        foreach (var cutsceneEvent in cutscene.Events)
        {
            yield return new WaitForSeconds(cutsceneEvent.Delay);
            cutsceneEvent.Execute();
        }

        float elapsedTime = Time.time - startTime;
        float remainingTime = cutscene.Duration - elapsedTime;
        Debug.Log("remanining time " + remainingTime);
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        EndCutscene();
    }

    private void EndCutscene()
    {
        Debug.Log("termina la cutscene");
        CameraManager.Instance.SwitchToPlayerCamera();
    }
}
