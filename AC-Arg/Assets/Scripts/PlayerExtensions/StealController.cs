using Climbing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealController : MonoBehaviour
{
    [SerializeField] PedestrianInteractionController pedestrianInteractionController;
    [SerializeField] ThirdPersonController controller;
    [SerializeField] Animator animator;
    [SerializeField] int currentMoney = 0;

    public int Money
    {
        get
        {
            return currentMoney;
        }
        set
        {
            currentMoney = value;
            EventManager.Instance.Trigger(Evento.OnMoneyUpdate, currentMoney);
        }
    }
    void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnInputRequestSteal, TrySteal);
    }

    private void TrySteal(params object[] parameters)
    {
        //Debug.Log("try steal...");

        if (!controller.isCrouch)
        {
            //Debug.Log("not crouching");
            return;
        }

        if (pedestrianInteractionController.isAlreadyInteracting)
        {
            //Debug.Log("already interacting");
            return;
        }

        if (pedestrianInteractionController.currentPedestrian == null)
        {
            //Debug.Log("no pedestrian");
            return;
        }

        if (!pedestrianInteractionController.currentPedestrian.canInteract)
        {
            //Debug.Log("cant interact with this pedestrian");
            return;
        }

        if (pedestrianInteractionController.IsBehindCurrentPedestrian())
        {
            StartSteal(pedestrianInteractionController.currentPedestrian);
        }
    }

    public void StartSteal(Pedestrian pedestrian)
    {
        Debug.Log("start steal");
        controller.RotatePlayerIndependentOfCamera(pedestrian.transform.position - transform.position);
        controller.DisableController();
        pedestrianInteractionController.isAlreadyInteracting = true;
        animator.CrossFade("Steal", 0.2f);
        pedestrian.GetStolenFrom();
    }   

    public void ANIMATION_OnStealEnd()
    {
        Debug.Log("el animator me dice que terminó la anim de steal");
        EndSteal();
    }

    public void EndSteal()
    {
        Debug.Log("end steal");
        pedestrianInteractionController.isAlreadyInteracting = false;
        controller.EnableController();
        Money += Random.Range(1, 10); //literal me aumenta la guitarra
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnInputRequestSteal, TrySteal);
        }
    }

}
