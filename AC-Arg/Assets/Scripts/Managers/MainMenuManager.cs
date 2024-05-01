using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public bool soundEnabled = true;
    public string sceneFacundo = "9 de Julio";
    public string sceneVaruzhan = "Varuzhan";

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        Debug.Log("sound " + soundEnabled);
    }

    public void BUTTON_LoadScene_Facundo()
    {
        SceneManager.LoadScene(sceneFacundo);
    }

    public void BUTTON_LoadScene_Varuzhan()
    {
        SceneManager.LoadScene(sceneVaruzhan);
    }

    public void BUTTON_QuitGame()
    {
        Application.Quit();
        Application.OpenURL("https://kimmiarts.itch.io/");
    }
}
