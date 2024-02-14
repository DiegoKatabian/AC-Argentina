using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class AlphaLerpUtility
{
    // Método estático para cambiar gradualmente la transparencia de una imagen
    public static IEnumerator LerpAlpha(Image image, float startAlpha, float targetAlpha, float time)
    {
        float elapsedTime = 0f;
        Color startColor = image.color;
        Color targetColor = image.color;
        startColor.a = startAlpha;
        targetColor.a = targetAlpha;

        while (elapsedTime < time)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / time);
            Debug.Log("image color = " + image.color);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la transparencia llegue exactamente al valor objetivo
        image.color = targetColor;
    }
}
