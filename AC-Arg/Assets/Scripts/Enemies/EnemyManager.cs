using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public ThirdPersonController player;

    PlayerHealthComponent playerHealth;

    Dictionary<Enemy, FiniteStateMachine> enemyFSMs = new Dictionary<Enemy, FiniteStateMachine>();
    Dictionary<Enemy, IState> enemyStates = new Dictionary<Enemy, IState>();
    Queue<Enemy> readyToAttackEnemiesQueue = new Queue<Enemy>();

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealthComponent>();
    }

    internal void DamagePlayer(float attackDamage)
    {
        playerHealth.TakeDamage(attackDamage);
        player.StartHurt();
    }
    public void DamageEnemy(Enemy enemy, float damage)
    {
        if (enemy == null)
        {
            Debug.Log("enemy is null");
            return;
        }
        HealthComponent enemyHealth = enemy.GetComponent<HealthComponent>();
        enemyHealth.TakeDamage(damage);
        enemy.StartHurt();
        Debug.Log("damage enemy: le hiciste " + damage + " al enemy " + enemy);
    }
    public void RegisterEnemy(Enemy enemy, FiniteStateMachine enemyFSM)
    {
        enemyFSMs.Add(enemy, enemyFSM);
        enemyStates.Add(enemy, enemyFSM._currentState);
    }
    public void KillEnemy(Enemy enemy)
    {
        //Debug.Log("killing " + enemy.gameObject.name);
        StopAllCoroutines();
        enemyFSMs.Remove(enemy);
        enemyStates.Remove(enemy);
        if (readyToAttackEnemiesQueue.Contains(enemy))
        {
            readyToAttackEnemiesQueue.Dequeue();
        }
        EventManager.Trigger(Evento.OnEnemyKilled, enemy);
        Destroy(enemy.gameObject);
    }
    public void UpdateEnemyState(FiniteStateMachine enemyFSM, IState currentState)
    {
        //Debug.Log("EnemyManager: an enemy has updated its state.");
        //Debug.Log("enemy: " + enemyFSM + " state: " + currentState);

        foreach (KeyValuePair<Enemy, FiniteStateMachine> enemyFSMEntry in enemyFSMs)
        {
            if (enemyFSMEntry.Value == enemyFSM)
            {
                enemyStates[enemyFSMEntry.Key] = currentState;
                UpdateReadyToAttackEnemiesQueue(enemyFSMEntry.Key, currentState);
                //UpdateAttackingEnemiesQueue(enemyFSMEntry.Key, currentState);
            }
        }
        EmitAlarm(currentState);
    }
    private void UpdateReadyToAttackEnemiesQueue(Enemy enemy, IState currentState)
    {
        if (currentState.GetType() == typeof(EnemyReadyToAttack))
        {
            if (!readyToAttackEnemiesQueue.Contains(enemy))
            {
                readyToAttackEnemiesQueue.Enqueue(enemy);
                //Debug.Log("agregué " + enemy + " a la cola");
            }
        }
        else
        {
            if (readyToAttackEnemiesQueue.Contains(enemy))
            {
                readyToAttackEnemiesQueue.Dequeue();
                //Debug.Log("saqué a " + enemy + " de la cola");
            }
        }
    }
    public bool IsAnyEnemyAttackingPlayer()
    {
        foreach (KeyValuePair<Enemy, IState> enemyState in enemyStates)
        {
            if (enemyState.Value.GetType() == typeof(EnemyAttack))
            {
                return true;
            }
        }
        return false;
    }
    public bool CanIAttackPlayerMisterEnemyManager(Enemy enemy)
    {
        //first, the enemy requesting must be in ReadyToAttack state
        //second, there has to be no other enemy already attacking the player
        //third, the enemy must be the first in the queue
        return enemyStates[enemy].GetType() == typeof(EnemyReadyToAttack) && 
            !IsAnyEnemyAttackingPlayer() && 
            IsEnemyNextInLine(enemy);
    }
    public bool IsEnemyNextInLine(Enemy enemy)
    {
        return readyToAttackEnemiesQueue.Count > 0 && 
            readyToAttackEnemiesQueue.Peek() == enemy;
    }
    public bool IsAnyEnemyChasingPlayer()
    {
        foreach (KeyValuePair<Enemy, IState> enemyState in enemyStates)
        {
            if (enemyState.Value.GetType() == typeof(EnemyChase))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsAnyEnemyHurting()
    {
        foreach (KeyValuePair<Enemy, IState> enemyState in enemyStates)
        {
            if (enemyState.Value.GetType() == typeof(EnemyHurt))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsAnyEnemyReadyToAttack()
    {
        foreach (KeyValuePair<Enemy, IState> enemyState in enemyStates)
        {
            if (enemyState.Value.GetType() == typeof(EnemyReadyToAttack))
            {
                return true;
            }
        }
        return false;
    }

    public bool AreAllEnemiesIdle()
    {
        foreach (KeyValuePair<Enemy, IState> enemyState in enemyStates)
        {
            if (enemyState.Value.GetType() != typeof(EnemyIdle))
            {
                return false;
            }
        }
        return true;
    }
    public void EmitAlarm(IState state)
    {
        if (IsAnyEnemyAttackingPlayer())
        {
            StealthManager.Instance.SetStealthStatus("Alert");
            return;
        }

        if (IsAnyEnemyChasingPlayer())
        {
            StealthManager.Instance.SetStealthStatus("Warning");
            return;
        }

        if (IsAnyEnemyHurting())
        {
            StealthManager.Instance.SetStealthStatus("Alert");
            return;
        }
        
        if (IsAnyEnemyReadyToAttack())
        {
            StealthManager.Instance.SetStealthStatus("Alert");
            return;
        }

        if (AreAllEnemiesIdle() && 
            StealthManager.Instance.currentStatus.status != StealthStatus.Hidden)
        {
            StealthManager.Instance.SetStealthStatus("Anonymous");
            return;
        }
    }

    public void RotateTowardsPlayer(Enemy enemy)
    {
        StartCoroutine(RotateCoroutine(enemy));
    }
    private IEnumerator RotateCoroutine(Enemy enemy)
    {
        enemy.isRotating = true;

        Vector3 playerPosition = player.transform.position;
        Vector3 enemyPosition = enemy.transform.position;

        Vector3 directionToPlayer = playerPosition - enemyPosition;
        directionToPlayer.y = 0f; // Establecer la componente Y a cero para ignorar la elevación

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        Quaternion initialRotation = enemy.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < enemy.rotationTime)
        {
            float t = elapsedTime / enemy.rotationTime;
            Quaternion lerpedRotation = Quaternion.Slerp(initialRotation, targetRotation, t);

            // Mantener los ejes X y Z sin cambios
            lerpedRotation.x = initialRotation.x;
            lerpedRotation.z = initialRotation.z;

            enemy.transform.rotation = lerpedRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.transform.rotation = targetRotation;
        enemy.isRotating = false;
    }

    public bool IsPlayerInLineOfSight(Vector3 position, float viewDistance, LayerMask layer)
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.transform.position - position).normalized;

        if (Physics.Raycast(position, directionToPlayer, out hit, viewDistance, layer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }


}
