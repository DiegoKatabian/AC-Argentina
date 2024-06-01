using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public bool isOneTimeOnly = true;
    bool hasBeenTriggered = false;

    private PlayableDirector playableDirector;

    public bool shouldTeleportPlayer = false;
    public Transform playerTeleportTarget;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector.playOnAwake)
        {
           //Debug.Log("trigger: playable director es playonawake, inicializo");
           InitializeCutscene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasBeenTriggered) return;


        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered by player");
            playableDirector.Play();
            InitializeCutscene();
        }
    }

    public void InitializeCutscene()
    {
        Debug.Log("initialize cutscene");
        EventManager.Instance.Trigger(Evento.OnCutsceneStart);
        playableDirector.stopped += OnPlayableDirectorStopped;
        hasBeenTriggered = true;
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director != playableDirector) return;

        if (shouldTeleportPlayer)
        {
            EventManager.Instance.Trigger(Evento.OnCutsceneEnd, playerTeleportTarget.position);
        }
        else
        {
            EventManager.Instance.Trigger(Evento.OnCutsceneEnd);
        }

    }

    private void OnDisable()
    {
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }
}
