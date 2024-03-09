using Climbing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    ThirdPersonController controller;
    public bool isInCombatMode = false;
    public Enemy currentEnemy;

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

    public void UpdateDetectionStatus(GameObject lastDetectedEnemy)
    {
        if (detectedEnemies.Count > 0)
        {
            areEnemiesDetected = true;
            EnterCombatMode();
            Debug.Log("enemies in volume");

            if (currentEnemy == null)
            {
                SetCurrentEnemy(lastDetectedEnemy.GetComponent<Enemy>());
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
        if (currentEnemy != null)
        {
            ObjectEnabler.EnableObject(currentEnemy.isCurrentEnemyMarker, false);
        }

        currentEnemy = enemy;
        ObjectEnabler.EnableObject(currentEnemy.isCurrentEnemyMarker, true);
    }
}
