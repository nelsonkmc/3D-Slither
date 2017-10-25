using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {

    private int score = 0;
    private int length = 0;
    private float magnitude = 0;

    public Text highScore;
    public Text endingText;
    public Text buttonText;
   
    public GameObject scoreManager;
    public GameObject snake;

    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager");
		if (scoreManager != null) {
			score = scoreManager.GetComponent<ScoreManager> ().score;
			length = scoreManager.GetComponent<ScoreManager> ().length;
		} else {
			score = 0;
			length = 0;
		}

        if (length >= 1000)
        {
            endingText.text = "You Won!";
            buttonText.text = "Exit";
        } else
        {
            endingText.text = "Game Over";
            buttonText.text = "Try Again!";
        }

        highScore.text = "Final Score: " + score;
    }

    public void Retry()
    {
		GameObject[] scoreManagers = GameObject.FindGameObjectsWithTag("ScoreManager");
		foreach (GameObject scoreManager in scoreManagers) {
			Destroy (scoreManager);
		}
        SceneManager.LoadScene("MainMenu");
    }
}
