using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public PlayerHandHitbox leftHandHitBox;
    public float leftHandAttackDamage = 1;

    [HideInInspector] public bool isInCombatMode = false;
    [HideInInspector] public bool isBlocking = false;
    [HideInInspector] public Enemy currentEnemy;
    [HideInInspector] public List<Enemy> detectedEnemies = new List<Enemy>();
    ThirdPersonController controller;
    bool areEnemiesDetected = false;
    bool handsAreOnCooldown = false;
    bool comboWindowOpen = false;
    int leftHandComboStep;

    public bool AreEnemiesDetected
    {
        get { return areEnemiesDetected; }
    }
    private void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        EventManager.Instance.Subscribe(Evento.OnLeftHandInput, PerformLeftHandAttack);
        EventManager.Instance.Subscribe(Evento.OnInputRequestBlock, RequestBlock);
        EventManager.Instance.Subscribe(Evento.OnInputReleaseBlock, ReleaseBlock);
    }
    private void Update()
    {
        if (controller.characterInput.changeCurrentEnemy != 0)
        {
            ChangeCurrentEnemy(controller.characterInput.changeCurrentEnemy);
        }

        if (AreEnemiesDetected && EnemyManager.Instance.AreEnemiesInCombat())
        {
            EnterCombatMode();
        }
    }


    private void ReleaseBlock(object[] parameters)
    {
        Debug.Log("suelto el bloqueo");
        isBlocking = false;
        controller.characterAnimation.animator.SetBool("isBlocking", isBlocking);

        handsAreOnCooldown = false;
    }

    private void RequestBlock(object[] parameters)
    {
        if (handsAreOnCooldown)
        {
            Debug.Log("mano en cooldown");
            return;
        }
        if (!isInCombatMode)
        {
            Debug.Log("no estoy en combate");
            return;
        }
        if (isBlocking)
        {
            Debug.Log("ya estoy bloqueando");
            return;
        }

        Debug.Log("bloqueo");
        isBlocking = true;
        handsAreOnCooldown = true;
        controller.characterAnimation.animator.SetBool("isBlocking", isBlocking);
    }


    private void PerformLeftHandAttack(params object[] parameters)
    {
        if (handsAreOnCooldown)
        {
            return;
        }

        if (!isInCombatMode || currentEnemy == null)
        {
            return;
        }

        if (comboWindowOpen)
        {
            PerformNextLeftHandComboAttack();
            return;
        }

        controller.DisableController();
        controller.characterAnimation.animator.CrossFade("Punch_01", 0.1f);
        AudioManager.Instance.PlayPunchAirSFX();
        handsAreOnCooldown = true;
    }

    private void PerformNextLeftHandComboAttack()
    {
        controller.DisableController();

        if (leftHandComboStep == 0)
        {
            controller.characterAnimation.animator.CrossFade("Punch_02", 0.1f);
        }
        else if (leftHandComboStep == 1)
        {
            controller.characterAnimation.animator.CrossFade("Punch_03", 0.1f);
            comboWindowOpen = false;
        }

        AudioManager.Instance.PlayPunchAirSFX();
        handsAreOnCooldown = true;
        leftHandComboStep++;
    }

    public void ANIMATION_OnAttackHit()
    {
        StartCoroutine(HitboxCouroutine(leftHandHitBox, leftHandAttackDamage));
        handsAreOnCooldown = false;
        comboWindowOpen = true;
        //leftHandComboStep = 0;
    }

    public void ANIMATION_OnAttackHit_EndCombo()
    {
        StartCoroutine(HitboxCouroutine(leftHandHitBox, leftHandAttackDamage));
    }

    public void ANIMATION_OnAttackEnd()
    {
        controller.EnableController();
        handsAreOnCooldown = false;
        comboWindowOpen = false;
        leftHandComboStep = 0;
    }
    IEnumerator HitboxCouroutine(PlayerHandHitbox hitbox, float damage)
    {
        ObjectEnabler.EnableObject(hitbox.gameObject, true);
        yield return new WaitForSeconds(0.1f);
        if (hitbox.isTaggedInside)
        {
            AudioManager.Instance.PlayPunchHitSFX();
            EnemyManager.Instance.DamageEnemy(hitbox.affectedEnemy, damage);
        }
        ObjectEnabler.EnableObject(hitbox.gameObject, false);
    }
    public void CancelAllAttacks() //due to received damage
    {
        //Debug.Log("cancelo los ataques porque recibí daño");
        //controller.EnableController();
        handsAreOnCooldown = true;
        comboWindowOpen = false;
    }
    public void ResetCooldowns()
    {
        handsAreOnCooldown = false;
    }

    private void ChangeCurrentEnemy(float v)
    {
        if (detectedEnemies.Count < 2)
        {
            //Debug.Log("no hay enemigos para ciclar");
            return;
        }

        int currentIndex = detectedEnemies.IndexOf(currentEnemy);
        if (v > 0)
        {
            if (currentIndex + 1 < detectedEnemies.Count)
            {
                SetCurrentEnemy(detectedEnemies[currentIndex + 1]);
            }
            else
            {
                SetCurrentEnemy(detectedEnemies[0]);
            }
        }
        else if (v < 0)
        {
            if (currentIndex - 1 >= 0)
            {
                SetCurrentEnemy(detectedEnemies[currentIndex - 1]);
            }
            else
            {
                SetCurrentEnemy(detectedEnemies[detectedEnemies.Count - 1]);
            }
        }
    }
    public void EnterCombatMode()
    {
        //solo entro a combat si el enemymanager confirma que algun enemigo esta en combate conmigo (es decir, alert o warning)
       

        isInCombatMode = true;
        controller.characterAnimation.switchCameras.CombatCam();
        controller.characterAnimation.animator.SetBool("isInCombatMode", isInCombatMode);
        //Debug.Log("entro a combat mode");
    }
    public void ExitCombatMode()
    {
        isInCombatMode = false;
        controller.characterAnimation.switchCameras.FreeLookCam();
        controller.characterAnimation.animator.SetBool("isInCombatMode", isInCombatMode);
        //Debug.Log("salgo de combat mode");

    }
    public void UpdateDetectionStatus(Enemy lastDetectedEnemy)
    {
        //Debug.Log("detected enemies count = " + detectedEnemies.Count);

        if (detectedEnemies.Count == 0)
        {
            //Debug.Log("no hay enemies in volume");
            areEnemiesDetected = false;
            ExitCombatMode();
            SetCurrentEnemy(null);
        }
        else
        {
            if (areEnemiesDetected)
            {
                //Debug.Log("detecto uno nuevo, pero ya habia enemigos, sigue todo igual");
            }
            else
            {
                //Debug.Log("primer enemigo detectado");
                areEnemiesDetected = true;

                if (currentEnemy == null)
                {
                    SetCurrentEnemy(lastDetectedEnemy);
                    //Debug.Log("current enemy = " + currentEnemy.name);
                }
            }
        }
    }
    public void AddEnemyToDetectedList(Enemy detectedEnemy)
    {
        //return iuf the enbemy is knocked out
        if (detectedEnemy.isKnockedOut)
        {
            return;
        }

        if (!detectedEnemies.Contains(detectedEnemy))
        {
            detectedEnemies.Add(detectedEnemy);
            UpdateDetectionStatus(detectedEnemy);
            //Debug.Log("Enemy Detected");
        }
    }
    public void RemoveEnemyFromDetectedList(Enemy detectedEnemy)
    {
        detectedEnemies.Remove(detectedEnemy);
        UpdateDetectionStatus(detectedEnemy);
        //Debug.Log("Enemy Lost");
    }
    public void SetCurrentEnemy(Enemy enemy)
    {
        CurrentEnemyMarkerToggler(false);
        currentEnemy = enemy;
        CurrentEnemyMarkerToggler(true);
    }
    private void CurrentEnemyMarkerToggler(bool state)
    {
        if (currentEnemy != null)
        {
            ObjectEnabler.EnableObject(currentEnemy.currentEnemyMarker, state);
        }
    }

    public void HandleEnemyKnockout(Enemy knockedOutEnemy)
    {
        RemoveEnemyFromDetectedList(knockedOutEnemy);
        if (currentEnemy == knockedOutEnemy)
        {
            SetCurrentEnemy(detectedEnemies.Count > 0 ? detectedEnemies[0] : null);
        }
    }


    private void OnDestroy()
    {
        if(!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnLeftHandInput, PerformLeftHandAttack);
        }
    }
}
