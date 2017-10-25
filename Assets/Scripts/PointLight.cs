using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour {

	public Color color;
    public float spinSpeed = 30;

    public void Start()
    {
        this.transform.position = new Vector3(0, 0, 800);
    }

    public void Update()
    {
        this.transform.RotateAround(Vector3.zero, Vector3.right, spinSpeed * Time.deltaTime);
    }

    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}
