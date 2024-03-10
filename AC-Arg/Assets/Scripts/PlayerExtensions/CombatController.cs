using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    ThirdPersonController controller;
    public bool isInCombatMode = false;
    public Enemy currentEnemy;

    public List<Enemy> detectedEnemies = new List<Enemy>();
    bool areEnemiesDetected = false;
    
    bool leftHandIsOnCooldown = false;

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

        Debug.Log("ataco mano izquierda");
        leftHandIsOnCooldown = true;
        StartCoroutine(LeftHandCooldown());

    }

    private IEnumerator LeftHandCooldown()
    {
        yield return new WaitForSeconds(2);
        leftHandIsOnCooldown = false;
    }
    public void OnAttackAnimationEnded()
    {
        leftHandIsOnCooldown = false;
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
        Debug.Log("entro a combat mode");
        //reemplazar anim de idle por idle-fight
    }
    public void ExitCombatMode()
    {
        isInCombatMode = false;
        Debug.Log("salio de combat mode");
        //reemplazar anim de idle-fight por idle

    }
    public void UpdateDetectionStatus(Enemy lastDetectedEnemy)
    {
        if (detectedEnemies.Count > 0)
        {
            areEnemiesDetected = true;
            EnterCombatMode();
            Debug.Log("enemies in volume");

            if (currentEnemy == null)
            {
                SetCurrentEnemy(lastDetectedEnemy);
                Debug.Log("current enemy = " + currentEnemy.name);
            }
        }
        else
        {
            areEnemiesDetected = false;
            ExitCombatMode();
            SetCurrentEnemy(null);
            Debug.Log("no enemies in volume");
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
