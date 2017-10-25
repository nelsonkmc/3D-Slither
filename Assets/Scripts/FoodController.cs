using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {

	public int addLengthWhenEaten;
    public GameObject explosion;

	private float worldRadius;

    void Start()
    {
		worldRadius = GameObject.FindGameObjectWithTag ("World").transform.localScale.x;
    }

	void Update()
	{
		// Food get outside of sphere, destroy this
		if (this.transform.position.magnitude > worldRadius) {
			Destroy (this.gameObject);
		}
	}
}
