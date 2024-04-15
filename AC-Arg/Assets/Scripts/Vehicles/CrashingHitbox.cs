using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashingHitbox : MonoBehaviour
{
    public float crashForce = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ICrashable>() != null)
        {
            //Debug.Log("le doy a un crashable");
            ICrashable crashable = other.GetComponent<ICrashable>();
            crashable.OnCrash(gameObject, crashForce);
        }
    }
}
