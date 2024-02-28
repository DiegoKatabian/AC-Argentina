using Climbing;

public class EnemyManager : Singleton<EnemyManager>
{
    public ThirdPersonController player;
    public bool anyEnemyIsAttackingPlayer = false;

}
