using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementScoreOnDestroy : MonoBehaviour {

    public int incrementAmount = 10;
    public ScoreManager scoreManager;

	// Use this for initialization
	void Start ()
    {
		if (scoreManager == null)
        {
            this.scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        }
	}
	
	
	void OnDestroy ()
    {
        this.scoreManager.score += this.incrementAmount;
	}
}
