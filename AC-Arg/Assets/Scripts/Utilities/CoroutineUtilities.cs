using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtilities
{
    public static IEnumerator DelayedAction(float delay, Action<object[]> action, params object[] parameters)
    {
        yield return new WaitForSeconds(delay);
        action(parameters);
    }
}
