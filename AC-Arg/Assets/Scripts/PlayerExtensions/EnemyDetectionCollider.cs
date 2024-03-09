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
            combatController.detectedEnemies.Add(other.gameObject);
            combatController.UpdateDetectionStatus();
            Debug.Log("Enemy Detected");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            combatController.detectedEnemies.Remove(other.gameObject);
            combatController.UpdateDetectionStatus();
            Debug.Log("Enemy Lost");
        }
    }

    

    
}
