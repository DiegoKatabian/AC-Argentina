using UnityEngine;

public class LightTargetLerp : MonoBehaviour
{
    public Light targetLight;
    public float duration = 300f; //duracion en segundos

    private Light originalLight;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Color initialColor;
    private float initialIntensity;
    private float elapsedTime = 0f;

    void Start()
    {
        originalLight = GetComponent<Light>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialColor = originalLight.color;
        initialIntensity = originalLight.intensity;
    }

    void Update()
    {
        if (elapsedTime >= duration)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);
        transform.position = Vector3.Lerp(initialPosition, targetLight.transform.position, t);
        transform.rotation = Quaternion.Lerp(initialRotation, targetLight.transform.rotation, t);
        originalLight.color = Color.Lerp(initialColor, targetLight.color, t);
        originalLight.intensity = Mathf.Lerp(initialIntensity, targetLight.intensity, t);
    }
}
