using Climbing;
using UnityEngine;

public class CoverController : MonoBehaviour
{
    public bool isInCover = false;
    private bool isEnteringCover = false;
    private bool isExitingCover = false;
    private ThirdPersonController controller;
    private AnimationCharacterController characterAnimation;
    private DetectionCharacterController characterDetection;

    void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        characterAnimation = controller.characterAnimation;
        characterDetection = controller.characterDetection;
    }

    private void Update()
    {
        CheckCover();
        HandleCoverEvents();
    }

    public void CheckCover()
    {
        bool wasInCover = isInCover;
        bool isCrouch = controller.isCrouch;

        if (!isCrouch)
        {
            isInCover = false;
        }
        else
        {
            RaycastHit[] hits;
            //isInCover = characterDetection.ThrowRayToCover(transform.position, out hit);
            isInCover = characterDetection.ThrowRaysToCover(transform.position, out hits, 5, 90);

        }

        if (isInCover != wasInCover)
        {
            if (isInCover)
            {
                isEnteringCover = true;
                isExitingCover = false;
            }
            else
            {
                isEnteringCover = false;
                isExitingCover = true;
            }
        }
        else
        {
            isEnteringCover = false;
            isExitingCover = false;
        }
    }

    private void HandleCoverEvents()
    {
        if (isEnteringCover)
        {
            OnPlayerEnterCover();
            isEnteringCover = false; // Resetea la bandera
        }

        if (isExitingCover)
        {
            OnPlayerExitCover();
            isExitingCover = false;
        }
    }

    private void OnPlayerEnterCover()
    {
        Debug.Log("Player entered cover");
        //animacion
        //limit movement to only left-right
        characterAnimation.switchCameras.CoverCam();
        SnapToCover();
        StealthManager.Instance.ActivateBlend();
    }

    private void OnPlayerExitCover()
    {
        Debug.Log("Player exited cover");
        characterAnimation.switchCameras.FreeLookCam();
        SnapOutOfCover();
        StealthManager.Instance.ExitBlendZone();
    }

    private void SnapToCover()
    {
        //a partir de ahora, el raycast tiene que ir en direccion a la pared, independientemente de la orientacion del jugador
        //asi el jugador se puede rotar y mover izq-derecha, pero manteniendo el cover y el snap contra la pared
    }

    private void SnapOutOfCover()
    {
        //Debug.Log("Snap out of cover");
    }
}
