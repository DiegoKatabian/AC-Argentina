using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionCollider : DetectionCollider
{
    public CombatController combatController;

    private void Start()
    {
        EventManager.Subscribe(Evento.OnEnemyKilled, RemoveEnemy);
    }

    public override void OnTagDetectedEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            Enemy detectedEnemy = other.GetComponent<Enemy>();
            combatController.AddEnemyToDetectedList(detectedEnemy);
        }
    }

    public override void OnTagDetectedExit(Collider other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            Enemy detectedEnemy = other.GetComponent<Enemy>();
            combatController.RemoveEnemyFromDetectedList(detectedEnemy);
        }
    }

    private void RemoveEnemy(object[] parameters)
    {
        combatController.RemoveEnemyFromDetectedList((Enemy)parameters[0]);
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
            EventManager.Unsubscribe(Evento.OnEnemyKilled, RemoveEnemy);
    }

   
}
