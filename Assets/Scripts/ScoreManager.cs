using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public  int score = 0;
    public int incrementFood = 10;
    public int incrementSnake = 100;
    public int length = 0;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        this.score = 0;
    }

    public void addFoodScore()
    {
        this.score += incrementFood;
    }

    public void addSnakeScore()
    {
        this.score += incrementSnake;
    }

    public void setSnakeLength(int length)
    {
        this.length = length;
    }

  
}
