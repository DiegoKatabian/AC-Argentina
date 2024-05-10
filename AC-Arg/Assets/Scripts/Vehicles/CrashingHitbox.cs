using System.Collections;
using System.Collections.Generic;
using TrafficSimulation;
using UnityEngine;

public class CrashingHitbox : MonoBehaviour
{
    public float crashForce = 10f;
    public float minimumVelocityNeededToCrash = 2;
    public VehicleAI vehicleAI;

    private void Start()
    {
        if (vehicleAI == null)
        {
            vehicleAI = transform.parent.GetComponentInParent<VehicleAI>();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ICrashable>() != null)
        {
            //Debug.Log("le doy a un crashable");
            ICrashable crashable = other.GetComponent<ICrashable>();
            TryCrash(crashable);
        }
    }

    void TryCrash(ICrashable crashable)
    {
        if (vehicleAI.GetRBVelocity() < minimumVelocityNeededToCrash)
        {
            Debug.Log("voy demasiado lento para crashear");
            return;
        }

        Debug.Log("voy rapido, crasheo");
        crashable.OnCrash(gameObject, crashForce);
    }

    
}
