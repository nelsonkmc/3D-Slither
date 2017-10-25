using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameController : MonoBehaviour {

    public Text scoreText;

    public ScoreManager scoreManager;

    // Use this for initialization
    void Start ()
    {
        this.scoreManager.score = 0;

	}
	
	// Update is called once per frame
	void Update ()
    {
        scoreText.text = "Score: " + scoreManager.score;
    }
}
