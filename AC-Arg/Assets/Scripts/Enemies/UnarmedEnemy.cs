using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnarmedEnemy : Enemy
{
    public GameObject hitBox;
    protected FiniteStateMachine _fsm;

    private void Start()
    {
        _fsm = new FiniteStateMachine();
        _fsm.AddState(State.EnemyIdle, new EnemyIdle(_fsm, this));
        _fsm.ChangeState(State.EnemyIdle);
    }

    private void Update()
    {
        _fsm.Update();

        //if (_player != null)
        //{
        //    target = _player.transform.position;
        //}
    }


    IEnumerator HitboxCouroutine() //esto se dispara en el momento correcto de la animacion de cabezazo
    {
        EnableHitbox(hitBox, true);
        yield return new WaitForSeconds(0.1f);
        EnableHitbox(hitBox, false);
    }

    public void EnableHitbox(GameObject hitbox, bool state)
    {
        //Debug.Log("hitbox = " + state);
        hitbox.SetActive(state);
        //isHitting = false;
    }

}
