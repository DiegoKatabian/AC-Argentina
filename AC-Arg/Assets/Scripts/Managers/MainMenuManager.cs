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

    public GameObject allMainScreenButtonsParent;
    public float timeToWaitBeforeButtonsAppear = 1;

    private void Start()
    {
        AudioManager.Instance.PlaySound(mainMenuMusic);
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
        SceneManager.LoadScene(sceneFacundo);

    }

    public void BUTTON_LoadScene_Varuzhan()
    {
        AudioManager.Instance.StopSound(mainMenuMusic);
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
