using UnityEngine;

[CreateAssetMenu(fileName = "CameraEvent", menuName = "CutsceneEvents/CameraEvent", order = 2)]
public class CameraEvent : CutsceneEvent
{
    [SerializeField] private GameObject cameraPrefab;

    public GameObject CameraPrefab => cameraPrefab;

    public override void Execute()
    {
        Debug.Log("CameraEvent: instancio la camara");
        GameObject cameraInstance = Instantiate(CameraPrefab);

        if (cameraInstance.GetComponentInChildren<CutsceneCamera>() != null)
        {
            Debug.Log("tiene cutscenecamera component");
            CutsceneCamera cutsceneCamera = cameraInstance.GetComponentInChildren<CutsceneCamera>();
            cutsceneCamera.Initialize();
        }
        else
        {
            Debug.Log("no tiene cutscenecamera component");
        }
    }
}
