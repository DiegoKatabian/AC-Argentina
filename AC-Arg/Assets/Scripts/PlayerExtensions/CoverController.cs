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
        // Lanzamos el raycast con la longitud adecuada
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
        SnapToCover();
    }

    private void OnPlayerExitCover()
    {
        Debug.Log("Player exited cover");
        SnapOutOfCover();
    }

    private void SnapToCover()
    {
        //a partir de ahora, el raycast tiene que ir en direccion a la pared, independientemente de la orientacion del jugador
        //asi el jugador se puede rotar y mover izq-derecha, pero manteniendo el cover y el snap contra la pared
    }

    private void SnapOutOfCover()
    {
        Debug.Log("Snap out of cover");
    }
}
