using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        //access scene manager and load game scene
        SceneManager.LoadScene(1); //game scene
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
