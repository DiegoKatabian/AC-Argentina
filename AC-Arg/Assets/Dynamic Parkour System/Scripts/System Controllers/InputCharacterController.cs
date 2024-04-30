using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Climbing
{
    public class InputCharacterController : MonoBehaviour
    {
        private PlayerControls controls = null;

        [HideInInspector] public Vector2 movement;
        [HideInInspector] public float changeCurrentEnemy;
        [HideInInspector] public bool run;
        [HideInInspector] public bool jump;
        [HideInInspector] public bool drop;
        [HideInInspector] public bool crouch;
        [HideInInspector] public bool leftHand;
        [HideInInspector] public bool rightHand;
        [HideInInspector] public bool block;


        private void OnEnable()
        {
            if (controls != null)
                controls.Enable();
        }

        private void OnDisable()
        {
            if (controls != null)
                controls.Disable();
        }

        void Awake()
        {
            //Hold and Release
            controls = new PlayerControls();
            controls.Player.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
            controls.Player.Movement.canceled += ctx => movement = ctx.ReadValue<Vector2>();
            controls.Player.Jump.performed += ctx => jump = ctx.ReadValueAsButton();
            controls.Player.Jump.canceled += ctx => jump = ctx.ReadValueAsButton();
            controls.Player.Drop.performed += ctx => drop = ctx.ReadValueAsButton();
            controls.Player.Drop.canceled += ctx => drop = ctx.ReadValueAsButton();
            controls.Player.Run.performed += ctx => run = ctx.ReadValueAsButton();
            controls.Player.Run.canceled += ctx => run = ctx.ReadValueAsButton();
            controls.Player.Block.performed += ctx => block = ctx.ReadValueAsButton();
            controls.Player.Block.canceled += ctx => block = ctx.ReadValueAsButton();
            controls.Player.Block.performed += ctx => OnBlockButtonPressed();
            controls.Player.Block.canceled += ctx => OnBlockButtonReleased();
            controls.GameManager.Exit.performed += ctx => Pause();
            controls.Player.Interact.performed += ctx => Interact();
            controls.Player.RequestBusStop.performed += ctx => OnBusStopButtonPressed();
            controls.Player.RequestBusStop.canceled += ctx => OnBusStopButtonReleased();
            controls.Player.Crouch.performed += ctx => crouch = ctx.ReadValueAsButton();
            controls.Player.Crouch.canceled += ctx => crouch = ctx.ReadValueAsButton();
            controls.Player.Crouch.performed += ctx => OnCrouchButtonPressed();
            controls.Player.Crouch.canceled += ctx => OnCrouchButtonReleased();
            controls.Player.ChangeCurrentEnemy.performed += ctx => changeCurrentEnemy = ctx.ReadValue<float>();
            controls.Player.ChangeCurrentEnemy.canceled += ctx => changeCurrentEnemy = ctx.ReadValue<float>();
            controls.Player.LeftHand.performed += ctx => LeftHandInput();
            controls.Player.RightHand.performed += ctx => RightHandInput();
            controls.Player.Steal.performed += ctx => OnStealButtonPressed();
            controls.Player.Steal.canceled += ctx => OnStealButtonReleased();
            controls.Player.Assassinate.performed += ctx => OnAssassinateButtonPressed();
            controls.Player.Assassinate.canceled += ctx => OnAssassinateButtonReleased();
        }

        private void OnBlockButtonPressed()
        {
            EventManager.Instance.Trigger(Evento.OnInputRequestBlock);
        }
        private void OnBlockButtonReleased()
        {
            EventManager.Instance.Trigger(Evento.OnInputReleaseBlock);
        }


        private void OnCrouchButtonPressed()
        {
            EventManager.Instance.Trigger(Evento.OnInputRequestCrouch);
        }
        private void OnCrouchButtonReleased()
        {
            EventManager.Instance.Trigger(Evento.OnInputReleaseCrouch);
        }

        private void LeftHandInput()
        {
            EventManager.Instance.Trigger(Evento.OnLeftHandInput);
        }

        private void RightHandInput()
        {
            EventManager.Instance.Trigger(Evento.OnRightHandInput);
        }

        //void ToggleRun()
        //{
        //    if (movement.magnitude > 0.2f && run == false)
        //        run = true;
        //    else
        //        run = false;
        //}

        void Pause()
        {
            Debug.Log("toque pause button");
            EventManager.Instance.Trigger(Evento.OnInputRequestPause);
        }

        void Interact()
        {
            Debug.Log("toque interact button");
            EventManager.Instance.Trigger(Evento.OnInputRequestInteract);
        }

        void OnBusStopButtonPressed()
        {
            Debug.Log("toque RequestBusStop button");
            EventManager.Instance.Trigger(Evento.OnInputRequestBusStop);
        }

        void OnStealButtonPressed()
        {
            Debug.Log("Steal button pressed");
            // Lógica para cuando se presiona el botón de robo
            EventManager.Instance.Trigger(Evento.OnInputRequestSteal);
        }

        void OnStealButtonReleased()
        {
            Debug.Log("Steal button released");
            EventManager.Instance.Trigger(Evento.OnInputReleaseSteal);

        }

        void OnAssassinateButtonPressed()
        {
            Debug.Log("toque assassinate button");
            EventManager.Instance.Trigger(Evento.OnInputRequestAssassinate);
        }

        void OnAssassinateButtonReleased()
        {
            EventManager.Instance.Trigger(Evento.OnInputReleaseAssassinate);
        }

        void OnBusStopButtonReleased()
        {
            Debug.Log("toque RequestBusStop button");
            EventManager.Instance.Trigger(Evento.OnInputReleaseBusStop);
        }
    }
}
