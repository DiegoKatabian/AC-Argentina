using System.Collections;
using UnityEngine;

public class UnarmedEnemy : Enemy
{
    public Hitbox punchHitBox;

    public override void Start()
    {
        base.Start();
        _fsm = new EnemyFSM();
        _fsm.AddState(State.EnemyIdle, new EnemyIdle(_fsm, this));
        _fsm.AddState(State.EnemyChase, new EnemyChase(_fsm, this));
        _fsm.AddState(State.EnemyAttack, new EnemyAttack(_fsm, this));
        _fsm.AddState(State.EnemyReadyToAttack, new EnemyReadyToAttack(_fsm, this));
        _fsm.ChangeState(State.EnemyIdle);
        EnemyManager.Instance.RegisterEnemy(this, _fsm);
    }

    private void Update()
    {
        _fsm.Update();
    }

    public override void TryAttack()
    {
        StartAttack();
    }

    public override void StartAttack()
    {
        //Debug.Log("empieza el ataque");
        isAttacking = true;
    }

    public void ANIMATION_OnAttackHit()
    {
        StartCoroutine(HitboxCouroutine()); 
    }

    public void ANIMATION_OnAttackEnd()
    {
        FinishAttack();
    }

    public void FinishAttack()
    {
        //Debug.Log("termina el ataque");
        finishedAttacking = true;
        isAttacking = false;
    }

    IEnumerator HitboxCouroutine()
    {
        ObjectEnabler.EnableObject(punchHitBox.gameObject, true);
        yield return new WaitForSeconds(0.2f);
        if (punchHitBox.isTaggedInside)
        {
            EnemyManager.Instance.DamagePlayer(attackDamage);
        }

        ObjectEnabler.EnableObject(punchHitBox.gameObject, false);

        yield return new WaitForSeconds(attackRecoveryTime);
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
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(EnemyManager.Instance.player.transform.position);
    }

    public override void OnDeath()
    {
        base.OnDeath();
        _fsm.ChangeState(State.EnemyIdle);
        EnemyManager.Instance.KillEnemy(this);
    }


}
