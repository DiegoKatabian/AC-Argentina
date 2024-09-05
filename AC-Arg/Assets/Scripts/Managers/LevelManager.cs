using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;

    [SerializeField] AudioClip thisLevelAmbience;
    [SerializeField] AudioClip desyncAudioClip;

    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnInputRequestPause, PauseMenuTrigger);
        EventManager.Instance.Subscribe(Evento.OnPlayerDied, DesyncMenuTrigger);
        AudioManager.Instance.PlaySound(thisLevelAmbience);

        if (SceneManager.GetActiveScene().name == "9 de Julio") //si estamos en la escena facundo, hace cosas con el respawn
        {
            Debug.Log("la escena era 9 de julio");
            if (RespawnManager.Instance.HasSeenAtLeastOneCutscene()) //si vi la primera
            {
                Debug.Log("level manager asks trigger player reset position");
                RespawnManager.Instance.TriggerPlayerResetPosition(); //tpea a donde corresponde
            }
            else if (RespawnManager.Instance.seenCutscenes[4])
            {
                Debug.Log("level manager asks to reset to new game then trigger player reset position");
                RespawnManager.Instance.ResetForNewGame(); //como ya gane el level, lo pongo de nuevo en simular que solo vi la primera
                CutsceneManager.Instance.ResetCutsceneData();
                RespawnManager.Instance.TriggerPlayerResetPosition(); 
            }
            else
            {
                Debug.Log("no vi la primera asi qe va desde arriba");
            }
        }


    }

    private void PauseMenuTrigger(object[] parameters)
    {
        if (pauseMenu == null)
        {
            Debug.Log("no tengo referencia al pause menu");
            return;
        }

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
             pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void DesyncMenuTrigger(object[] parameters)
    {
        if (gameOverMenu == null)
        {
            Debug.Log("no tengo referencia al game over menu");
            return;
        }

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            AudioManager.Instance.PlaySound(desyncAudioClip);
            gameOverMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            gameOverMenu.SetActive(false);
        }
    }

    public void BUTTON_RestartGame()
    {
        Time.timeScale = 1;
        gameOverMenu.SetActive(false);
        AudioManager.Instance.StopSound(thisLevelAmbience);
        AudioManager.Instance.StopAllMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BUTTON_QuitGame()
    {
        Debug.Log("quiteo porque toque el boton en el pause menu");
        Application.Quit();
    }

    public void BUTTON_LoadMainMenu()
    {
        Time.timeScale = 1;
        LoadMainMenu();
    }
    public void SIGNAL_LoadMainMenu()
    {
        LoadMainMenu();
    }

    private void LoadMainMenu()
    {
        AudioManager.Instance.StopSound(thisLevelAmbience);
        AudioManager.Instance.StopCombatMusic();
        SceneManager.LoadScene("MainMenu");
    }

}
