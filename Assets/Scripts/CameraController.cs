﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {

	// Very simple code designed to work with the camera inside the player class
	// I'm not commenting it because it's really simple

	public float cameraSpeed = 400.0f;
	public GameObject player;

	void LateUpdate () {
		transform.RotateAround (player.transform.position, Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * cameraSpeed);
		transform.RotateAround (player.transform.position, Vector3.right, Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSpeed);
	}

	public Vector3 PlayerFaceTo()
	{
		Vector3 offset = new Vector3 (0, 0, 10);
		return (transform.position + offset);
	}
}

// version 0.01
// LUCA DEL VECCHIO