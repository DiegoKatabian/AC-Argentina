using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Climbing
{
    public class SwitchCameras : MonoBehaviour
    {
        Animator animator;

        enum CameraType
        {
            None,
            FreeLook,
            Slide,
            Vehicle,
            Cover,
            Combat
        }

        CameraType curCam = CameraType.None;

        [SerializeField] private CinemachineVirtualCameraBase freeLookCamera;
        [SerializeField] private CinemachineVirtualCameraBase slideCamera;
        [SerializeField] private CinemachineVirtualCameraBase vehicleCamera; // Agregamos referencia a la nueva cámara
        [SerializeField] private CinemachineVirtualCameraBase coverCamera; // Agregamos referencia a la nueva cámara
        [SerializeField] private CinemachineVirtualCameraBase combatCamera; // Agregamos referencia a la nueva cámara
        void Start()
        {
            animator = GetComponent<Animator>();
            FreeLookCam();
        }

        //Switches To FreeLook Cam
        public void FreeLookCam()
        {
            if (curCam != CameraType.FreeLook)
            {
                slideCamera.Priority = 0;
                freeLookCamera.Priority = 1;
                vehicleCamera.Priority = 0; // Aseguramos que la cámara Vehicle tenga prioridad 0
                coverCamera.Priority = 0;
                combatCamera.Priority = 0;
                curCam = CameraType.FreeLook; // Actualizamos el tipo de cámara actual
            }
        }

        //Switches To Slide Cam
        public void SlideCam()
        {
            if (curCam != CameraType.Slide)
            {
                freeLookCamera.Priority = 0;
                slideCamera.Priority = 1;
                vehicleCamera.Priority = 0; // Aseguramos que la cámara Vehicle tenga prioridad 0
                coverCamera.Priority = 0;
                combatCamera.Priority = 0;
                curCam = CameraType.Slide; // Actualizamos el tipo de cámara actual
            }
        }

        //Switches To Vehicle Cam
        public void VehicleCam(Transform targetVehicle)
        {
            if (curCam != CameraType.Vehicle)
            {
                freeLookCamera.Priority = 0;
                slideCamera.Priority = 0;
                vehicleCamera.Priority = 1;
                coverCamera.Priority = 0;
                combatCamera.Priority = 0;
                vehicleCamera.Follow = targetVehicle;
                vehicleCamera.LookAt = targetVehicle;
                curCam = CameraType.Vehicle; // Actualizamos el tipo de cámara actual
            }
        }

        public void CoverCam()
        {
            if (curCam != CameraType.Cover)
            {
                Debug.Log("cambio a cover cam");
                freeLookCamera.Priority = 0;
                slideCamera.Priority = 0;
                vehicleCamera.Priority = 0;
                coverCamera.Priority = 1;
                combatCamera.Priority = 0;
                curCam = CameraType.Cover; // Actualizamos el tipo de cámara actual
            }
        }

        public void CombatCam()
        {
            if (curCam != CameraType.Combat)
            {
                freeLookCamera.Priority = 0;
                slideCamera.Priority = 0;
                vehicleCamera.Priority = 0;
                coverCamera.Priority = 0;
                combatCamera.Priority = 1;
                curCam = CameraType.Combat; // Actualizamos el tipo de cámara actual
            }
        }

    }
}
