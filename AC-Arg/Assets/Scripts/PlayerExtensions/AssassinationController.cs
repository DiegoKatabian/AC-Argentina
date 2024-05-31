using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinationController : MonoBehaviour
{
    public PedestrianInteractionController pedestrianInteractionController;
    public ThirdPersonController controller;

    public AudioClip hiddenBladeStabSound, hiddenBladeTakeOutSound;

    void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnInputRequestAssassinate, TryAssassination);
    }

    private void TryAssassination(params object[] parameters)
    {
        Debug.Log("try assassination...");

        //if (!controller.isCrouch)
        //{
        //    Debug.Log("not crouching");
        //    return;
        //}

        if (pedestrianInteractionController.isAlreadyInteracting)
        {
            Debug.Log("already interacting");
            return;
        }

        if (pedestrianInteractionController.currentPedestrian != null &&
            !pedestrianInteractionController.currentPedestrian.CanInteract())
        {
            Debug.Log("cant interact with this pedestrian");
            return;
        }


        if (pedestrianInteractionController.IsBehindCurrentPedestrian())
        {
            StartAssassination(pedestrianInteractionController.currentPedestrian);
        }
    }

    public void StartAssassination(IPedestrian pedestrian)
    {
        Debug.Log("start assassination");
        //controller.RotatePlayer(pedestrian.transform.position);
        controller.RotatePlayerIndependentOfCamera(pedestrian.gameObject.transform.position - transform.position);
        controller.DisableController();
        pedestrianInteractionController.isAlreadyInteracting = true;
        controller.characterAnimation.animator.CrossFade("Assassinate", 0.2f);
        EventManager.Instance.Trigger(Evento.OnAssassinationStart);
        pedestrian.GetAssassinated(this.gameObject);
    }

    public void ANIMATION_OnAssassinateEnd()
    {
        Debug.Log("el animator me dice que terminó la anim de assassinate");
        EndAssassination();
    }

    public void ANIMATION_PlayHiddenBladeSound()
    {
        AudioManager.Instance.PlaySound(hiddenBladeStabSound);
        AudioManager.Instance.PlayHurtSFX();

    }

    public void ANIMATION_PlayHiddenBladeTakeOutSound()
    {
        AudioManager.Instance.PlaySound(hiddenBladeTakeOutSound);
        AudioManager.Instance.PlayDeathSFX();
    }



    public void EndAssassination()
    {
        Debug.Log("end assassination");
        controller.EnableController();
        pedestrianInteractionController.isAlreadyInteracting = false;
        //trigger assassianation completed feedback
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnInputRequestAssassinate, TryAssassination);
        }
    }
}
