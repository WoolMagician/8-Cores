using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControllerNew : MonoBehaviour {

	// Very simple code designed to work with the camera inside the player class
	// I'm not commenting it because it's really simple

	public float cameraSpeed = 10.0f;
	public GameObject player;
	Vector3 offset = new Vector3 (0, 0, 30);

    private float orbitHorizontal;
    private float orbitVertical;

    private void Update()
    {
        orbitHorizontal = Input.GetAxis("Mouse X") * Time.deltaTime;
        orbitVertical = Input.GetAxis("Mouse Y") * Time.deltaTime;
    }

    void LateUpdate () {

		transform.RotateAround(player.transform.position, Vector3.up, orbitHorizontal * (cameraSpeed * 100));
		transform.RotateAround(player.transform.position, Vector3.right, orbitVertical * (cameraSpeed * 100));

	}

	public Vector3 PlayerFaceTo()
	{
		return (transform.position + offset);
	}
}

// version 0.01
// LUCA DEL VECCHIO
