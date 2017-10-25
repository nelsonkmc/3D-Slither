using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBehaviour : MonoBehaviour {

	public Shader shader;
	public PointLight pointLight;
	public Color color;

	private MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
		// Add a MeshRenderer component. This component actually renders the mesh that
		// is defined by the MeshFilter component.
		meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material.shader = shader;

		meshRenderer.material.SetColor("_PointLightColor", this.color * pointLight.color);
		meshRenderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
	}
	
	// Update is called once per frame
	void Update () {
	}
}
