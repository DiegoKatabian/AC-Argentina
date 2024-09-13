using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public bool soundEnabled = true;
    public string sceneFacundo = "9 de Julio";
    public string sceneVaruzhan = "Varuzhan";
    public AudioClip mainMenuMusic;
    public AudioClip gameStartSFX;

    public GameObject allMainScreenButtonsParent;
    public float timeToWaitBeforeButtonsAppear = 1;

    private void Start()
    {
        //ask the audiomanager if mainmenumusic is already playing. if it is, do nothing. if it is not, play it. print a debug.log for each case


        if (AudioManager.Instance.IsPlaying(mainMenuMusic))
        {
            Debug.Log("MainMenuMusic is already playing");
        }
        else
        {
            AudioManager.Instance.PlaySound(mainMenuMusic);
            //Debug.Log("MainMenuMusic is not playing, so I'm playing it now");
        }

        //AudioManager.Instance.PlaySound(mainMenuMusic);
        Invoke("ShowMainScreenButtons", timeToWaitBeforeButtonsAppear);
    }

    public void ShowMainScreenButtons()
    {
        allMainScreenButtonsParent.SetActive(true);
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        Debug.Log("sound " + soundEnabled);
    }

    public void BUTTON_LoadScene_Facundo()
    {
        AudioManager.Instance.StopSound(mainMenuMusic);
        AudioManager.Instance.PlaySound(gameStartSFX);
        SceneManager.LoadScene(sceneFacundo);

    }

    public void BUTTON_LoadScene_Varuzhan()
    {
        AudioManager.Instance.StopSound(mainMenuMusic);
        AudioManager.Instance.PlaySound(gameStartSFX);
        SceneManager.LoadScene(sceneVaruzhan);
    }

    public void BUTTON_QuitGame()
    {
        Application.Quit();
    }

    public void BUTTON_KimmiArts()
    {
        Application.OpenURL("https://kimmiarts.itch.io/");
    }
}
