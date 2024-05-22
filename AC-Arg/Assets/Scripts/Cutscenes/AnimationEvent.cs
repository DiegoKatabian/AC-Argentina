using UnityEngine;

[CreateAssetMenu(fileName = "AnimationEvent", menuName = "CutsceneEvents/AnimationEvent", order = 1)]
public class AnimationEvent : ScriptableObject, ICutsceneEvent
{
    [SerializeField] private GameObject targetModel;
    [SerializeField] private string animationName;
    [SerializeField] private float delay;

    public GameObject TargetModel => targetModel;
    public string AnimationName => animationName;
    public float Delay => delay;

    public void Execute()
    {
        // Implementación de la reproducción de animación
    }
}
