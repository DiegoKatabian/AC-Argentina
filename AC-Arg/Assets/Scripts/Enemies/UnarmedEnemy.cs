using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class UnarmedEnemy : Enemy, ICrashable, IPedestrian
{
    public Hitbox punchHitBox;
    public Transform[] waypoints;
    public bool canInteract = true;

    public GameObject bloodParticlesPrefab;


    public override void Start()
    {
        _fsm = new EnemyFSM();
        _fsm.AddState(State.EnemyIdle, new EnemyIdle(_fsm, this));
        _fsm.AddState(State.EnemyPatrol, new EnemyPatrol(_fsm, this, waypoints));
        _fsm.AddState(State.EnemyChase, new EnemyChase(_fsm, this));
        _fsm.AddState(State.EnemyAttack, new EnemyAttack(_fsm, this));
        _fsm.AddState(State.EnemyReadyToAttack, new EnemyReadyToAttack(_fsm, this));
        _fsm.AddState(State.EnemyHurt, new EnemyHurt(_fsm, this));
        _fsm.AddState(State.EnemyKnockedOut, new EnemyKnockedOut(_fsm, this));
        _fsm.AddState(State.EnemyDead, new EnemyDead(_fsm, this));
        _fsm.ChangeState(State.EnemyIdle);
        EnemyManager.Instance.RegisterEnemy(this, _fsm);
        isDead = false;
    }

    public void Update()
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
    public IEnumerator HitboxCouroutine()
    {
        ObjectEnabler.EnableObject(punchHitBox.gameObject, true);
        punchHitBox.isTaggedInside = false;
        yield return new WaitForSeconds(0.1f);
        if (punchHitBox.isTaggedInside)
        {
            AudioManager.Instance.PlayPunchHitSFX();
            EnemyManager.Instance.DamagePlayer(attackDamage);
        }

        ObjectEnabler.EnableObject(punchHitBox.gameObject, false);
    }
   
    //HURT
    public override void StartHurt()
    {
        base.StartHurt();
        AudioManager.Instance.PlayHurtSFX();
        finishedAttacking = true;
        isAttacking = false;
        isHurting = true;
    }
    public void ANIMATION_OnHurtEnd() //llamado por la animacion de daño
    {
        finishedHurting = true;
    }

    //CHASE
    public override void StartChasingPlayer()
    {
        //Debug.Log("enemy: start chasing player");
        InvokeRepeating("ChasePlayer", 0, playerDetection.checkDelay);
    }
    public override void CancelChasePlayer()
    {
        CancelInvoke("ChasePlayer");
        navMeshAgent.SetDestination(transform.position);
    }
    public void ChasePlayer()
    {
        //Debug.Log("enemy: chase player");
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(EnemyManager.Instance.player.transform.position);
    }

    //DEATH
    public override void OnDeath()
    {
        base.OnDeath();
        //isntantiate the blood particles

        if (bloodParticlesPrefab != null)
        {
            GameObject bloodParticles = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
            bloodParticles.GetComponent<ParticleSystem>().Play();
        }
        
        AudioManager.Instance.PlayDeathSFX();
        EnemyManager.Instance.KillEnemy(this);
        isDead = true;
        _fsm.ChangeState(State.EnemyDead);
    }

    public void OnCrash(GameObject vehicle, float crashForce)
    {
        StartHurt();
    }

    public void SetInteractionMarkerActive(bool active)
    {
        currentEnemyMarker.SetActive(active);
    }

    public void GetAssassinated(GameObject assassin)
    {
        Debug.Log("enemy: me asesinaron");
        animator.CrossFade("GetAssasinated", 0.2f);
        Invoke("OnDeath", 1f);
    }



    public void GetStolen()
    {
        Debug.Log("enemy: me robaron!");
    }

    public bool CanInteract()
    {
        return canInteract;
    }

    public override void OnPedestrianAlarmEmit()
    {
        if (isKnockedOut)
        {
            //Debug.Log("enemy: onpedestrianalarmemit! pero no hago nada xq esoy noqueado");
            return;
        }

        //Debug.Log("enemy: onpedestrianalarmemit!");
        StartChasingPlayer();
        animator.CrossFade("Chase", 0.2f);
        chasesPlayerOnlyWhileWarning = false;
    }


}
