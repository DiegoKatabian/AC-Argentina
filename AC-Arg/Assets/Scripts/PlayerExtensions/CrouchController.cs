using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchController : MonoBehaviour
{
    ThirdPersonController controller;
    AnimationCharacterController characterAnimation;

    private void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        characterAnimation = controller.characterAnimation;
    }

    void Update()
    {
        if (controller.characterInput.crouch && !controller.isCrouch)
        {
            Crouch();
        }
        else if (!controller.characterInput.crouch && controller.isCrouch)
        {
            UnCrouch();
        }
    }

    private void Crouch()
    {
        Debug.Log("me agacho");
        controller.isCrouch = true;
        characterAnimation.Crouch();
    }
    private void UnCrouch()
    {
        Debug.Log("me levanto");
        controller.isCrouch = false;
        characterAnimation.UnCrouch();
    }
}
