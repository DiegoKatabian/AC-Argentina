using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "CameraEvent", menuName = "CutsceneEvents/CameraEvent", order = 2)]
public class CameraEvent : CutsceneEvent, ICutsceneEvent
{
    [SerializeField] private Camera targetCamera;

    public Camera TargetCamera => targetCamera;

    public override void Execute()
    {
        //SwitchCameras.SetCutsceneCamera(targetCamera);
        Debug.Log("Camera Event: cambio a la camara " + targetCamera);
    }
}
