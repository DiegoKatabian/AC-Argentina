using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentTest : MonoBehaviour
{
    NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log("tuki soy agent");
        MoveToPlayer();
    }

    public void MoveToPlayer()
    {
        Debug.Log("voy para alla");
        agent.SetDestination(EnemyManager.Instance.player.transform.position);
    }
}
