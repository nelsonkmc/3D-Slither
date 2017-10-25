using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeController : MonoBehaviour {

	private const float NORM_SPEED = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Just find all the foods on map, find the closest food to the snake, try to rotate towards.
		FoodController[] foodControllers = FindObjectsOfType(typeof(FoodController)) as FoodController[];

		// Make there are food.
		if (foodControllers.Length == 0) {
			// need to just move foward

			return;
		}

		// just from FoodController to GameObject.
		List<GameObject> foods = new List<GameObject> ();
		foreach (FoodController foodController in foodControllers) {
			foods.Add (foodController.gameObject);
		}

		// Find target.
		GameObject closestFood = foods [0];
		float dist = (this.transform.position - closestFood.transform.position).magnitude;
		foreach (GameObject food in foods) {
			float tempDist = (this.transform.position - food.transform.position).magnitude;
			if (tempDist < dist) {
				dist = tempDist;
				closestFood = food;
			}
		}

		// Try to turn to the target.
		Vector3 direction = (closestFood.transform.position - this.transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		this.transform.rotation = lookRotation;

		this.transform.position += transform.forward * NORM_SPEED * Time.deltaTime;
	}
}
