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
        controller.RotatePlayer(pedestrian.transform.position);
        //controller.RotatePlayerIndependentOfCamera(pedestrian.transform.position);
        //rotate towards pedestrian
        //disable movement
        //controller.characterAnimation.animator.CrossFade("Assassination", 0.2f);
        //pedestrian.GetAssassinated();
        StartCoroutine(AssassinationCoroutine());
    }

    public void EndAssassination()
    {
        Debug.Log("end assassination");
        //enable movement again
        //trigger assassianation completed feedback
    }

    public IEnumerator AssassinationCoroutine()
    {
        //while (controller.characterAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("Assassination"))
        //{
        //    yield return null;
        //}

        yield return new WaitForSeconds(1f);
        EndAssassination();
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnInputRequestAssassinate, TryAssassination);
        }
    }
}
