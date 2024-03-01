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
        foreach (KeyValuePair<Enemy, FiniteStateMachine> enemyFSMEntry in enemyFSMs)
        {
            if (enemyFSMEntry.Value == enemyFSM)
            {
                enemyStates[enemyFSMEntry.Key] = currentState;
                UpdateAttackingEnemiesQueue(enemyFSMEntry.Key, currentState);
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

    public bool CanEnemyAttack(Enemy enemy)
    {
        //Debug.Log(attackingEnemiesQueue.Count);
        //Debug.Log(attackingEnemiesQueue.Peek().name);
        //Debug.Log(enemy.name);
        return attackingEnemiesQueue.Count > 0 && attackingEnemiesQueue.Peek() == enemy;
    }
}
