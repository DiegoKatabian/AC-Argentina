using UnityEngine;

public enum Character
{
    Facundo,
    Varuzhan,
    Mother
}
[CreateAssetMenu(fileName = "DialogueEvent", menuName = "CtusceneEvents/DialogueEvent", order = 3)]
public class DialogueEvent : ScriptableObject, ICutsceneEvent
{
    [SerializeField] private Character character;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float delay;

    public Character Character => character;
    public AudioClip AudioClip => audioClip;
    public float Delay => delay;

    public void Execute()
    {
        // Implementación de la reproducción de diálogo
    }
}
