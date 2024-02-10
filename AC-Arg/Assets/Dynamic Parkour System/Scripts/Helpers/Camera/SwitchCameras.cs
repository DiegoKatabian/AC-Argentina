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
            Vehicle // Agregamos el nuevo tipo de cámara
        }

        CameraType curCam = CameraType.None;

        [SerializeField] private CinemachineVirtualCameraBase FreeLook;
        [SerializeField] private CinemachineVirtualCameraBase Slide;
        [SerializeField] private CinemachineVirtualCameraBase Vehicle; // Agregamos referencia a la nueva cámara

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
                Slide.Priority = 0;
                FreeLook.Priority = 1;
                Vehicle.Priority = 0; // Aseguramos que la cámara Vehicle tenga prioridad 0
                curCam = CameraType.FreeLook; // Actualizamos el tipo de cámara actual
            }
        }

        //Switches To Slide Cam
        public void SlideCam()
        {
            if (curCam != CameraType.Slide)
            {
                FreeLook.Priority = 0;
                Slide.Priority = 1;
                Vehicle.Priority = 0; // Aseguramos que la cámara Vehicle tenga prioridad 0
                curCam = CameraType.Slide; // Actualizamos el tipo de cámara actual
            }
        }

        //Switches To Vehicle Cam
        public void VehicleCam()
        {
            if (curCam != CameraType.Vehicle)
            {
                FreeLook.Priority = 0;
                Slide.Priority = 0;
                Vehicle.Priority = 1;
                curCam = CameraType.Vehicle; // Actualizamos el tipo de cámara actual
            }
        }
    }
}
