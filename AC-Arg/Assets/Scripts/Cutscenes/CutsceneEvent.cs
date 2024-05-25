using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutsceneEvent : ScriptableObject
{
    public float Delay = 0;
    public abstract void Execute();
}
