using UnityEngine;
using Cinemachine;

public class CutsceneCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCameraBase cinemachineCamera;

    void Awake()
    {
        Debug.Log("CutsceneCamera: me despierto ;)");
    }

    public void Initialize()
    {
        if (cinemachineCamera != null)
        {
            Debug.Log("CutsceneCamera: me inicializo");
            CameraManager.Instance.RegisterCamera(transform.parent.name, cinemachineCamera);
            CameraManager.Instance.SetCutsceneCam(transform.parent.name, cinemachineCamera);
        }
        else
        {
            Debug.Log("CutsceneCamera: me quise inicializar pero mi cinemachineCamera era null");
        }
    }

    private void OnDestroy()
    {
        if (cinemachineCamera != null)
        {
            CameraManager.Instance.UnregisterCamera(transform.parent.name);
        }
    }
}
