using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendZone : MonoBehaviour
{
    Renderer myRenderer;
    Color originalColor;
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
            StartCoroutine(BlendColor(StealthManager.Instance.blendDelay, Color.green));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<StealthController>() != null)
        {
            StealthManager.Instance.ExitBlendZone();
            StopAllCoroutines();
            StartCoroutine(BlendColor(StealthManager.Instance.blendDelay / 4, originalColor));
        }
    }

    public IEnumerator BlendColor(float blendDelay, Color targetColor)
    {
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
