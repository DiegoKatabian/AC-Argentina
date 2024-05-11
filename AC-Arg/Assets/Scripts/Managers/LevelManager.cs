using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //en realidad es mas como el ThisLevelCanvasEnabler

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;

    [SerializeField] AudioClip thisLevelAmbience;

    private void Start()
    {
        EventManager.Instance.Subscribe(Evento.OnInputRequestPause, PauseMenuTrigger);
        EventManager.Instance.Subscribe(Evento.OnPlayerDied, GameOverMenuTrigger);

        AudioManager.Instance.PlaySound(thisLevelAmbience);
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

    public void GameOverMenuTrigger(object[] parameters)
    {
        if (gameOverMenu == null)
        {
            Debug.Log("no tengo referencia al game over menu");
            return;
        }

        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BUTTON_QuitGame()
    {
        Debug.Log("quiteo porque toque el boton en el pause menu");
        Application.Quit();
    }

    //a similar method, but it loads the main menu scene
    public void BUTTON_LoadMainMenu()
    {
        Time.timeScale = 1;
        AudioManager.Instance.StopSound(thisLevelAmbience);
        SceneManager.LoadScene("MainMenu");
    }
}
