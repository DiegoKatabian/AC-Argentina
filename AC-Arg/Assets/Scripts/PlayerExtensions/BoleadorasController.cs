using Climbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BoleadorasController : MonoBehaviour
{
    public CombatController combatController;
    public ThirdPersonController controller;
    bool isUsingBoleadoras = false;

    // Tiempo de espera para las boleadoras
    public float boleadorasDuration = 3f;
    private float boleadorasTimer = 0f;

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
        controller.RotatePlayer(enemy.transform.position);
        //controller.RotatePlayerIndependentOfCamera(enemy.gameObject.transform.position - transform.position);
        controller.DisableController();
        isUsingBoleadoras = true;
        controller.characterAnimation.animator.CrossFade("Boleadoras", 0.2f);
        boleadorasTimer = 0f; // Reiniciar el temporizador
        Invoke("EndBoleadoras", boleadorasDuration); // Invocar EndBoleadoras despu�s de boleadorasDuration segundos
        //enemy.GetBoleadoraed(this.gameObject);
    }

    // M�todo invocado por el temporizador
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