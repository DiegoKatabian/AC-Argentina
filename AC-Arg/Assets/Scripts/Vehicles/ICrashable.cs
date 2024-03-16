using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICrashable
{
    public void OnCrash(GameObject vehicle, float crashForce);
}
