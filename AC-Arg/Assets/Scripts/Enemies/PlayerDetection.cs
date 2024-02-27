using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float fieldOfViewAngle = 110f; // Ángulo de visión del enemigo
    public float viewDistance = 30f; // Distancia máxima de visión
    public float meleeDistance = 5f; // Distancia de ataque melee
    public LayerMask playerLayer; // Capa del jugador

    private bool isPlayerInFOV = false; // Variable para detectar si el jugador está en el campo de visión
    private bool isPlayerInMeleeDistance = false; // Variable para detectar si el jugador está en distancia melee

    private void Start()
    {
        // Llama a las funciones de detección cada segundo
        InvokeRepeating("CheckPlayerInFOV", 0f, 1f);
        InvokeRepeating("CheckPlayerInMeleeDistance", 0f, 1f);
    }

    private void CheckPlayerInFOV()
    {
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);

        foreach (Collider collider in playerColliders)
        {
            Vector3 directionToPlayer = (collider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, viewDistance))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        isPlayerInFOV = true;
                        Debug.Log("player in fov");
                        return;
                    }
                }
            }
        }

        isPlayerInFOV = false;
    }

    private void CheckPlayerInMeleeDistance()
    {
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, meleeDistance, playerLayer);

        if (playerColliders.Length > 0)
        {
            isPlayerInMeleeDistance = true;
            Debug.Log("player in melee distance");
        }
        else
        {
            isPlayerInMeleeDistance = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Gizmos.color = Color.red;
        float halfFOV = fieldOfViewAngle / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * viewDistance);
        Gizmos.DrawRay(transform.position, rightRayDirection * viewDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeDistance);
    }
}
