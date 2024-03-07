using Climbing;
using UnityEngine;

public class CoverController : MonoBehaviour
{
    public bool isInCover = false;
    private bool isEnteringCover = false;
    private bool isExitingCover = false;
    private ThirdPersonController characterController;
    private AnimationCharacterController characterAnimation;
    private DetectionCharacterController characterDetection;

    void Start()
    {
        characterController = GetComponent<ThirdPersonController>();
        characterAnimation = characterController.characterAnimation;
        characterDetection = characterController.characterDetection;
    }

    private void Update()
    {
        CheckCover();
        HandleCoverEvents();
    }

    public void CheckCover()
    {
        RaycastHit hit;
        bool wasInCover = isInCover;
        isInCover = characterDetection.ThrowRayToCover(transform.position, out hit);

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
        //cambio de camara
        //limit movement to only left-right
        //snap to cover
    }

    private void OnPlayerExitCover()
    {
        Debug.Log("Player exited cover");
    }
}
