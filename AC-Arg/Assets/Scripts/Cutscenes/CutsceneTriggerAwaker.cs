using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(BoxCollider))]
public class CutsceneTriggerAwaker : MonoBehaviour
{
    /// <summary>
    ///this script should be attached to a box collider (trigger).
    /// if a certain condition is met while the player is inside this trigger,
    /// a certain cutscenetrigger will be awaken.
    /// </summary>

    [SerializeField] private CutsceneTrigger cutsceneTrigger;
    [SerializeField] Evento condition;
    [SerializeField] float delay = 2f;

    bool isPlayerInside = false;
    bool hasBeenTriggered = false;

    private void Start()
    {
        EventManager.Instance.Subscribe(condition, OnConditionMet);
    }

    private void OnConditionMet(params object[] parameters)
    {
        if (isPlayerInside && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            StartCoroutine(AwakeCutsceneTriggerCoroutine());
        }
        else
        {
            Debug.Log("cutscene trigger awaker: condition was met, but the player wasnt inside the collider");
        }

    }

    IEnumerator AwakeCutsceneTriggerCoroutine()
    {
        yield return new WaitForSeconds(delay);
        cutsceneTrigger.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubscribe(condition, OnConditionMet);
    }

}
