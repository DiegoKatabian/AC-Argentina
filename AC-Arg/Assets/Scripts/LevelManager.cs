using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    // Update is called once per frame
    private void Start()
    {
        //subscribe to eventmanger only event with the pausemenutrigger method
        EventManager.Subscribe(Evento.OnPlayerPressedEsc, PauseMenuTrigger);
    }

    private void PauseMenuTrigger(object[] parameters)
    {
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

    public void QuitGame()
    {
        Debug.Log("quiteo porque toque el boton en el pause menu");
        Application.Quit();
    }
}
