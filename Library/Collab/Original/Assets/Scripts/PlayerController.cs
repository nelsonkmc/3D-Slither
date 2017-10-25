using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private const float NORM_SPEED = 5;
    private const float FAST_SPEED = NORM_SPEED * 2;
    private const int rightClick = 1;
    private float spinSpeed = 10;
    
	private float speed;
    

    //public string tagToEat;

    // Use this for initialization
    void Start () {
		speed = NORM_SPEED;
	}
	
	// Update is called once per frame
	void Update () {
		float yRotate = Input.GetAxis ("Mouse Y") * spinSpeed;
		float xRotate = Input.GetAxis ("Mouse X") * spinSpeed;
		float totalRotate = Mathf.Sqrt (xRotate * xRotate + yRotate * yRotate);

		Snake snake = (Snake) gameObject.GetComponent("Snake");
		float maxRotate = snake.maxTurning ();
		if (totalRotate > maxRotate) {
			float ratio = maxRotate / totalRotate;
			yRotate *= ratio;
			xRotate *= ratio;
		}

		this.transform.Rotate(-yRotate, xRotate, 0);

        this.transform.position += transform.forward * speed * Time.deltaTime;

        if (Input.GetMouseButton(rightClick) ||
			Input.GetKey(KeyCode.Space))
        {
			speed = FAST_SPEED;
			snake.applyHighSpeed (true);
        } else
        {
			speed = NORM_SPEED;
			snake.applyHighSpeed (false);
        }

        ScoreManager scoreManager = this.gameObject.GetComponent<ScoreManager>();
        			
    }
}
