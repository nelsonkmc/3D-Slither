using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("GameOver");
        Application.Quit();
    }
   
}
