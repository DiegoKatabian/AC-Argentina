using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class RespawnManager : Singleton<RespawnManager>
{
    [SerializeField] private Transform[] _spawnPoints; // Transforms asociados a cada cutscene.
    private Transform _currentSpawnPoint; // El punto actual de respawn.
    private int _currentCutsceneIndex = -1; // Índice de la última cutscene vista.
    //private bool _hasSeenAtLeastOneCutscene = false;
    public Dictionary<int, bool> seenCutscenes = new Dictionary<int, bool>();

    public int CurrentCutsceneIndex => _currentCutsceneIndex;
    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            seenCutscenes.Add(i, false);
        }
    }

    public void UpdateSpawnPoint(int cutsceneNumber)
    {
        Debug.Log("updating spawn point");

        if (cutsceneNumber >= 0 && cutsceneNumber < _spawnPoints.Length)
        {
            Debug.Log("current cutscene index: " + cutsceneNumber);
            _currentSpawnPoint = _spawnPoints[cutsceneNumber];
            Debug.Log("current spawn point " + _currentSpawnPoint.position);
            _currentCutsceneIndex = cutsceneNumber;
            seenCutscenes[cutsceneNumber] = true;
        }
    }

    public bool HasSeenAtLeastOneCutscene()
    {
        return seenCutscenes.TryGetValue(0, out bool value);
    }

    public void ResetForNewGame()
    {
        _currentCutsceneIndex = 0; // Solo la primera cutscene cuenta.
        _currentSpawnPoint = _spawnPoints[0];
        foreach (int key in seenCutscenes.Keys.ToList())
        {
            seenCutscenes[key] = key == 0;
        }   
    }

    public void TriggerPlayerResetPosition()
    {
        if (_currentSpawnPoint != null)
        {
            Debug.Log("TriggerPlayerResetPosition: " + _currentSpawnPoint.position);
            StartCoroutine(RespawnPlayer());
        }
        else
        {
            Debug.Log("current spawn point was null");
        }
    }

    public IEnumerator RespawnPlayer()
    {
        //wait until all start methods have been called
        yield return new WaitForEndOfFrame();
        EventManager.Instance.Trigger(Evento.OnPlayerResetPosition, _currentSpawnPoint.position);
    }
}
