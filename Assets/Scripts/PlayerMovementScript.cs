using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

	// Animator
	public Animator anim;
	// Camera object to be referenced, double a to avoid problems
	public GameObject cameraa;
	// To be exchanged with values from the character
	public float forwardSpeed = 8.0f;
	public float lateralSpeed = 4.0f;
	public float backwardSpeed = 2.0f;

	// Taking reference to the camera
	//private GameObject reference = GameObject.Find ("Camera");

	void Start()
	{
		anim = GetComponent<Animator> ();
	}

	void Update()
	{
		// This can be heavily optimized via input checks, omw
		// This needs to be animated via anim.Play()
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * forwardSpeed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * forwardSpeed;

		// Next line is commented until i get the way to making this fucking object to return
		//transform.LookAt (CameraController.PlayerFaceTo());
		transform.Translate(x, 0, z);
	}

}

// Version 0.01
// LUCA  DEL VECCHIO