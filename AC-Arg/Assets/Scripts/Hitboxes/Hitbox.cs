using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public string tagToHit;
    public bool isTaggedInside = false;

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToHit))
        {
            isTaggedInside = true;
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToHit))
        {
            isTaggedInside = false;
        }
    }
}
