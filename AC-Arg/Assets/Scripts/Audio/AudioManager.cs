using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //dictionary of audiosources and audioclips
    Dictionary<AudioSource, AudioClip> allSounds = new Dictionary<AudioSource, AudioClip>();

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        foreach (AudioSource audioSource in GetComponentsInChildren<AudioSource>())
        {
            //Debug.Log("cargo sonido...");
            allSounds.Add(audioSource, audioSource.clip);
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        foreach (AudioSource audioSource in allSounds.Keys)
        {
            if (allSounds[audioSource] == audioClip)
            {
                Debug.Log("le doy play");
                audioSource.Play();
                return;
            }
        }
    }

    public void StopSound(AudioClip audioClip)
    {
        foreach (AudioSource audioSource in allSounds.Keys)
        {
            if (allSounds[audioSource] == audioClip)
            {
                audioSource.Stop();
                return;
            }
        }
    }



}
