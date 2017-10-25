using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public GameObject FoodPrefab;
	public ScoreManager sm;

    public Vector3 center;
    public Vector3 size;
	public GameObject SnakeSection;
    public float sizeSphere = 100;
    private float foodSpawnTime = 5;
	private float snakeSpawnTime = 10;
	public const int FOOD_SIZE = 10;
	public const int MAX_FOOD_COUNT = 50;
	public const int MAX_SNAKE_COUNT = 20;
	private Color[] COLORS = new Color[] {
		Color.red,
		Color.green,
		Color.cyan,
		Color.blue
	};

	private static int foodCount;
	private static int snakeCount;

	public PointLight pointLight;
	public Shader shader;

	// Use this for initialization
	void Start () {
		foodCount = 0;
		snakeCount = 0;
		InvokeRepeating("SpawnFood", 0, foodSpawnTime);
		InvokeRepeating("SpawnSnake", 10, snakeSpawnTime);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void SpawnFood(){
		if (foodCount >= MAX_FOOD_COUNT) {
			return;
		}
		foodCount++;

		Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2)) .normalized * Random.Range(0, this.sizeSphere);

		GameObject food = Instantiate<GameObject>(FoodPrefab, pos, this.transform.rotation);
		food.transform.position = pos;
		FoodController foodController = (FoodController) food.GetComponent ("FoodController");
		foodController.addLengthWhenEaten = FOOD_SIZE;

		Color color = COLORS [((int)Random.Range (0, COLORS.Length))];
		food.GetComponent<Renderer>().material.color = color;
    }

    void OnDrawGizmosSelected(){

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(center, sizeSphere);
    }

	public void SpawnSnake() {
		if (snakeCount >= MAX_SNAKE_COUNT) {
			return;
		}
		snakeCount++;

		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		char[] randNameChars = new char[8];
		for (int i = 0; i < randNameChars.Length; i++) {
			randNameChars[i] = chars[Random.Range(0, chars.Length)];
		}
		string randName = new string (randNameChars);

		Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
		Vector3 dir = (center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2))).normalized;
		GameObject snakeObj = new GameObject ("__snake_" + randName);
		snakeObj.transform.position = pos;

		snakeObj.tag = "AISnake";

		Snake snake = snakeObj.AddComponent (typeof(Snake)) as Snake;
        snake.so = this;
		snake.sm = sm;
		snake.initHeadDirection = dir;
		snake.SnakePrefab = SnakeSection;
		snake.shader = this.shader;
		snake.pointLight = this.pointLight;
		snake.color = new Color (
			104.0f / 255.0f, 
			67.0f / 255.0f,
			253.0f / 255.0f,
			255.0f / 255.0f);

		snakeObj.AddComponent (typeof(AISnakeController));

		Rigidbody rigidbody = snakeObj.AddComponent (typeof(Rigidbody)) as Rigidbody;
		rigidbody.mass = 1;
		rigidbody.drag = 0;
		rigidbody.angularDrag = 0.05f;
		rigidbody.isKinematic = true;
		rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        
		SphereCollider sphereCollider = snakeObj.AddComponent (typeof(SphereCollider)) as SphereCollider;
		sphereCollider.center = Vector3.zero;
		sphereCollider.radius = 0.5f;

	}

	public void decreaseFoodCount() {
		foodCount--;
	}

	public void decreaseSnakeCount() {
		snakeCount--;
	}
}
