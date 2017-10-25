using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public GameObject FoodPrefab;

    public Vector3 center;
    public Vector3 size;
	public GameObject SnakeSection;
    public float sizeSphere = 500;
    private float foodSpawnTime = 5;
	private float snakeSpawnTime = 20;
	private const int FOOD_SIZE = 10;

	public PointLight pointLight;
	public Shader shader;

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnFood", 0, foodSpawnTime);
		InvokeRepeating("SpawnSnake", 10, snakeSpawnTime);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void SpawnFood(){

        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));

        GameObject food = Instantiate<GameObject>(FoodPrefab, pos, Quaternion.identity);
		FoodController foodController = (FoodController) food.GetComponent ("FoodController");
		foodController.addLengthWhenEaten = FOOD_SIZE;
        
    }

    void OnDrawGizmosSelected(){

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(center, sizeSphere);
    }

	public void SpawnSnake() {
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		char[] randNameChars = new char[8];
		for (int i = 0; i < randNameChars.Length; i++) {
			randNameChars[i] = chars[Random.Range(0, chars.Length)];
		}
		string randName = new string (randNameChars);

		Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
		Vector3 dir = (center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2))).normalized;
		GameObject snakeObj = new GameObject ("__snake_" + randName);

		snakeObj.tag = "AISnake";

		Snake snake = snakeObj.AddComponent (typeof(Snake)) as Snake;
        snake.SpawnObject = this.gameObject;
		snake.initHeadLocation = pos;
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
}
