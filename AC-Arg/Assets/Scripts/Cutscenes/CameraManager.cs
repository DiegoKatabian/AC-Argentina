using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Climbing;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private SwitchCameras switchCameras;

    private Dictionary<string, CinemachineVirtualCameraBase> cameraDict = new Dictionary<string, CinemachineVirtualCameraBase>();
    private string curCam = null;

    public void RegisterCamera(string key, CinemachineVirtualCameraBase camera)
    {
        if (!cameraDict.ContainsKey(key))
        {
            Debug.Log("CameraManager: agrego camera " + key + " al dict");
            cameraDict.Add(key, camera);
        }
        else
        {
            Debug.Log("CameraManager: la camara ya estaba en el dict");
        }
    }

    public void UnregisterCamera(string key)
    {
        if (cameraDict.ContainsKey(key))
        {
            Debug.Log("CameraManager: quito camera " + key + "  al dict");
            cameraDict.Remove(key);
        }
    }

    public void SwitchToCutsceneCamera(string newCameraKey)
    {
        if (curCam != newCameraKey && cameraDict.ContainsKey(newCameraKey))
        {
            Debug.Log("CameraManager: cambio a camara de cutscene");
            if (switchCameras != null)
            {
                switchCameras.DisableAllCameras();
            }

            foreach (var cam in cameraDict.Values)
            {
                cam.Priority = 0;
            }

            cameraDict[newCameraKey].Priority = 1;
            curCam = newCameraKey;
        }
        else
        {
            Debug.Log("switch to cutscene camera falló");
        }
    }

    public void SwitchToPlayerCamera()
    {
        Debug.Log("CameraManager: cambio a camara de jugador");
        
        //disable all my cameras
        foreach (var cam in cameraDict.Values)
        {
            cam.Priority = 0;
        }

        switchCameras.FreeLookCam();
    }

    public void SetCutsceneCam(string cameraKey, CinemachineVirtualCameraBase cutsceneCamera)
    {
        Debug.Log("CameraManager: seteo camara de cutscene");
        //only register the camera if it's not already registered
        if (!cameraDict.ContainsKey(cameraKey))
        {
            Debug.Log("epa, no estaba registrado. lo registro");
            RegisterCamera(cameraKey, cutsceneCamera);
        }
        else
        {
               Debug.Log("bien ahi, ya estaba registrado");
        }
        SwitchToCutsceneCamera(cameraKey);
    }
}
