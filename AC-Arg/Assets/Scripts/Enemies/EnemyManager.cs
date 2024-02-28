using Climbing;
using System;

public class EnemyManager : Singleton<EnemyManager>
{
    public ThirdPersonController player;
    public bool isAnyoneAttackingPlayer = false;

    PlayerHealthComponent playerHealth;



    private void Start()
    {
        playerHealth = player.GetComponent<PlayerHealthComponent>();
    }
    internal void DamagePlayer(float attackDamage)
    {
        playerHealth.TakeDamage(attackDamage);
    }
}
