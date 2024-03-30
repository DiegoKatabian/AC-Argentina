using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinationController : MonoBehaviour
{
    public PedestrianInteractionController pedestrianInteractionController;
    public ThirdPersonController controller;

    void Start()
    {
        EventManager.Subscribe(Evento.OnInputRequestAssassinate, TryAssassination);
    }

    private void TryAssassination(params object[] parameters)
    {
        Debug.Log("try assassination...");

        if (!controller.isCrouch)
        {
            Debug.Log("not crouching");
            return;
        }

        if (pedestrianInteractionController.IsBehindCurrentPedestrian())
        {
            StartAssassination(pedestrianInteractionController.currentPedestrian);
        }
    }

    public void StartAssassination(Pedestrian pedestrian)
    {
        Debug.Log("start assassination");
        //controller.RotatePlayer(pedestrian.transform.position);
        controller.RotatePlayerIndependentOfCamera(pedestrian.transform.position - transform.position);
        controller.DisableController();
        controller.characterAnimation.animator.CrossFade("Assassinate", 0.2f);
        pedestrian.GetAssassinated();
    }

    public void ANIMATION_OnAssassinateEnd()
    {
        Debug.Log("el animator me dice que terminó la anim de assassinate");
        EndAssassination();
    }



    public void EndAssassination()
    {
        Debug.Log("end assassination");
        controller.EnableController();
        //trigger assassianation completed feedback
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnInputRequestAssassinate, TryAssassination);
        }
    }
}
