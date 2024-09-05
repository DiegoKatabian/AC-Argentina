using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneAudioRelay : MonoBehaviour
{

    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }
    public void SIGNAL_PlayMusicOblivion(AudioClip clip)
    {
        audioManager.PlaySound(clip);
    }

    public void SIGNAL_PlayMusicCombat(AudioClip clip)
    {
        audioManager.PlaySound(clip);
    }

    public void SIGNAL_StopMusicCombat()
    {
        audioManager.EndCombatMusic();
    }

    public void SIGNAL_PlaySteps()
    {
        //Debug.Log("relay: pls audiomanager play steps");
        audioManager.PlayFootstepSFX();
    }

    public void SIGNAL_FadeOutEzioTango()
    {
        audioManager.FadeOutEzioTango();
    }
}
