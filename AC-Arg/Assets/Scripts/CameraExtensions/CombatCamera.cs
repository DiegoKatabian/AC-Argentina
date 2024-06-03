using UnityEngine;
using Cinemachine;
using Climbing;
using System;

public class CombatCamera : MonoBehaviour
{
    public CombatController player;
    [HideInInspector] public CinemachineVirtualCamera virtualCamera;
    public Transform midPoint;

    public bool isMidPointEnabled = false;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    void Update()
    {
        if (player.currentEnemy != null && isMidPointEnabled)
        {
            //Debug.Log("updateo la posicion del midpoint");
            midPoint.position = (player.transform.position + player.currentEnemy.transform.position) / 2f;
        }
    }
}
