using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendZone : MonoBehaviour
{
    Renderer myRenderer;
    Color originalColor;
    public Color activatedColor;
    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        originalColor = myRenderer.material.color;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<StealthController>() != null)
        {
            StealthManager.Instance.EnterBlendZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<StealthController>() != null)
        {
            StealthManager.Instance.ExitBlendZone();
            StopAllCoroutines();
            StartCoroutine(LerpColorToDeactivate(StealthManager.Instance.blendDelay / 4, originalColor));
        }
    }

    public void EnterZoneConfirmed()
    {
        StartCoroutine(LerpColorToActivate(StealthManager.Instance.blendDelay, Color.green));
    }

    public void BlendCanceled()
    {
        StopAllCoroutines();
        StartCoroutine(LerpColorToDeactivate(StealthManager.Instance.blendDelay / 4, originalColor));
    }

    public IEnumerator LerpColorToActivate(float blendDelay, Color targetColor)
    {
        targetColor = new Color(targetColor.r, targetColor.g, targetColor.b, 0.5f);
        float elapsedTime = 0;
        while (elapsedTime < blendDelay)
        {
            myRenderer.material.color = Color.Lerp(originalColor, targetColor, elapsedTime / blendDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        myRenderer.material.color = activatedColor;
    }

    public IEnumerator LerpColorToDeactivate(float blendDelay, Color targetColor)
    {
        targetColor = new Color(targetColor.r, targetColor.g, targetColor.b, 0.5f);
        float elapsedTime = 0;
        while (elapsedTime < blendDelay)
        {
            myRenderer.material.color = Color.Lerp(originalColor, targetColor, elapsedTime / blendDelay);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        myRenderer.material.color = targetColor;
    }
}
