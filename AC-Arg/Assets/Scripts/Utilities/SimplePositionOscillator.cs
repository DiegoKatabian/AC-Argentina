using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePositionOscillator : MonoBehaviour
{
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float offset = 0f;
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.time * frequency) * amplitude) + offset, transform.position.z);

    }
}
