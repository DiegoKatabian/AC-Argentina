using UnityEngine;
using UnityEngine.AI;

public class CutscenePawnMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("No NavMeshAgent component found in " + gameObject.name);
        }
        else
        {
            Debug.Log("character: tengo navmesh");
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public void MoveToNextQueue(bool isDirectMove)
    {
        Transform nextCue = MovementCueManager.Instance.GetNextCue(gameObject.name);

        Debug.Log("character: obtengo el siguiente cue");

        if (nextCue != null)
        {
            if (isDirectMove)
            {
                MoveDirectlyTo(nextCue.position);
            }
            else
            {
                MoveTo(nextCue.position);
            }
        }
        else
        {
            Debug.Log("character: el nextcue era null");
        }
    }

    public void MoveTo(Vector3 position)
    {
        if (agent != null)
        {
            Debug.Log("character: muevo al agente");
            agent.SetDestination(position);
        }
    }

    public void MoveDirectlyTo(Vector3 position)
    {
        if (agent != null)
        {
            agent.enabled = false;
        }
        Debug.Log("character: muevo al agente directamente");
        transform.position = position;
        if (agent != null)
        {
            agent.enabled = true;
        }
    }
}
