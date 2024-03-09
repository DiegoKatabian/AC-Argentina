using Climbing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    ThirdPersonController controller;
    public bool isInCombatMode = false;
    Enemy currentEnemy;

    public List<GameObject> detectedEnemies = new List<GameObject>();
    bool areEnemiesDetected = false;
    public bool AreEnemiesDetected
    {
        get { return areEnemiesDetected; }
    }
    private void Start()
    {
        controller = GetComponent<ThirdPersonController>();
    }

    public void EnterCombatMode()
    {
        isInCombatMode = true;
        Debug.Log("entro a combat mode");
    }

    public void ExitCombatMode()
    {
        isInCombatMode = false;
        Debug.Log("salio de combat mode");
    }

    public void UpdateDetectionStatus()
    {
        if (detectedEnemies.Count > 0)
        {
            areEnemiesDetected = true;
            EnterCombatMode();
            Debug.Log("enemies in volume");
        }
        else
        {
            areEnemiesDetected = false;
            ExitCombatMode();
            Debug.Log("no enemies in volume");
        }
    }
}
