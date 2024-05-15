using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    //dictionary of audiosources and audioclips
    Dictionary<AudioSource, AudioClip> allSounds = new Dictionary<AudioSource, AudioClip>();

    public AudioSource[] deathMaleGroup, hurtMaleGroup;
    public AudioSource[] punchAirGroup, punchHitGroup;


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

    public void PlaySound(AudioClip audioClip, float centralPitch, float pitchVariation)
    {
        foreach (AudioSource audioSource in allSounds.Keys)
        {
            if (allSounds[audioSource] == audioClip)
            {
                audioSource.pitch = Random.Range(centralPitch - pitchVariation, centralPitch + pitchVariation);
                audioSource.Play();
                StartCoroutine(WaitTillSoundHasFinishedPlayingAndReturnPitchToOriginal(audioSource));
                return;
            }
        }
    }

    //similar but parameter is fixed new pitch
    public void PlaySound(AudioClip audioClip, float newPitch)
    {
        foreach (AudioSource audioSource in allSounds.Keys)
        {
            if (allSounds[audioSource] == audioClip)
            {
                audioSource.pitch = newPitch;
                audioSource.Play();
                StartCoroutine(WaitTillSoundHasFinishedPlayingAndReturnPitchToOriginal(audioSource));
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

    public void PlayDeathSFX()
    {
        deathMaleGroup[Random.Range(0, deathMaleGroup.Length)].Play();
    }

    public void PlayHurtSFX()
    {
        hurtMaleGroup[Random.Range(0, hurtMaleGroup.Length)].Play();
    }

    public void PlayPunchHitSFX()
    {
        punchHitGroup[Random.Range(0, punchHitGroup.Length)].Play();
    }

    public void PlayPunchAirSFX()
    {
        punchAirGroup[Random.Range(0, punchAirGroup.Length)].Play();
    }

    public IEnumerator WaitTillSoundHasFinishedPlayingAndReturnPitchToOriginal(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.pitch = 1;
        yield break;
    }
}
