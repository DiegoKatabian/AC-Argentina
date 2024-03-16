﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Climbing
{
    [RequireComponent(typeof(InputCharacterController))]
    [RequireComponent(typeof(MovementCharacterController))]
    [RequireComponent(typeof(AnimationCharacterController))]
    [RequireComponent(typeof(DetectionCharacterController))]
    [RequireComponent(typeof(CameraController))]
    [RequireComponent(typeof(VaultingController))]

    public class ThirdPersonController : MonoBehaviour, ICrashable
    {
        [HideInInspector] public InputCharacterController characterInput;
        [HideInInspector] public MovementCharacterController characterMovement;
        [HideInInspector] public AnimationCharacterController characterAnimation;
        [HideInInspector] public DetectionCharacterController characterDetection;
        [HideInInspector] public VaultingController vaultingController;
        [HideInInspector] public CoverController coverController;
        [HideInInspector] public CombatController combatController;
        [HideInInspector] public VehicleInteractionController vehicleInteractionController;
        [HideInInspector] public bool isGrounded = false;
        [HideInInspector] public bool allowMovement = true;
        [HideInInspector] public bool onAir = false;
        [HideInInspector] public bool isJumping = false;
        [HideInInspector] public bool inSlope = false;
        [HideInInspector] public bool isVaulting = false;
        [HideInInspector] public bool dummy = false;
        [HideInInspector] public bool isOnVehicle = false;
        [HideInInspector] public Rigidbody vehicleBelowMe;
        [HideInInspector] public bool isCrouch = false;
        [HideInInspector] public bool isHurting = false;
        [HideInInspector] public bool isCrashing = false;

        [Header("Cameras")]
        public CameraController cameraController;
        public Transform mainCamera;
        public Transform freeCamera;

        [Header("Step Settings")]
        [Range(0, 10.0f)] public float stepHeight = 0.8f;
        public float stepVelocity = 0.2f;

        [Header("Colliders")]
        public CapsuleCollider normalCapsuleCollider;
        public CapsuleCollider slidingCapsuleCollider;

        private float turnSmoothTime = 0.1f;
        private float turnSmoothVelocity;

        private void Awake()
        {
            characterInput = GetComponent<InputCharacterController>();
            characterMovement = GetComponent<MovementCharacterController>();
            characterAnimation = GetComponent<AnimationCharacterController>();
            characterDetection = GetComponent<DetectionCharacterController>();
            vaultingController = GetComponent<VaultingController>();
            coverController = GetComponent<CoverController>();
            combatController = GetComponent<CombatController>();
            vehicleInteractionController = GetComponent<VehicleInteractionController>();

            if (cameraController == null)
                Debug.LogError("Attach the Camera Controller located in the Free Look Camera");
        }

        private void Start()
        {
            characterMovement.OnLanded += characterAnimation.Land;
            characterMovement.OnFall += characterAnimation.Fall;
        }

        void Update()
        {
            //Detect if Player is on Ground
            isGrounded = OnGround();

            //Get Input if controller and movement are not disabled
            if (!dummy && allowMovement)
            {
                AddMovementInput(characterInput.movement);

                if (coverController.isInCover)
                {
                    ToggleWalk();
                }
                else if (characterInput.movement.magnitude > 0.5f)
                {
                    ToggleRun();
                }
            }
        }

        private void FixedUpdate()
        {
            if (isOnVehicle)
            {
                //Debug.Log("FixedUpdate: agrego velocity = " + vehicleBelowMe.velocity);
                characterMovement.rb.AddForce(vehicleBelowMe.velocity, ForceMode.VelocityChange);
            }
        }

        private bool OnGround()
        {
            return characterDetection.IsGrounded(stepHeight);
        }
        public void AddMovementInput(Vector2 direction)
        {
            Vector3 translation = Vector3.zero;
            translation = GroundMovement(direction);
            characterMovement.SetVelocity(Vector3.ClampMagnitude(translation, 1.0f)); //deberia sumar la velocity del auto aca
        }
        Vector3 GroundMovement(Vector2 input)
        {
            Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;

            //Gets direction of movement relative to the camera rotation
            freeCamera.eulerAngles = new Vector3(0, mainCamera.eulerAngles.y, 0);
            Vector3 translation = freeCamera.transform.forward * input.y + freeCamera.transform.right * input.x;
            translation.y = 0;

            //Detects if player is moving to any direction
            if (translation.magnitude > 0)
            {
                if (combatController.isInCombatMode)
                {
                    RotatePlayerIndependentOfCamera(combatController.currentEnemy.transform.position - transform.position);
                    characterAnimation.animator.SetBool("Released", false);
                }
                else
                {
                    RotatePlayer(direction);
                    characterAnimation.animator.SetBool("Released", false);
                }
            }
            else
            {
                ToggleWalk();
                characterAnimation.animator.SetBool("Released", true);
            }

            return translation;
        }
        public void RotatePlayer(Vector3 direction)
        {
            //Get direction with camera rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            //Rotate Mesh to Movement
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        public void RotatePlayerIndependentOfCamera(Vector3 direction)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        public Quaternion RotateToCameraDirection(Vector3 direction)
        {
            //Get direction with camera rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            //Rotate Mesh to Movement
            return Quaternion.Euler(0f, targetAngle, 0f);
        }
        public void ResetMovement()
        {
            characterMovement.ResetSpeed();
        }
        public void ToggleRun()
        {
            if (characterMovement.GetState() != MovementState.Running)
            {
                characterMovement.SetCurrentState(MovementState.Running);
                characterMovement.curSpeed = characterMovement.RunSpeed;
                characterAnimation.animator.SetBool("Run", true);
            }
        }
        public void ToggleWalk()
        {
            if (characterMovement.GetState() != MovementState.Walking)
            {
                characterMovement.SetCurrentState(MovementState.Walking);
                characterMovement.curSpeed = characterMovement.walkSpeed;
                characterAnimation.animator.SetBool("Run", false);
            }
        }

        public void StartHurt()
        {
            Debug.Log("ouch me pegaron");
            isHurting = true;
            combatController.CancelAllAttacks();
            DisableController();
            characterAnimation.animator.CrossFade("Hurt", 0.1f);
            //StartCoroutine(HurtRecoveryCouroutine());
        }

        public void ANIMATION_OnHurtEnd()
        {
            isHurting = false;
            combatController.ResetCooldowns();
            EnableController();
        }

        IEnumerator HurtRecoveryCouroutine()
        {
            Debug.Log("una lloradita y a seguir...");
            yield return new WaitForSeconds(1);
            Debug.Log("bueno listo, ya esta");
            isHurting = false;
            combatController.ResetCooldowns();
            EnableController();
        }

        public void EnterVehicleRoof(Rigidbody veh)
        {
            vehicleBelowMe = veh;
            isOnVehicle = true;
            Debug.Log("is on vehicle true");
        }
        public void ExitVehicleRoof()
        {
            isOnVehicle = false;
            Debug.Log("is on vehicle false");
        }

        public void OnCrash(GameObject vehicle, float crashForce)
        {
            if (isCrashing)
            {
                Debug.Log("ya estoy chocado, banca hermano");
                return;
            }

            isCrashing = true;
            Debug.Log("me choco con un auto");
            characterMovement.OnVehicleCrash(vehicle, crashForce);
            characterAnimation.StartCrashAnimation();
            allowMovement = false;
            //DisableController();
        }

        public void ANIMATION_OnCrashEnd()
        {
            Debug.Log("player: terminó la animacion de crash");
            characterAnimation.EndCrashAnimation();
            allowMovement = true;
            isCrashing = false;
            //EnableController();
        }
        public float GetCurrentVelocity()
        {
            return characterMovement.GetVelocity().magnitude;
        }
        public void DisableController()
        {
            characterMovement.SetKinematic(true);
            characterMovement.enableFeetIK = false;
            dummy = true;
            allowMovement = false;

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
        }
        public void EnableController()
        {
            characterMovement.SetKinematic(false);
            characterMovement.EnableFeetIK();
            characterMovement.ApplyGravity();
            characterMovement.stopMotion = false;
            dummy = false; 
            allowMovement = true;

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }
        }
    }
}