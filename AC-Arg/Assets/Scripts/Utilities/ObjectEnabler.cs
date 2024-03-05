using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectEnabler
{
    //this is a static everything that will provide methods to other classes

    public static void EnableObject(GameObject go, bool state)
    {
        go.SetActive(state);
    }

    public static void EnableObject(Component comp, bool state)
    {
        comp.gameObject.SetActive(state);
    }

    public static void EnableObject(Renderer rend, bool state)
    {
        rend.enabled = state;
    }

    public static void EnableObject(Collider col, bool state)
    {
        col.enabled = state;
    }

    public static void EnableObject(Rigidbody rb, bool state)
    {
        rb.isKinematic = !state;
    }


}
