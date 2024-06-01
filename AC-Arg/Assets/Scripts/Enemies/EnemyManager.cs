using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : Singleton<EnemyManager>
{
    public ThirdPersonController player;
    public float pedestrianAlarmRadius = 20f;

    Dictionary<Enemy, FiniteStateMachine> enemyFSMs = new Dictionary<Enemy, FiniteStateMachine>();
    Dictionary<Enemy, IState> enemyStates = new Dictionary<Enemy, IState>();
    Queue<Enemy> readyToAttackEnemiesQueue = new Queue<Enemy>();


    internal void DamagePlayer(float attackDamage)
    {
        player.ReceiveEnemyDamage(attackDamage);
    }
    public void DamageEnemy(Enemy enemy, float damage)
    {
        if (enemy == null)
        {
            Debug.Log("enemy is null");
            return;
        }
        //si el enemy esta blockeando, no recibe da�o
        if (enemy.isBlocking)
        {   
            Debug.Log("enemy is blocking");
            return;
        }

        HealthComponent enemyHealth = enemy.GetComponent<HealthComponent>();
        enemyHealth.TakeDamage(damage);
        enemy.StartHurt();
        //Debug.Log("damage enemy: le hiciste " + damage + " al enemy " + enemy);
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
        UnregisterEnemy(enemy);
        EventManager.Instance.Trigger(Evento.OnEnemyKilled, enemy);
        EmitAlarm();
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
        EmitAlarm();
    }
    private void UpdateReadyToAttackEnemiesQueue(Enemy enemy, IState currentState)
    {
        //sie sta en ready o blocking ahora
        if (currentState.GetType() == typeof(EnemyReadyToAttack)
            || currentState.GetType() == typeof(EnemyBlock))
        {
            if (!readyToAttackEnemiesQueue.Contains(enemy))
            {
                readyToAttackEnemiesQueue.Enqueue(enemy);
                //Debug.Log("agregu� " + enemy + " a la cola");
            }
        }
        else
        {
            if (readyToAttackEnemiesQueue.Contains(enemy))
            {
                readyToAttackEnemiesQueue.Dequeue();
                //Debug.Log("saqu� a " + enemy + " de la cola");
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
        bool isInPermittedState = enemyStates[enemy].GetType() == typeof(EnemyReadyToAttack) ||
                                    enemyStates[enemy].GetType() == typeof(EnemyBlock);


        return isInPermittedState && 
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
            if (enemyState.Value.GetType() == typeof(EnemyReadyToAttack) ||
                enemyState.Value.GetType() == typeof(EnemyBlock))
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

    public bool IsAnyEnemyInState<T>() where T : IState
    {
        return enemyStates.Any(kv => kv.Value.GetType() == typeof(T));
    }

    public bool AreAllEnemiesInState(params Type[] stateTypes)
    {
        return enemyStates.All(kv => stateTypes.Contains(kv.Value.GetType()));
    }

    public bool AreAllEnemiesIdleOrPatrolling()
    {
        return AreAllEnemiesInState(typeof(EnemyIdle), typeof(EnemyPatrol), typeof(EnemyKnockedOut));
    }

    public void EmitAlarm()
    {
        if (StealthManager.Instance == null)
        {
            return;
        }

        if (IsAnyEnemyInState<EnemyAttack>())
        {
            Debug.Log("habia enemies en attack, alert");
            StealthManager.Instance.SetStealthStatus(StealthStatus.Alert);
            return;
        }

        if (IsAnyEnemyInState<EnemyChase>())
        {
            StealthManager.Instance.SetStealthStatus(StealthStatus.Warning);
            return;
        }

        if (IsAnyEnemyInState<EnemyHurt>())
        {
            Debug.Log("habia enemies en hurt, alert");
            StealthManager.Instance.SetStealthStatus(StealthStatus.Alert);
            return;
        }
        
        if (IsAnyEnemyInState<EnemyReadyToAttack>())
        {
            Debug.Log("habia enemies en ready to attack, alert");
            StealthManager.Instance.SetStealthStatus(StealthStatus.Alert);
            return;
        }

        if (AreAllEnemiesIdleOrPatrolling() &&
            StealthManager.Instance.currentStatus.status != StealthStatus.Hidden)
        {
            StealthManager.Instance.SetStealthStatus(StealthStatus.Visible);
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
        directionToPlayer.y = 0f; // Establecer la componente Y a cero para ignorar la elevaci�n

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
    public float GetDistanceToPlayer(Vector3 enemyPosition)
    {
        return Vector3.Distance(player.transform.position, enemyPosition);
    }
    public Vector3 GetDirectionToPlayer(Vector3 enemyPosition)
    {
        return player.transform.position - enemyPosition;
    }

    internal void TriggerPedestrianAlarm(Vector3 position)
    {
        IEnumerable<KeyValuePair<Enemy, IState>> nearbyEnemies = enemyStates.Where(kv => kv.Value.GetType() == typeof(EnemyIdle) || kv.Value.GetType() == typeof(EnemyPatrol))
                                .Where(kv => Vector3.Distance(kv.Key.transform.position, position) < pedestrianAlarmRadius);

        if (nearbyEnemies.Count() > 0)
        {
            StealthManager.Instance.SetStealthStatus(StealthStatus.Warning);
        }

        foreach (KeyValuePair<Enemy, IState> enemyState in nearbyEnemies)
        {
            //Debug.Log("enemy manager: mando a 1 enemigo cercano a investigar");
            enemyState.Key.navMeshAgent.isStopped = false;
            enemyState.Key.navMeshAgent.SetDestination(position);
            enemyState.Key.OnPedestrianAlarmEmit();
        }
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        enemyFSMs.Remove(enemy);
        enemyStates.Remove(enemy);
        if (readyToAttackEnemiesQueue.Contains(enemy))
        {
            readyToAttackEnemiesQueue.Dequeue();
        }
    }

    public void TriggerAlarm(Enemy triggeringEnemy, Vector3 position)
    {
        var nearbyEnemies = enemyStates.Where(kv => kv.Value.GetType() == typeof(EnemyIdle) || kv.Value.GetType() == typeof(EnemyPatrol))
                                .Where(kv => Vector3.Distance(kv.Key.transform.position, position) < pedestrianAlarmRadius);

        if (nearbyEnemies.Count() > 0)
        {
            StealthManager.Instance.SetStealthStatus(StealthStatus.Warning);
        }

        foreach (KeyValuePair<Enemy, IState> enemyState in nearbyEnemies)
        {
            enemyState.Key.navMeshAgent.SetDestination(position);
        }
    }

    internal bool AreEnemiesInCombat()
    {
        if (IsAnyEnemyAttackingPlayer() || IsAnyEnemyChasingPlayer() || IsAnyEnemyHurting())
        {
            return true;
        }
        return false;
    }
}
