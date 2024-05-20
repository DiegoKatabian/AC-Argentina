using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;

    //im going to make a method that will be called when the player health is updated
    //it function will be to paint the background of the slider Red, so that each time you lose health
    //the slider background will flash red for 3 seconcds

    public Image background;
    Color originalBackgroundColor;

    public float flashFrequency = 8f;
    public float flashTime = 3f;


    void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnPlayerHealthUpdate, UpdateHealthBar);
        originalBackgroundColor = background.color;
    }

    private void UpdateHealthBar(object[] parameters)
    {
        slider.value = (float)parameters[0] / (float)parameters[1];
        StartCoroutine(MakeBackgroundImageFlashRed(flashTime, flashFrequency));
    }


    public IEnumerator MakeBackgroundImageFlashRed(float duration, float frequency)
    {
        //for the duration of the flashTime, the background will sine wave lerping between red and the original color
        float elapsedTime = 0f;

        Color targetColor = Color.red;

        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            background.color = Color.Lerp(originalBackgroundColor, targetColor, Mathf.Sin(elapsedTime * frequency));
            yield return null;
        }

        background.color = originalBackgroundColor;

    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            EventManager.Instance.Unsubscribe(Evento.OnPlayerHealthUpdate, UpdateHealthBar);
        }
    }


}
