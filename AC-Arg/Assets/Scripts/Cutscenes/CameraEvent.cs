using UnityEngine;

[CreateAssetMenu(fileName = "CameraEvent", menuName = "CutsceneEvents/CameraEvent", order = 2)]
public class CameraEvent : ScriptableObject, ICutsceneEvent
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float delay;

    public Camera TargetCamera => targetCamera;
    public float Delay => delay;

    public void Execute()
    {
        // Implementación del cambio de cámara
    }
}
