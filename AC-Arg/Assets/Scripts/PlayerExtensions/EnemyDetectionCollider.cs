using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionCollider : MonoBehaviour
{
    public CombatController combatController;

    private void Start()
    {
        EventManager.Subscribe(Evento.OnEnemyKilled, RemoveEnemy);
    }

    private void RemoveEnemy(object[] parameters)
    {
        RemoveEnemyFromDetectedList((Enemy)parameters[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>() != null)
            {
                Enemy detectedEnemy = other.GetComponent<Enemy>();
                AddEnemyToDetectedList(detectedEnemy);
            }
        }
    }

    private void AddEnemyToDetectedList(Enemy detectedEnemy)
    {
        combatController.detectedEnemies.Add(detectedEnemy);
        combatController.UpdateDetectionStatus(detectedEnemy);
        Debug.Log("Enemy Detected");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>() != null)
            {
                Enemy detectedEnemy = other.GetComponent<Enemy>();
                RemoveEnemyFromDetectedList(detectedEnemy);
            }
        }
    }

    private void RemoveEnemyFromDetectedList(Enemy detectedEnemy)
    {
        combatController.detectedEnemies.Remove(detectedEnemy);
        combatController.UpdateDetectionStatus(detectedEnemy);
        Debug.Log("Enemy Lost");
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
            EventManager.Unsubscribe(Evento.OnEnemyKilled, RemoveEnemy);
    }




}
