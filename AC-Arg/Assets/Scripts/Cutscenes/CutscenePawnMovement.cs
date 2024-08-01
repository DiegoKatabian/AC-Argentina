using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class CutscenePawnMovement : MonoBehaviour
{
    //this script should be attached to the dummies that will be moved in the cutscenes
    //these dummies have a navmesh agent, and move from cue to cue or tp directly

    private NavMeshAgent agent;

    public float walkSpeed = 1.1f;
    public float jogSpeed = 1.55f;
    public float runSpeed = 2f;

    void Start()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogError("No NavMeshAgent component found in " + this);
        }
        else
        {
            //Debug.Log("character: tengo navmesh");
            agent = GetComponent<NavMeshAgent>();
        }
    }

    public void MoveToNextQueue(bool isDirectMove)
    {
        Transform nextCue = MovementCueManager.Instance.GetNextCue(this);

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
            Debug.Log("character " + gameObject.name + ": muevo al agente");
            agent.SetDestination(nextCue.position);
            //transform.rotation = nextCue.rotation;
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
        //transform.rotation = nextCue.rotation;

        if (agent != null)
        {
            agent.enabled = true;
        }
    }

    //create a series of methods to be triggered by the timeline, same signature as MoveToNextQueue. 
    //these methods should set the navmesh speed to one of three possible values, 1.1, 2, 4, for walk, jog and run respectively
    public void SetWalkSpeed()
    {
        agent.speed = walkSpeed;
    }

    public void SetJogSpeed()
    {
        agent.speed = jogSpeed;
    }

    public void SetRunSpeed()
    {
        agent.speed = runSpeed;
    }

}
