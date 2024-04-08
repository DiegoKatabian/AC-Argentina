using UnityEngine;
using Cinemachine;
using Climbing;
using System;

public class CombatCamera : MonoBehaviour
{
    public CombatController player;
    public CinemachineVirtualCamera virtualCamera;
    public Transform midPoint;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    void Update()
    {
        if (player.currentEnemy != null)
        {
            //Debug.Log("updateo la posicion del midpoint");
            midPoint.position = (player.transform.position + player.currentEnemy.transform.position) / 2f;
        }
    }
}
