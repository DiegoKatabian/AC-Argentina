using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Climbing
{
    public class SwitchCameras : MonoBehaviour
    {
        Animator animator;
        CinemachineFreeLook freeLook;

        enum CameraType
        {
            None,
            FreeLook,
            Slide,
            Vehicle,
            Cover,
            Combat,
            Cutscene
        }

        CameraType curCam = CameraType.None;

        [SerializeField] private CinemachineVirtualCameraBase freeLookCamera;
        [SerializeField] private CinemachineVirtualCameraBase slideCamera;
        [SerializeField] private CinemachineVirtualCameraBase vehicleCamera;
        [SerializeField] private CinemachineVirtualCameraBase coverCamera;
        [SerializeField] private CinemachineVirtualCameraBase combatCamera;
        [SerializeField] private CinemachineVirtualCameraBase cutsceneCamera;



        private Dictionary<CameraType, CinemachineVirtualCameraBase> cameraDict;

        void Start()
        {
            animator = GetComponent<Animator>();
            freeLook = freeLookCamera.GetComponent<CinemachineFreeLook>();

            cameraDict = new Dictionary<CameraType, CinemachineVirtualCameraBase>
            {
                { CameraType.FreeLook, freeLookCamera },
                { CameraType.Slide, slideCamera },
                { CameraType.Vehicle, vehicleCamera },
                { CameraType.Cover, coverCamera },
                { CameraType.Combat, combatCamera },
                { CameraType.Cutscene, cutsceneCamera }
            };

            SwitchCamera(CameraType.FreeLook);
        }

        private void SwitchCamera(CameraType newCameraType)
        {
            if (curCam != newCameraType)
            {
                foreach (var cam in cameraDict.Values)
                {
                    cam.Priority = 0;
                }

                cameraDict[newCameraType].Priority = 1;
                curCam = newCameraType;
            }
        }

        public void FreeLookCam()
        {
            SwitchCamera(CameraType.FreeLook);
        }

        public void SlideCam()
        {
            SwitchCamera(CameraType.Slide);
        }

        public void VehicleCam(Transform targetVehicle)
        {
            if (curCam != CameraType.Vehicle)
            {
                vehicleCamera.Follow = targetVehicle;
                vehicleCamera.LookAt = targetVehicle;
                SwitchCamera(CameraType.Vehicle);
            }
        }

        public void CoverCam()
        {
            SwitchCamera(CameraType.Cover);
        }

        public void CombatCam()
        {
            SwitchCamera(CameraType.Combat);
        }

        public void CutsceneCam()
        {
            freeLook.m_XAxis.m_MaxSpeed = 0;
            freeLook.m_YAxis.m_MaxSpeed = 0;
            SwitchCamera(CameraType.Cutscene);
        }

        public void SetCutsceneCam(Camera targetCamera)
        {
            //todo: setea la current cutscene camera
        }
    }
}
