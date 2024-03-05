using Climbing;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public ThirdPersonController player;
    public bool isAnyEnemyAttackingPlayer = false;
    public bool isAnyEnemyChasingPlayer = false;

    PlayerHealthComponent playerHealth;

    Dictionary<Enemy, FiniteStateMachine> enemyFSMs = new Dictionary<Enemy, FiniteStateMachine>();
    Dictionary<Enemy, IState> enemyStates = new Dictionary<Enemy, IState>();
    Queue<Enemy> attackingEnemiesQueue = new Queue<Enemy>();
    Queue<Enemy> readyToAttackEnemiesQueue = new Queue<Enemy>();

    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealthComponent>();
    }

    internal void DamagePlayer(float attackDamage)
    {
        playerHealth.TakeDamage(attackDamage);
    }

    public void RegisterEnemy(Enemy enemy, FiniteStateMachine enemyFSM)
    {
        enemyFSMs.Add(enemy, enemyFSM);
        enemyStates.Add(enemy, enemyFSM._currentState);
    }

    public void UpdateEnemyState(FiniteStateMachine enemyFSM, IState currentState)
    {
        Debug.Log("EnemyManager: an enemy has updated its state.");
        Debug.Log("enemy: " + enemyFSM + " state: " + currentState);

        foreach (KeyValuePair<Enemy, FiniteStateMachine> enemyFSMEntry in enemyFSMs)
        {
            if (enemyFSMEntry.Value == enemyFSM)
            {
                enemyStates[enemyFSMEntry.Key] = currentState;
                UpdateAttackingEnemiesQueue(enemyFSMEntry.Key, currentState);
                UpdateReadyToAttackEnemiesQueue(enemyFSMEntry.Key, currentState);
            }
        }


        isAnyEnemyChasingPlayer = IsAnyEnemyChasingPlayer();
        isAnyEnemyAttackingPlayer = IsAnyEnemyAttackingPlayer();
    }

    private void UpdateAttackingEnemiesQueue(Enemy enemy, IState currentState)
    {
        if (currentState.GetType() == typeof(EnemyAttack))
        {
            if (!attackingEnemiesQueue.Contains(enemy))
            {
                attackingEnemiesQueue.Enqueue(enemy);
                //Debug.Log("agregué " + enemy + " a la cola");
            }
        }
        else
        {
            if (attackingEnemiesQueue.Contains(enemy))
            {
                attackingEnemiesQueue.Dequeue();
                //Debug.Log("saqué a " + enemy + " de la cola");
            }
        }
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

    public bool IsAnyEnemyReadyToAttackPlayer()
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

    public int GetNumberOfEnemiesReadyToAttackPlayer()
    {
        int count = 0;
        foreach (KeyValuePair<Enemy, IState> enemyState in enemyStates)
        {
            if (enemyState.Value.GetType() == typeof(EnemyReadyToAttack))
            {
                count++;
            }
        }
        return count;
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
}
