using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public PlayerHandHitbox leftHandHitBox;
    public float leftHandAttackDamage = 1;

    public PlayerHandHitbox rightHandHitBox;
    public float rightHandAttackDamage = 2;
    
    [HideInInspector] public bool isInCombatMode = false;
    [HideInInspector] public Enemy currentEnemy;
    [HideInInspector] public List<Enemy> detectedEnemies = new List<Enemy>();
    ThirdPersonController controller;
    bool areEnemiesDetected = false;
    bool handsAreOnCooldown = false;
    bool comboWindowOpen = false;

    public bool AreEnemiesDetected
    {
        get { return areEnemiesDetected; }
    }
    private void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        EventManager.Subscribe(Evento.OnLeftHandInput, PerformLeftHandAttack);
        EventManager.Subscribe(Evento.OnRightHandInput, PerformRightHandAttack);
    }
   
    private void Update()
    {
        if (controller.characterInput.changeCurrentEnemy != 0)
        {
            ChangeCurrentEnemy(controller.characterInput.changeCurrentEnemy);
        }
    }

    private void PerformLeftHandAttack(params object[] parameters)
    {
        if (handsAreOnCooldown)
        {
            //Debug.Log("mano izquierda en cooldown");
            return;
        }

        if (!isInCombatMode || currentEnemy == null)
        {
            //Debug.Log("no estoy en combate o no tengo enemigos");
            return;
        }

        if (comboWindowOpen)
        {
            PerformNextLeftHandComboAttack();
            return;
        }

        Debug.Log("ataco mano izquierda 1");
        controller.DisableController();
        controller.characterAnimation.animator.Play("Punch_Left_01", 0, 0);
        handsAreOnCooldown = true;
    }

    private void PerformRightHandAttack(object[] parameters)
    {
        if (handsAreOnCooldown)
        {
            //Debug.Log("mano izquierda en cooldown");
            return;
        }

        if (!isInCombatMode || currentEnemy == null)
        {
            //Debug.Log("no estoy en combate o no tengo enemigos");
            return;
        }

        if (comboWindowOpen)
        {
            PerformNextRightHandComboAttack();
            return;
        }

        Debug.Log("ataco mano derecha 1");
        controller.DisableController();
        controller.characterAnimation.animator.Play("Punch_Right_01", 0, 0);
        handsAreOnCooldown = true;
    }


    private void PerformNextLeftHandComboAttack()
    {
        Debug.Log("ataco mano izquierda 2");
        controller.DisableController();
        controller.characterAnimation.animator.Play("Punch_Left_02", 0, 0);
        handsAreOnCooldown = true;
        comboWindowOpen = false;
    }

    private void PerformNextRightHandComboAttack()
    {
        Debug.Log("ataco mano derecha 2");
        controller.DisableController();
        controller.characterAnimation.animator.Play("Punch_Right_02", 0, 0);
        handsAreOnCooldown = true;
        comboWindowOpen = false;
    }

    public void ANIMATION_OnLeftHandAttackHit()
    {
        //Debug.Log("animation: left hand attack 1 - hit");
        StartCoroutine(HitboxCouroutine(leftHandHitBox, leftHandAttackDamage));
        handsAreOnCooldown = false;
        comboWindowOpen = true;
    }

    public void ANIMATION_OnRightHandAttackHit()
    {
        //Debug.Log("animation: right hand attack 1 - hit");
        StartCoroutine(HitboxCouroutine(rightHandHitBox, rightHandAttackDamage));
        handsAreOnCooldown = false;
        comboWindowOpen = true;
    }

    public void ANIMATION_OnLeftHandAttackHit_EndCombo()
    {
        //Debug.Log("animation: left hand attack 2 - hit  - ends combo");
        StartCoroutine(HitboxCouroutine(leftHandHitBox, leftHandAttackDamage));
    }

    public void ANIMATION_OnRightHandAttackHit_EndCombo()
    {
        //Debug.Log("animation: right hand attack 2 - hit - ends combo");
        StartCoroutine(HitboxCouroutine(rightHandHitBox, rightHandAttackDamage));
    }

    public void ANIMATION_OnAttackEnd()
    {
        //Debug.Log("animation - attack ended");
        controller.EnableController();
        handsAreOnCooldown = false;
        comboWindowOpen = false;
    }

    IEnumerator HitboxCouroutine(PlayerHandHitbox hitbox, float damage)
    {
        ObjectEnabler.EnableObject(hitbox.gameObject, true);
        yield return new WaitForSeconds(0.2f);
        if (hitbox.isTaggedInside)
        {
            //Debug.Log("daño al enemy");
            EnemyManager.Instance.DamageEnemy(hitbox.affectedEnemy, damage);
        }
        ObjectEnabler.EnableObject(hitbox.gameObject, false);
    }

    public void CancelAllAttacks() //due to received damage
    {
        Debug.Log("cancelo los ataques porque recibí daño");
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
        isInCombatMode = true;
        //Debug.Log("entro a combat mode");
        //reemplazar anim de idle por idle-fight
    }
    public void ExitCombatMode()
    {
        isInCombatMode = false;
        //Debug.Log("salio de combat mode");
        //reemplazar anim de idle-fight por idle

    }
    public void UpdateDetectionStatus(Enemy lastDetectedEnemy)
    {
        if (detectedEnemies.Count > 0)
        {
            areEnemiesDetected = true;
            EnterCombatMode();
            //Debug.Log("enemies in volume");

            if (currentEnemy == null)
            {
                SetCurrentEnemy(lastDetectedEnemy);
                //Debug.Log("current enemy = " + currentEnemy.name);
            }
        }
        else
        {
            areEnemiesDetected = false;
            ExitCombatMode();
            SetCurrentEnemy(null);
            //Debug.Log("no enemies in volume");
        }
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
            ObjectEnabler.EnableObject(currentEnemy.isCurrentEnemyMarker, state);
        }
    }

    private void OnDestroy()
    {
        if(!gameObject.scene.isLoaded)
        {
            EventManager.Unsubscribe(Evento.OnLeftHandInput, PerformLeftHandAttack);
            EventManager.Unsubscribe(Evento.OnRightHandInput, PerformRightHandAttack);
        }
    }
}
