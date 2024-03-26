using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverMenu;

    private void Awake()
    {
        EventManager.Subscribe(Evento.OnInputRequestPause, PauseMenuTrigger);
        EventManager.Subscribe(Evento.OnPlayerDied, GameOverMenuTrigger);
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

    public void RestartGame()
    {
        Time.timeScale = 1;
        //gameOverMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("quiteo porque toque el boton en el pause menu");
        Application.Quit();
    }
}
