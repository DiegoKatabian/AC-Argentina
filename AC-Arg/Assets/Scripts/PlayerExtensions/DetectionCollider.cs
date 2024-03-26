using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetectionCollider : MonoBehaviour
{
    public string tagToDetect = "";

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == tagToDetect)
        {
            OnTagDetectedEnter(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == tagToDetect)
        {
            OnTagDetectedExit(other);
        }
    }

    public abstract void OnTagDetectedEnter(Collider other);
    public abstract void OnTagDetectedExit(Collider other);


}
