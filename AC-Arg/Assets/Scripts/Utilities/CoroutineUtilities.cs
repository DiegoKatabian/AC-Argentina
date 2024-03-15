using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtilities : MonoBehaviour
{
    public IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
