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
        combatController.RemoveEnemyFromDetectedList((Enemy)parameters[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log("detecte un tag enemy - " + other.gameObject.name);
            if (other.GetComponent<Enemy>() != null)
            {
                //Debug.Log("...y tenia enemy component");
                Enemy detectedEnemy = other.GetComponent<Enemy>();
                combatController.AddEnemyToDetectedList(detectedEnemy);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>() != null)
            {
                Enemy detectedEnemy = other.GetComponent<Enemy>();
                combatController.RemoveEnemyFromDetectedList(detectedEnemy);
            }
        }
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
            EventManager.Unsubscribe(Evento.OnEnemyKilled, RemoveEnemy);
    }
}
