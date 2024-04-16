using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class AlphaLerpUtility
{
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
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;
    }

    public static IEnumerator LerpColorCoroutine(Image image, Color startColor, Color targetColor, float time)
    {
        Debug.Log("init lerpcolor");
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            Debug.Log("lerping...");
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;
        Debug.Log("color = target color");
    }

    public static IEnumerator LerpColor(Image image, Color startColor, Color targetColor, float time, float alphaCap)
    {
        float elapsedTime = 0f;
        targetColor = new Color(targetColor.r, targetColor.g, targetColor.b, alphaCap);

        while (elapsedTime < time)
        {
            image.color = Color.Lerp(startColor, targetColor, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = targetColor;
    }
}
