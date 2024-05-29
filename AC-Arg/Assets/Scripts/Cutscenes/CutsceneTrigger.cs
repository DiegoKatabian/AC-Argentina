using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public bool isOneTimeOnly = true;
    bool hasBeenTriggered = false;

    //get the playable director in roder to trigger the timeline ontriggerenter

    private PlayableDirector playableDirector;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasBeenTriggered) return;


        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered by player");
            StartTimeline();
            hasBeenTriggered = true;
        }
    }

    public void StartTimeline()
    {
        playableDirector.Play();
        EventManager.Instance.Trigger(Evento.OnCutsceneStart);
    }
}
