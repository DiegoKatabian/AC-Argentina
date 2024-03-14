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
        _fsm.AddState(State.EnemyHurt, new EnemyHurt(_fsm, this));
        _fsm.ChangeState(State.EnemyIdle);
        EnemyManager.Instance.RegisterEnemy(this, _fsm);
    }

    private void Update()
    {
        _fsm.Update();
    }

    //ATTACK
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
    public void ANIMATION_OnAttackEnd() //llamado por la animacion de ataque
    {
        //Debug.Log("termina el ataque");
        finishedAttacking = true;
        isAttacking = false;
    }
    IEnumerator HitboxCouroutine()
    {
        ObjectEnabler.EnableObject(punchHitBox.gameObject, true);
        punchHitBox.isTaggedInside = false;
        yield return new WaitForSeconds(0.1f);
        if (punchHitBox.isTaggedInside)
        {
            EnemyManager.Instance.DamagePlayer(attackDamage);
        }

        ObjectEnabler.EnableObject(punchHitBox.gameObject, false);
    }

   
    //HURT
    public override void StartHurt()
    {
        base.StartHurt();
        finishedAttacking = true;
        isAttacking = false;
        isHurting = true;
        //StartCoroutine(HurtRecoveryCouroutine());
    }

    IEnumerator HurtRecoveryCouroutine()
    {
        Debug.Log("espero 2 segunditos...");
        yield return new WaitForSeconds(1);
        Debug.Log("...terminado");
        finishedHurting = true;
    }


    public void ANIMATION_OnHurtEnd() //llamado por la animacion de da�o
    {
        finishedHurting = true;
    }

    //CHASE
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


    //DEATH
    public override void OnDeath()
    {
        base.OnDeath();
        _fsm.ChangeState(State.EnemyIdle);
        EnemyManager.Instance.KillEnemy(this);
    }

}
