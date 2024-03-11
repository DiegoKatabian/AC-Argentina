using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public PlayerHandHitbox leftHandHitBox;
    public float leftHandAttackDamage = 3;
    
    [HideInInspector] public bool isInCombatMode = false;
    [HideInInspector] public Enemy currentEnemy;
    [HideInInspector] public List<Enemy> detectedEnemies = new List<Enemy>();
    ThirdPersonController controller;
    bool areEnemiesDetected = false;
    bool leftHandIsOnCooldown = false;
    bool comboWindowOpen = false;

    public bool AreEnemiesDetected
    {
        get { return areEnemiesDetected; }
    }
    private void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        EventManager.Subscribe(Evento.OnLeftHandInput, PerformLeftHandAttack);
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
        
        if (leftHandIsOnCooldown)
        {
            Debug.Log("mano izquierda en cooldown");
            return;
        }

        if (!isInCombatMode || currentEnemy == null)
        {
            //Debug.Log("no estoy en combate o no tengo enemigos");
            return;
        }

        if (comboWindowOpen)
        {
            PerformNextComboAttack();
            return;
        }

        Debug.Log("ataco mano izquierda 1");
        controller.DisableController();
        controller.characterAnimation.animator.Play("Punch_Left_01", 0, 0);
        leftHandIsOnCooldown = true;
    }

    private void PerformNextComboAttack()
    {
        Debug.Log("ataco mano izquierda 2");
        controller.DisableController();
        controller.characterAnimation.animator.Play("Punch_Left_02", 0, 0);
        leftHandIsOnCooldown = true;
        comboWindowOpen = false;
        //por ahora ambos ataques abren la misma hitbox con el mismo daño
    }

    public void OnAttackAnimationHit()
    {
        Debug.Log("attack animation hit");
        StartCoroutine(HitboxCouroutine());
        leftHandIsOnCooldown = false;
        comboWindowOpen = true;
    }

    public void OnAttackAnimationHitNoCombo()
    {
        Debug.Log("attack animation hit");
        StartCoroutine(HitboxCouroutine());
    }

    public void OnAttackAnimationEnded()
    {
        Debug.Log("on attack animation ended");
        controller.EnableController();
        leftHandIsOnCooldown = false;
        comboWindowOpen = false;
    }

    IEnumerator HitboxCouroutine()
    {
        ObjectEnabler.EnableObject(leftHandHitBox.gameObject, true);
        yield return new WaitForSeconds(0.2f);
        if (leftHandHitBox.isTaggedInside)
        {
            //Debug.Log("daño al enemy");
            EnemyManager.Instance.DamageEnemy(leftHandHitBox.affectedEnemy, leftHandAttackDamage);
        }
        ObjectEnabler.EnableObject(leftHandHitBox.gameObject, false);
    }

    private void ChangeCurrentEnemy(float v)
    {
        if (detectedEnemies.Count < 2)
        {
            Debug.Log("no hay enemigos para ciclar");
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
        }
    }
}
