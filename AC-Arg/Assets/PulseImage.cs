using UnityEngine;
using UnityEngine.UI;

public class ImagePulse : MonoBehaviour
{
    public Image image; // La imagen que quieres animar.
    public float speed = 1f; // Velocidad de la animación.
    public float minScale = 0.8f; // Escala mínima.
    public float maxScale = 1.2f; // Escala máxima.
    public float minAlpha = 0.5f; // Opacidad mínima.
    public float maxAlpha = 1f; // Opacidad máxima.

    private float scaleLerpTime = 0f;
    private float alphaLerpTime = 0f;
    private bool increasing = true;

    void Update()
    {
        // Animación de la escala.
        scaleLerpTime += Time.deltaTime * speed;
        if (scaleLerpTime > 1f)
        {
            scaleLerpTime = 0f;
            increasing = !increasing;
        }

        float scale = Mathf.Lerp(minScale, maxScale, increasing ? scaleLerpTime : 1f - scaleLerpTime);
        transform.localScale = new Vector3(scale, scale, 1f);

        // Animación de la opacidad.
        alphaLerpTime += Time.deltaTime * speed;
        if (alphaLerpTime > 1f)
        {
            alphaLerpTime = 0f;
        }

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(alphaLerpTime, 1f));
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}