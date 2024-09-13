using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Climbing
{
    public class SwitchCameras : MonoBehaviour
    {
        private Animator animator;
        private CinemachineFreeLook freeLook;

        private Dictionary<string, CinemachineVirtualCameraBase> cameraDict = new Dictionary<string, CinemachineVirtualCameraBase>();
        private string curCam = null;

        [SerializeField] private CinemachineVirtualCameraBase freeLookCamera;
        [SerializeField] private CinemachineVirtualCameraBase slideCamera;
        [SerializeField] private CinemachineVirtualCameraBase vehicleCamera;
        [SerializeField] private CinemachineVirtualCameraBase coverCamera;
        [SerializeField] private CinemachineVirtualCameraBase combatCamera;

        void Start()
        {
            animator = GetComponent<Animator>();
            freeLook = freeLookCamera.GetComponent<CinemachineFreeLook>();

            RegisterCamera("FreeLook", freeLookCamera);
            RegisterCamera("Slide", slideCamera);
            RegisterCamera("Vehicle", vehicleCamera);
            RegisterCamera("Cover", coverCamera);
            RegisterCamera("Combat", combatCamera);

            EnableCamera("FreeLook");

            // Set blend style for all cameras
            foreach (var cam in cameraDict.Values)
            {
                var brain = cam.gameObject.GetComponent<CinemachineBrain>();
                if (brain != null)
                {
                    brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
                }
            }
        }

        private void RegisterCamera(string key, CinemachineVirtualCameraBase camera)
        {
            if (!cameraDict.ContainsKey(key))
            {
                cameraDict.Add(key, camera);
            }
        }

        public void DisableAllCameras()
        {
            //Debug.Log("SwitchCamera: deshabilito todas");
            foreach (var cam in cameraDict.Values)
            {
                cam.Priority = 0;
            }
        }

        public void EnableCamera(string newCameraKey)
        {
            if (cameraDict.ContainsKey(newCameraKey))
            {
                //Debug.Log("SwitchCamera: habilito la camara que me piden");

                DisableAllCameras();
                cameraDict[newCameraKey].Priority = 1;
                curCam = newCameraKey;
            }
            else
            {
                Debug.Log("SwitchCamera: enable camera failed");
            }
        }

        public void FreeLookCam() => EnableCamera("FreeLook");

        public void SlideCam() => EnableCamera("Slide");

        public void VehicleCam(Transform targetVehicle)
        {
            if (curCam != "Vehicle")
            {
                vehicleCamera.Follow = targetVehicle;
                vehicleCamera.LookAt = targetVehicle;
                EnableCamera("Vehicle");
            }
        }

        public void CoverCam() => EnableCamera("Cover");

        public void CombatCam() => EnableCamera("Combat");
    }
}
