using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_BackgroundChange : MonoBehaviour {
	[SerializeField]private Camera camera;
	// Use this for initialization
	void Start () {
		camera = GetComponent <Camera> ();
		camera.backgroundColor = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
