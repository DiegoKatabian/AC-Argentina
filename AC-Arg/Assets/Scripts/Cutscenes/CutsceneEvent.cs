using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CutsceneEvent : ScriptableObject
{
    public float Delay { get; }
    public abstract void Execute();
}
