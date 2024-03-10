using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionCollider : MonoBehaviour
{
    public CombatController combatController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.GetComponent<Enemy>() != null)
            {
                Enemy detectedEnemy = other.GetComponent<Enemy>();
                combatController.detectedEnemies.Add(detectedEnemy);
                combatController.UpdateDetectionStatus(detectedEnemy);
                Debug.Log("Enemy Detected");
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
                combatController.detectedEnemies.Remove(detectedEnemy);
                combatController.UpdateDetectionStatus(detectedEnemy);
                Debug.Log("Enemy Lost");
            }
        }

    }

    

    
}
