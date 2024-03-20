using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDetection : MonoBehaviour
{
    public float fieldOfViewAngle = 110f; // �ngulo de visi�n del enemigo
    public float viewDistance = 30f; // Distancia m�xima de visi�n
    public float meleeDistance = 5f; // Distancia de ataque melee
    public LayerMask playerLayer; // Capa del jugador
    public LayerMask detectableLayer; // Capa de objetos detectables

    public bool isPlayerInFOV = false; // Variable para detectar si el jugador est� en el campo de visi�n
    public bool isPlayerInMeleeRange = false; // Variable para detectar si el jugador est� en distancia melee
    public bool isPlayerInLineOfSight = false; // Variable para detectar si el jugador est� en l�nea de visi�n

    public float checkDelay = 0.045f; // Tiempo entre chequeos de raycast
    private float lastCheckTime;
    private float checkInterval = 10;

    private void Start()
    {
        InvokeRepeating("CheckPlayerInFOVRaycastOnly", 0f, checkDelay);
        InvokeRepeating("CheckPlayerInMeleeDistance", 0f, checkDelay);
    }

    private void Update()
    {
        if (isPlayerInFOV)
        {
            RotateTowardsPlayer();
        }

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
                        return;
                    }
                }
            }
        }

        if (playerColliders.Length == 0)
        {
            isPlayerInFOV = false;
        }
    }

    private void CheckPlayerInFOVRaycastOnly()
    {
        //if enemymanager is null, return
        if (EnemyManager.Instance == null)
        {
            return;
        }

        Collider[] playerColliders = Physics.OverlapSphere(transform.position, viewDistance, playerLayer);
        Vector3 directionToPlayer = (EnemyManager.Instance.player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < fieldOfViewAngle)
        {
            if (EnemyManager.Instance.IsPlayerInLineOfSight(transform.position, viewDistance, detectableLayer))
            {
                //Debug.Log("Est� en LoS");
                isPlayerInFOV = true;
            }
        }

        if (Time.time - lastCheckTime > checkInterval)
        {
            if (playerColliders.Length == 0 ||
                !EnemyManager.Instance.IsPlayerInLineOfSight(transform.position, viewDistance, detectableLayer))
            {
                isPlayerInFOV = false;
            }

            lastCheckTime = Time.time; // Actualizar el tiempo de la �ltima comprobaci�n
        }
    }

    private void CheckPlayerInMeleeDistance()
    {
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, meleeDistance, playerLayer);

        if (playerColliders.Length > 0)
        {
            isPlayerInMeleeRange = true;
        }
        else
        {
            isPlayerInMeleeRange = false;
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

    public void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = (EnemyManager.Instance.player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > 5f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            lookRotation.eulerAngles = new Vector3(0, lookRotation.eulerAngles.y, 0);   
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        
    }


    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded)
        {
            CancelInvoke();
        }

    }
}
