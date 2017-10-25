using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private const float NORM_SPEED = 5;
    private const float FAST_SPEED = 7;
    private const int rightClick = 1;
    private float spinSpeed = 10;
    
	private float speed;

	private Snake snake;

    //public string tagToEat;

    // Use this for initialization
    void Start () {
		speed = NORM_SPEED;

		snake = (Snake) gameObject.GetComponent("Snake");
	}
	
	// Update is called once per frame
	void Update () {
		float yRotate = Input.GetAxis ("Mouse Y") * spinSpeed;
		float xRotate = Input.GetAxis ("Mouse X") * spinSpeed;
		float totalRotate = Mathf.Sqrt (xRotate * xRotate + yRotate * yRotate);

		float maxRotate = snake.maxTurning ();
		if (totalRotate > maxRotate) {
			float ratio = maxRotate / totalRotate;
			yRotate *= ratio;
			xRotate *= ratio;
		}

		this.transform.Rotate(-yRotate, xRotate, 0);

		if (Input.GetMouseButton(rightClick) ||
			Input.GetKey (KeyCode.Space))
        {
			speed = FAST_SPEED;
			snake.applyHighSpeed (true);
        } else
        {
			speed = NORM_SPEED;
			snake.applyHighSpeed (false);
        }

		this.transform.position += transform.forward * speed * Time.deltaTime;
    }
}
