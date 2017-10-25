using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour {

    public GameObject SnakePrefab;
    public GameObject SpawnObject;
    public GameObject ScoreManager;
	public Shader shader;
	public PointLight pointLight;
	public Color color;

	private List<Vector3> bodyCoordinates;
	private Vector3 direction;
	private List<GameObject> bodySections;
	private Vector3 headPosition;
	private int pendingLength;
	private bool highSpeed;

	public const float INIT_LENGTH = 6;
	public const float POINT_DELTA = 0.1f;
	public int lengthPerFood;
	public Vector3 initHeadLocation;
	public Vector3 initHeadDirection;

    // Use this for initialization
    void Start () {
		direction = initHeadDirection.normalized;
		this.transform.position = initHeadLocation;
		InitBodyList ();

		// Just generate
		this.InitBody();

		this.pendingLength = 0;
		this.highSpeed = false;
    }

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Food")
		{
			FoodController foodController = (FoodController) col.gameObject.GetComponent ("FoodController");
			pendingLength += foodController.addLengthWhenEaten;
			Destroy(col.gameObject);
                    
            GameObject obj = Instantiate(foodController.explosion);
            obj.transform.position = col.transform.position;

            SpawnObject so = SpawnObject.gameObject.GetComponent<SpawnObject>();
            so.SpawnFood();
        }

        if (col.gameObject.tag == "Food" && this.gameObject.tag == "Player")
        {
            ScoreManager sm = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            sm.addFoodScore();
        }

		// when AISnake head hit PlayerSnakeBody
		if (this.gameObject.tag == "AISnake" && col.gameObject.tag == "PlayerSnakeBody")
		{
            ScoreManager sm = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
            sm.addSnakeScore();

            this.DestroyThis ();
		}

		// when Player head hit AISnakeBody
		if (this.gameObject.tag == "Player" && col.gameObject.tag == "AISnakeBody")
		{
			SceneManager.LoadScene ("GameOver");
		}

		// when AISnake head hit an AISnakeBody
		if (this.gameObject.tag == "AISnake" && col.gameObject.tag == "AISnakeBody")
		{
			// only destroy when the body is not belonging to the head
			if (col.gameObject.transform.parent.gameObject != this.gameObject)
			{
				this.DestroyThis ();
                
			}
		}
	}

	/**
	 * Initialise the body List
	 */
	private void InitBodyList () {
		this.bodyCoordinates = new List<Vector3> ();
		int pointsNum = (int) (INIT_LENGTH / POINT_DELTA);

		this.bodyCoordinates.Add (initHeadLocation);
		for (int i = 1; i < pointsNum; i++) {
			this.bodyCoordinates.Add (this.bodyCoordinates [i - 1] - direction * POINT_DELTA);
		}
	}
	
	// Update is called once per frame
	void Update () {
        ScoreManager sm = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        sm.setSnakeLength(this.getLength());

        Debug.Log( "Magnitude :" + this.transform.position.magnitude);

        if (this.transform.position.magnitude > 100) {
			if (this.tag == "Player") {
				SceneManager.LoadScene ("GameOver");
			} else {
				this.DestroyThis ();
			}
		}

        //Debug.Log("Snake Length: " + this.getLength());

        if (this.getLength() > 9999)
        {
            if (this.tag == "Player")
            {
                SceneManager.LoadScene("GameOver");
            }
        }

		this.SetHeadPosition (this.transform.position);

		this.UpdateDirection ();

		// update the body child object
		this.UpdateBody ();
	}

	/*
	 * set the head position
	 */
	private void SetHeadPosition (Vector3 position) {
		this.headPosition = position;
	}

	/*
	 * Update the points in the direction
	 */
	private void UpdateDirection () {
		if (this.pendingLength > 0) {
			Vector3 position = this.bodyCoordinates [this.bodyCoordinates.Count - 1];
			this.bodyCoordinates.Add (position);
			this.addSection (position);
			this.pendingLength--;
		}

		List<Vector3> newCoordinates = new List<Vector3> ();
		float t = Time.deltaTime;

		float headMag = (headPosition - this.bodyCoordinates [0]).magnitude;

		// init the head to go towards the direction
		newCoordinates.Add (headPosition);
		//this.bodyCoordinates [0] = headPosition;

		for (int i = 1; i < this.bodyCoordinates.Count; i++) {
			Vector3 pointDirection = (this.bodyCoordinates [i - 1] - bodyCoordinates [i]).normalized;

			Vector3 tempNewPoint = this.bodyCoordinates [i] + pointDirection * headMag;

			// make sure the new points are POINT_DELTA distance apart, only perform when too far
			if ((tempNewPoint - newCoordinates [i - 1]).magnitude > POINT_DELTA) {
				newCoordinates.Add (newCoordinates [i - 1] + (tempNewPoint - newCoordinates [i - 1]).normalized * POINT_DELTA);
			} else {
				newCoordinates.Add (tempNewPoint);
			}
		}

		this.bodyCoordinates = newCoordinates;
	}

	private void InitBody () {
		this.bodySections = new List<GameObject> ();
		for (int i = 0; i < bodyCoordinates.Count - 1; i++) {
			GameObject snakeSection = GameObject.Instantiate<GameObject> (SnakePrefab);
            snakeSection.transform.parent = transform;
            snakeSection.transform.position = bodyCoordinates [i];
			bodySections.Add (snakeSection);

			LightBehaviour lightBehaviour = snakeSection.AddComponent (typeof(LightBehaviour)) as LightBehaviour;
			lightBehaviour.pointLight = this.pointLight;
			lightBehaviour.shader = this.shader;
			lightBehaviour.color = this.color;
		}
	}

	private void UpdateBody () {
		for (int i = 0; i < bodyCoordinates.Count - 1; i++) {
            GameObject snakeSection = bodySections[i];
			snakeSection.transform.position = bodyCoordinates[i];
		}
	}

	private void addSection (Vector3 position) {
		GameObject snakeSection = GameObject.Instantiate<GameObject> (SnakePrefab);
        snakeSection.transform.parent = transform;

        snakeSection.transform.position = position;
		bodySections.Add (snakeSection);
	}

	private void ShortenAndDropFood () {
		
	}

	public int getLength() {
		return this.bodyCoordinates.Count;
	}

	public float maxTurning() {
		return 7.0f;
	}

	public void applyHighSpeed(bool isHighSpeed) {
		this.highSpeed = isHighSpeed;
	}

	private void DestroyThis() {
		int childCound = this.gameObject.transform.childCount;
		for (int i = 0; i < this.gameObject.transform.childCount; i++) {
			GameObject child = this.gameObject.transform.GetChild (i).gameObject;
			Destroy (child);
		}
		Destroy (this.gameObject);
	}
}
