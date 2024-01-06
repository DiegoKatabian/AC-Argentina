using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public float speed = 5f; // Ajusta este valor para controlar la velocidad del movimiento
    public float distance = 5f; // Ajusta este valor para controlar la distancia del movimiento
    private float initialPositionZ;

    void Start()
    {
        // Guarda la posición inicial en el eje Z del objeto de juego
        initialPositionZ = transform.position.z;
    }

    void Update()
    {
        // Calcula la nueva posición en el eje Z basándose en la función seno para crear un movimiento de ida y vuelta
        float newPositionZ = initialPositionZ + Mathf.Sin(Time.time * speed) * distance;

        // Actualiza la posición del objeto de juego
        transform.position = new Vector3(transform.position.x, transform.position.y, newPositionZ);
    }
}
