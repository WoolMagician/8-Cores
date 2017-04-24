/*
 * - CLASS: BASE PLATFORM CLASS
 * - AUTHOR: FRANCESCO PODDA
 * - VERSION: 1.0
 * - CREATION DATE: MONDAY, 24 APRIL 2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class used for platforms behaviour.
/// </summary>
[System.Serializable]
[RequireComponent(typeof(Rigidbody))]
public class BasePlatform : MonoBehaviour {

    //Used to store RigidBody attached to platform obj.
    private Rigidbody rigidBody;

    //If true enables direction change on collision after wait time.
    public bool changeDirOnCollision = true;

    //Time that platform waits after colliding.
    [Range(0, 10)]
    public int timeBeforeDirChange = 2;

    //Used to lock movement while waiting.
    [HideInInspector]
    public bool isLocked = false;

    //Speed multipliers.
    [Range(-1000, 1000)]
    public float xSpeedMultiplier = 100f;

    [Range(-1000, 1000)]
    public float ySpeedMultiplier = 0f;

    [Range(-1000, 1000)]
    public float zSpeedMultiplier = 0f;

    //Used to store current RigidBody speeds.
    private float xSpeed = 1f;
    private float ySpeed = 1f;
    private float zSpeed = 1f;

    //Used to store current platform direction.
    private float xDirection = 1f;
    private float yDirection = 1f;
    private float zDirection = 1f;

	// Use this for initialization.
	void Start ()
    {
        //Assign attached Rigidbody component to variable.
        rigidBody = this.GetComponent<Rigidbody>();

        //Used to initialize the attached Rigidbody component.
        InitRigidbody();

        //Start moving platform.
        rigidBody.AddForce(new Vector3(xSpeed, ySpeed, zSpeed));
    }

    private void InitRigidbody()
    {
        //Avoid physics rotation bugs while adding forces.
        rigidBody.freezeRotation = true;

        //Keep the platform out of gravity physics.
        rigidBody.useGravity = false;

    }
	
	// Update is called once per frame.
	void Update ()
    {
        //Check if platform is locked before setting velocity.
        if (!isLocked)
        {
            //Update speeds.
            xSpeed *= xSpeedMultiplier * Time.deltaTime;
            ySpeed *= ySpeedMultiplier * Time.deltaTime;
            zSpeed *= zSpeedMultiplier * Time.deltaTime;

            //Set final speed.
            this.rigidBody.velocity = new Vector3(xSpeed, ySpeed, zSpeed);
        }
    }

    /// <summary>
    /// This function is used to wait and change direction of platform.
    /// </summary>
    /// <param name="lockSeconds">Time (s) platform waits after direction change.</param>
    /// <param name="velocity">Velocity assigned to platform after <code>lockSeconds</code>.</param>
    /// <param name="forceDirection">Force added to platform after velocity is setted.</param>
    /// <param name="newDirection">New direction, usually is equal to <code>-Vector3.one</code>.</param>
    /// <returns></returns>
    IEnumerator LockMovAndChangeDir(int lockSeconds, Vector3 velocity, Vector3 forceDirection, Vector3 newDirection)
    {
        //Change directions.
        xSpeed *= newDirection.x;
        ySpeed *= newDirection.y;
        zSpeed *= newDirection.z;

        //Update velocity.
        velocity = new Vector3(xSpeed * xDirection, ySpeed * yDirection, zSpeed * zDirection);

        //Update force.
        forceDirection = new Vector3(xSpeed, ySpeed, zSpeed);

        //Set lock state to true.
        isLocked = true;

        //Reset force & velocity.
        rigidBody.velocity = Vector3.zero;

        yield return new WaitForSeconds(2);

        //Reset lock state.
        isLocked = false;

        //Add force to platform.
        rigidBody.AddForce(forceDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (changeDirOnCollision)
        {
            //Make sure to avoid players and enemies.
            if (other.gameObject.tag != "Player" && other.gameObject.tag != "Enemy")
            {
                //Passing zero vectors just to make sure that local vars are cleared.
                StartCoroutine(LockMovAndChangeDir(timeBeforeDirChange, Vector3.zero, Vector3.zero, -Vector3.one));
            }
        }
    }
}
