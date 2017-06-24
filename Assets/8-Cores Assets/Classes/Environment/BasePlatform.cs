/*
 * - CLASS: BASE PLATFORM CLASS
 * - AUTHOR: FRANCESCO PODDA
 * - VERSION: 1.1
 * - CREATION DATE: MONDAY, 24 APRIL 2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class used for platforms behaviour.
/// </summary>
[System.Serializable]
public class BasePlatform : MonoBehaviour {

    //If true enables direction change on collision after wait time.
    public bool changeDirOnCollision = true;

    //Time that platform waits after colliding.
    [Range(0, 10)]
    public int timeBeforeDirChange = 2;

    //Used to lock movement while waiting.
    [HideInInspector]
    public bool isLocked = false;

    //Speed multipliers.
    [Range(0, 1000)]
    public float xSpeedMultiplier = 100f;

    [Range(0, 1000)]
    public float ySpeedMultiplier = 0f;

    [Range(0, 1000)]
    public float zSpeedMultiplier = 0f;

    //Used to store current platform speed.
    private float xSpeed = 0f;
    private float ySpeed = 0f;
    private float zSpeed = 0f;

    //Used to store current platform direction.
    private float xDirection = 1f;
    private float yDirection = 1f;
    private float zDirection = 1f;

    //Used to store current platform position.
    private float xPosition = 0f;
    private float yPosition = 0f;
    private float zPosition = 0f;

    public int direction = 1;

	// Update is called once per frame.
	void Update ()
    {
        //Check if platform is locked before moving.
        if (!isLocked)
        {
            MovePlatform();
        }
        else
        {
            xPosition = 0;
            yPosition = 0;
            zPosition = 0;
        }
    }

    private void MovePlatform()
    {
        xPosition = Mathf.SmoothDamp(transform.position.x, transform.position.x, ref xSpeed, (1000 - xSpeedMultiplier) * Time.fixedDeltaTime);
        yPosition = Mathf.SmoothDamp(transform.position.y, transform.position.y, ref ySpeed, (1000 - ySpeedMultiplier) * Time.deltaTime);
        zPosition = Mathf.SmoothDamp(transform.position.z, transform.position.z + direction, ref zSpeed, (1000 - zSpeedMultiplier) * Time.deltaTime);

        transform.position = new Vector3(xPosition * xDirection, yPosition * yDirection, zPosition);
    }

    /// <summary>
    /// This function is used to wait and change direction of platform.
    /// </summary>
    /// <param name="lockSeconds">Time (s) platform waits after direction change.</param>
    /// <param name="velocity">Velocity assigned to platform after <code>lockSeconds</code>.</param>
    /// <param name="newDirection">New direction, usually is equal to <code>-Vector3.one</code>.</param>
    /// <returns></returns>
    IEnumerator LockMovAndChangeDir(int lockSeconds, Vector3 velocity, Vector3 newDirection)
    {
        Vector3 oldpos = Vector3.zero;

        oldpos = transform.position;
        //Change directions.


        //Update velocity.
        //velocity = new Vector3(xSpeed * xDirection, ySpeed * yDirection, zSpeed * zDirection);

        //Set lock state.


        direction *= -1;

        isLocked = true;

        yield return new WaitForSeconds(lockSeconds);

        //xDirection *= newDirection.x;
        //yDirection *= newDirection.y;
        //zDirection *= newDirection.z;

        //Reset lock state.
        isLocked = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (changeDirOnCollision)
        {
            //Make sure to avoid players and enemies.
            if (collision.collider.tag != "Player" && collision.collider.tag != "Enemy")
            {
                //Passing zero vectors just to make sure that local vars are cleared.
                StartCoroutine(LockMovAndChangeDir(timeBeforeDirChange, Vector3.zero, -Vector3.one));
            }
        }
    }
}

