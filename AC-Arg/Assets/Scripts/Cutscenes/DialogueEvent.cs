using UnityEngine;

public enum Character
{
    Facundo,
    Varuzhan,
    Mother
}
[CreateAssetMenu(fileName = "DialogueEvent", menuName = "CutsceneEvents/DialogueEvent", order = 3)]
public class DialogueEvent : CutsceneEvent
{
    [SerializeField] private Character character;
    [SerializeField] private AudioClip audioClip;

    public Character Character => character;
    public AudioClip AudioClip => audioClip;

    public override void Execute()
    {
        //AudioSource auso = AudioManager.Instance.GetAudioSource(character);
        //auso.clip = audioClip;
        //auso.Play();
        Debug.Log("Dialogue Event: disparo el audioclip" + audioClip + " desde el character " + character);
    }
}
