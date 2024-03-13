﻿using System;
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


        private void OnEnable()
        {
            if(controls != null)
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
            controls.GameManager.Exit.performed += ctx => Pause();
            controls.Player.Interact.performed += ctx => Interact();
            controls.Player.RequestBusStop.performed += ctx => RequestBusStop();
            controls.Player.Crouch.performed += ctx => crouch = ctx.ReadValueAsButton();
            controls.Player.Crouch.canceled += ctx => crouch = ctx.ReadValueAsButton();
            controls.Player.ChangeCurrentEnemy.performed += ctx => changeCurrentEnemy = ctx.ReadValue<float>();
            controls.Player.ChangeCurrentEnemy.canceled += ctx => changeCurrentEnemy = ctx.ReadValue<float>();
            controls.Player.LeftHand.performed += ctx => LeftHandInput();
            controls.Player.RightHand.performed += ctx => RightHandInput();
        }

        private void LeftHandInput()
        {
            EventManager.Trigger(Evento.OnLeftHandInput);
        }

        private void RightHandInput()
        {
            EventManager.Trigger(Evento.OnRightHandInput);
        }

        void ToggleRun()
        {
            if (movement.magnitude > 0.2f && run == false)
                run = true;
            else
                run = false;
        }

        void Pause()
        {
            //Debug.Log("toque esc");
            EventManager.Trigger(Evento.OnPlayerPressedEsc);
        }

        void Interact()
        {
            //Debug.Log("toque E");
            EventManager.Trigger(Evento.OnPlayerPressedE);
        }

        void RequestBusStop()
        {
            Debug.Log("toque R");
            EventManager.Trigger(Evento.OnPlayerPressedR);
        }
    }

}