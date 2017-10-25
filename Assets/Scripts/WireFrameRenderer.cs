using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireFrameRenderer : MonoBehaviour {
	public Shader shader;
	private MeshRenderer meshRenderer;

	private GameObject player;
	private GameObject playerCamera;

	// Use this for initialization
	void Start () {
		meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material.shader = shader;

		player = GameObject.FindGameObjectWithTag ("Player");
		playerCamera = GameObject.FindGameObjectWithTag ("Camera");

		meshRenderer.material.SetVector("_PlayerPosition", player.transform.position);
		meshRenderer.material.SetVector("_CameraDirection", playerCamera.transform.forward);
		meshRenderer.material.SetFloat("_WorldRadius", transform.localScale.x);
	}
	
	// Update is called once per frame
	void Update () {
		meshRenderer.material.SetVector("_PlayerPosition", player.transform.position);
		meshRenderer.material.SetVector("_CameraDirection", playerCamera.transform.forward);
	}
}
