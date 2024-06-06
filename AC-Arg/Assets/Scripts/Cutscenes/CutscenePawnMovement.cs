using UnityEngine;
using UnityEngine.AI;

public enum PawnName
{
    Facundo,
    Varuzhan
}

public class CutscenePawnMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    public PawnName pawnName;

    void Start()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("No NavMeshAgent component found in " + pawnName);
        }
        else
        {
            Debug.Log("character: tengo navmesh");
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public void MoveToNextQueue(bool isDirectMove)
    {
        Transform nextCue = MovementCueManager.Instance.GetNextCue(pawnName);

        Debug.Log("character: obtengo el siguiente cue");

        if (nextCue != null)
        {
            if (isDirectMove)
            {
                MoveDirectlyTo(nextCue);
            }
            else
            {
                MoveTo(nextCue);
            }
        }
        else
        {
            Debug.Log("character: el nextcue era null");
        }
    }

    public void MoveTo(Transform nextCue)
    {
        if (agent != null)
        {
            Debug.Log("character: muevo al agente");
            agent.SetDestination(nextCue.position);
            transform.rotation = nextCue.rotation;
        }
    }

    public void MoveDirectlyTo(Transform nextCue)
    {
        if (agent != null)
        {
            agent.enabled = false;
        }
        Debug.Log("character: muevo al agente directamente");
        transform.position = nextCue.position;
        transform.rotation = nextCue.rotation;

        if (agent != null)
        {
            agent.enabled = true;
        }
    }
}
