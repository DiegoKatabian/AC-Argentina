using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public bool isOneTimeOnly = true;
    bool hasBeenTriggered = false;

    private PlayableDirector playableDirector;

    public bool shouldTeleportPlayer = false;
    public Transform playerTeleportTarget;
    public bool shouldDisappearPlayer = true;

    public SubtitleSetSO subtitleSet;

    public int CutsceneNumber = 0;

    private void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector.playOnAwake)
        {
            InitializeCutscene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasBeenTriggered) return;

        if (other.gameObject.CompareTag("Player"))
        {
            playableDirector.Play();
            InitializeCutscene();
        }
    }

    public void InitializeCutscene()
    {
        EventManager.Instance.Trigger(Evento.OnCutsceneStart, subtitleSet, shouldDisappearPlayer);
        playableDirector.stopped += OnPlayableDirectorStopped;
        hasBeenTriggered = true;
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director != playableDirector) return;
        RespawnManager.Instance.UpdateSpawnPoint(CutsceneNumber);
        EventManager.Instance.Trigger(Evento.OnCutsceneEnd, playerTeleportTarget.position, this);
    }

    private void OnDisable()
    {
        if (playableDirector != null)
        playableDirector.stopped -= OnPlayableDirectorStopped;
    }
}
