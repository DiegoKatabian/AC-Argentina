using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using System;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public CutsceneTrigger[] cutsceneTriggers; // Añadir manualmente desde el inspector.

    private void Start()
    {
        //turn off gameobjcet and playon awake for each cutscenetrigger whos index in respawnmanager is less than the current cutscene index. i mean, turn off all seen cutscenes
        for (int i = 0; i < cutsceneTriggers.Length; i++)
        {
            if (RespawnManager.Instance.seenCutscenes[i])
            {
                Debug.Log("turning off cutscene " + cutsceneTriggers[i] + " for i have seen it already");
                cutsceneTriggers[i].gameObject.SetActive(false);
                cutsceneTriggers[i].GetComponent<PlayableDirector>().playOnAwake = false;
            }

            //if the current index is exactly 3, turn on the prebattle cutscene
            if (RespawnManager.Instance.CurrentCutsceneIndex == 3)
            {
                cutsceneTriggers[3].gameObject.SetActive(true);
            }
        }
    }

    public void ResetCutsceneData()
    {
        // Simular que solo la primera cutscene fue vista.
        for (int i = 1; i < cutsceneTriggers.Length; i++)
        {
            cutsceneTriggers[i].gameObject.SetActive(true);
        }
        //apago el playonawake de la primera cutsce
        cutsceneTriggers[0].GetComponent<PlayableDirector>().playOnAwake = false;
    }

}
