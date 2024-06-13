using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public bool isOneTimeOnly = true;
    bool hasBeenTriggered = false;

    private PlayableDirector playableDirector;

    public bool shouldTeleportPlayer = false;
    public Transform playerTeleportTarget;

    public SubtitleSetSO subtitleSet;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector.playOnAwake)
        {
           Debug.Log("cutscene: triggered on awake");
           InitializeCutscene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasBeenTriggered) return;


        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("cutscene: triggered by player");
            playableDirector.Play();
            InitializeCutscene();
        }
    }

    public void InitializeCutscene()
    {
        Debug.Log("cutscene: initialize");
        EventManager.Instance.Trigger(Evento.OnCutsceneStart, subtitleSet);
        playableDirector.stopped += OnPlayableDirectorStopped;
        hasBeenTriggered = true;
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director != playableDirector) return;
        Debug.Log("cutscene: on playable director stopped");

        if (shouldTeleportPlayer)
        {
            EventManager.Instance.Trigger(Evento.OnCutsceneEnd, playerTeleportTarget.position, this);
        }
        else
        {
            EventManager.Instance.Trigger(Evento.OnCutsceneEnd, Vector3.zero, this);
        }

    }

    private void OnDisable()
    {
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }
}
