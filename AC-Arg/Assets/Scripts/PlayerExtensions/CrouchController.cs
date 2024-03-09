using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchController : MonoBehaviour
{
    ThirdPersonController controller;
    AnimationCharacterController characterAnimation;

    bool isCrouch = false;
    private void Start()
    {
        controller = GetComponent<ThirdPersonController>();
    }

    void Update()
    {
        if(controller.characterInput.crouch)
        {
            Crouch();
        }
        else if (isCrouch)
        {
            UnCrouch();
        }
    }

    private void Crouch()
    {
        Debug.Log("me agacho");
        isCrouch = true;
        characterAnimation.Crouch();
    }
    private void UnCrouch()
    {
        Debug.Log("me levanto");
        characterAnimation.UnCrouch();
    }
}
