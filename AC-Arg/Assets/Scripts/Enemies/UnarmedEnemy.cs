using System.Collections;
using UnityEngine;

public class UnarmedEnemy : Enemy
{
    public PunchHitbox punchHitBox;

    public override void Start()
    {
        base.Start();
        _fsm = new EnemyFSM();
        _fsm.AddState(State.EnemyIdle, new EnemyIdle(_fsm, this));
        _fsm.AddState(State.EnemyChase, new EnemyChase(_fsm, this));
        _fsm.AddState(State.EnemyAttack, new EnemyAttack(_fsm, this));
        _fsm.ChangeState(State.EnemyIdle);
        EnemyManager.Instance.RegisterEnemy(this, _fsm);
    }

    private void Update()
    {
        _fsm.Update();
    }

    public override void TryAttack()
    {
        if (!EnemyManager.Instance.CanEnemyAttack(this))
        {
            return;
        }
        StartAttack();
    }

    public void StartAttack()
    {
        Debug.Log("empieza el ataque");
        isAttacking = true;
        StartCoroutine(HitboxCouroutine()); //en vez de aca, esto deberia dispararse en el momento correcto de la animacion
    }

    public void FinishAttack()
    {
        Debug.Log("termina el ataque");
        finishedAttacking = true;
        isAttacking = false;
    }

    IEnumerator HitboxCouroutine()
    {
        EnableObject(punchHitBox.gameObject, true);
        yield return new WaitForSeconds(0.2f);
        if (punchHitBox.isPlayerInside)
        {
            EnemyManager.Instance.DamagePlayer(attackDamage);
        }

        EnableObject(punchHitBox.gameObject, false);

        yield return new WaitForSeconds(attackRecoveryTime);
        FinishAttack();
    }

    public override void StartChasingPlayer()
    {
        InvokeRepeating("ChasePlayer", 0, playerDetection.checkDelay);
    }

    public override void CancelChasePlayer()
    {
        CancelInvoke("ChasePlayer");
    }

    public void ChasePlayer()
    {
        //Debug.Log("movinggg");
        navMeshAgent.SetDestination(EnemyManager.Instance.player.transform.position);
    }


}
