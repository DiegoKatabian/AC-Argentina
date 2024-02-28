using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnarmedEnemy : Enemy
{
    public PunchHitbox punchHitBox;
    protected FiniteStateMachine _fsm;

    private void Start()
    {
        _fsm = new FiniteStateMachine();
        _fsm.AddState(State.EnemyIdle, new EnemyIdle(_fsm, this));
        _fsm.AddState(State.EnemyChase, new EnemyChase(_fsm, this));
        _fsm.AddState(State.EnemyAttack, new EnemyAttack(_fsm, this));
        _fsm.ChangeState(State.EnemyIdle);
    }

    private void Update()
    {
        _fsm.Update();
    }

    public override void TryAttack()
    {
        if (EnemyManager.Instance.isAnyoneAttackingPlayer)
        {
            //Debug.Log("no puedo atacar todavia");
            return;
        }

        Debug.Log("puedo atacar");
        StartAttack();
    }

    public void StartAttack()
    {
        Debug.Log("empieza el ataque");
        EnemyManager.Instance.isAnyoneAttackingPlayer = true;
        StartCoroutine(HitboxCouroutine()); //en vez de aca, esto deberia dispararse en el momento correcto de la animacion
    }

    public void FinishAttack()
    {
        Debug.Log("termina el ataque");
        EnemyManager.Instance.isAnyoneAttackingPlayer = false;
    }

    IEnumerator HitboxCouroutine()
    {
        EnableHitbox(punchHitBox.gameObject, true);
        yield return new WaitForSeconds(0.2f);
        if (punchHitBox.isPlayerInside)
        {
            EnemyManager.Instance.DamagePlayer(attackDamage);
        }

        EnableHitbox(punchHitBox.gameObject, false);

        yield return new WaitForSeconds(attackRecoveryTime);
        FinishAttack();
    }

    public void EnableHitbox(GameObject hitbox, bool state)
    {
        Debug.Log("hitbox = " + state);
        hitbox.SetActive(state);
        //isHitting = false;
    }

   

}
