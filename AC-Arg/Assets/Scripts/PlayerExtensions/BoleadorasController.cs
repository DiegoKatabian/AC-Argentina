using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEngine.EventSystems.EventTrigger;

public class BoleadorasController : MonoBehaviour
{
    public CombatController combatController;
    public ThirdPersonController controller;
    bool isUsingBoleadoras = false;

    // Tiempo de espera para las boleadoras
    public float boleadorasDuration = 3f;

    public AudioClip boleadorasStartSound, boleadorasHitSound;

    public void Start()
    {
        combatController = controller.combatController;
        EventManager.Instance.Subscribe(Evento.OnInputRequestBoleadoras, TryBoleadoras);
    }

    private void TryBoleadoras(object[] parameters)
    {
        Debug.Log("try boleadoras...");

        if (combatController.isInCombatMode)
        {
            Debug.Log("no puedo usar boleadoras en combate");
            return;
        }

        if (combatController.detectedEnemies.Count == 0)
        {
            Debug.Log("no hay enemigos detectados");
            return;
        }

        if (isUsingBoleadoras)
        {
            Debug.Log("ya estoy usando las boleadoras");
            return;
        }

        StartBoleadoras(combatController.currentEnemy);
    }


    public void StartBoleadoras(Enemy enemy)
    {
        Debug.Log("start boleadoras");
        controller.RotatePlayerIndependentOfCamera(enemy.gameObject.transform.position - transform.position);
        controller.DisableController();
        isUsingBoleadoras = true;
        controller.characterAnimation.animator.CrossFade("Boleadoras", 0.2f);
        EventManager.Instance.Trigger(Evento.OnBoleadorasStart);
        StartCoroutine(HitBoleadoras(enemy));
    }

    public void ANIMATION_PlayBoleadorasStartSound()
    {
        AudioManager.Instance.PlaySound(boleadorasStartSound);
        AudioManager.Instance.PlayHurtSFX();

    }

    public void ANIMATION_PlayBoleadorasHitSound()
    {
        AudioManager.Instance.PlaySound(boleadorasHitSound);
        AudioManager.Instance.PlayDeathSFX();
    }


    public IEnumerator HitBoleadoras(Enemy enemy)
    {
        Debug.Log("hit boleadoras");

        yield return new WaitForSeconds(0.5f);
        enemy.GetBoleadoraed();

        yield return new WaitForSeconds(boleadorasDuration);
        Debug.Log("end boleadoras");
        controller.EnableController();
        isUsingBoleadoras = false;
    }

    public void EndBoleadoras()
    {
        Debug.Log("end boleadoras");
        controller.EnableController();
        isUsingBoleadoras = false;
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnInputRequestBoleadoras, TryBoleadoras);
        }
    }
}
