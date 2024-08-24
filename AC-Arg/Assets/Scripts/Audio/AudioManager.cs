using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    Dictionary<AudioSource, AudioClip> allSounds = new Dictionary<AudioSource, AudioClip>();

    public AudioSource[] deathMaleGroup, hurtMaleGroup;
    public AudioSource[] punchAirGroup, punchHitGroup, punchBlockGroup;
    public AudioSource[] footstepGroup;
    public AudioSource[] musicGroup;
    public AudioSource combatMusic, endCombatMusic;
    public AudioSource leapSound;
    public AudioSource ezioTango;


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
                //Debug.Log("le doy play");
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

    internal void PlayPunchBlockedSFX()
    {
        PlaySound(allSounds[punchBlockGroup[Random.Range(0, punchBlockGroup.Length)]], 1, 0.1f);
    }

    public void PlayFootstepSFX()
    {
        Debug.Log("audiomanager: ok, play footstep");
        footstepGroup[Random.Range(0, footstepGroup.Length)].Play();
    }

    //y si me pasaras por parametro tu ubicacion, yo te pondria al audiosource ahi
    public void PlayFootstepAtPosition(Vector3 position)
    {
        footstepGroup[Random.Range(0, footstepGroup.Length)].transform.position = position;
        footstepGroup[Random.Range(0, footstepGroup.Length)].Play();
    }

    public void StopAllMusic()
    {
        foreach (AudioSource audioSource in musicGroup)
        {
            audioSource.Stop();
        }
    }

    public void StopEzioTango()
    {
       ezioTango.Stop();
    }

    public void StopCombatMusic()
    {
       combatMusic.Stop();
    }

    public void FadeOutEzioTango()
    {
        StartCoroutine(FadeOut(ezioTango, 2));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float originalVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = originalVolume;
    }

    public void EndCombatMusic()
    {
        combatMusic.Stop();
        endCombatMusic.Play();
    }

    internal void PlayLeapSFX()
    {
        leapSound.Play();
    }

    internal bool IsPlaying(AudioClip audioClip)
    {
        //return whether that clip is playing or not
        foreach (AudioSource audioSource in allSounds.Keys)
        {
            if (allSounds[audioSource] == audioClip)
            {
                return audioSource.isPlaying;
            }
        }

        Debug.Log("audiomanager: isplaying method returned false because there was no audiosource associated to that clip");
        return false;
    }
}
