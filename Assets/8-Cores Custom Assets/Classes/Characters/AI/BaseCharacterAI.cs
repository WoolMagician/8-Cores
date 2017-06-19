using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseCharacterAI : BaseCharacter
{
    public bool isFollowing = false;
    public bool isRunning = false;
    public bool interactWithObjects = false;

    public float distanceFromTarget = 1.0f;
    public BaseCharacter charTarget;
    public BaseEnemy enemyTarget;

    public float randomWanderAreaRange = 10.0f;
    public float minDistanceFromTarget = 1.0f;

    private GameObject[] availableObjects;
    private GameObject interactionObject;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isFollowing)
        {
            //Update distance between AI and character to follow.
            distanceFromTarget = Vector3.Distance(this.transform.position, charTarget.transform.position);

            if(distanceFromTarget > 10) //10 is just and indicative value, needs to be changed.
            {

            }
        }

        //TEMP LOGIC, NEEDS TO BE CHANGED
        if (interactWithObjects)
        {
            availableObjects = UpdateNearObjects(10f);

            interactionObject = SearchForNearestObject(availableObjects);

            InteractWithObject(interactionObject);

        }
        else
        {
            System.Array.Resize(ref availableObjects, 0);

            interactionObject = null;
        }
    }


    private GameObject[] UpdateNearObjects(float distance)
    {
        Collider[] tempObjects;
        List<GameObject> availableObjects = new List<GameObject>();

        tempObjects = Physics.OverlapSphere(this.transform.position, distance);

        for (int i = 0; i < tempObjects.Length; i++)
        {
            if (tempObjects[i] != null)
            {
                availableObjects.Add(tempObjects[i].gameObject);
            }
        }

        System.Array.Resize(ref tempObjects, 0);

        return availableObjects.ToArray();

    }

    private GameObject SearchForNearestObject(GameObject[] objects)
    {
            GameObject oldTarget = null;

            GameObject finalTarget = null;

            if(objects.Length > 0)
            {
                oldTarget = objects[0];

                foreach(GameObject obj in objects)
                {
                    if (obj != null)
                    {
                        finalTarget = obj;

                        if (Vector3.Distance(finalTarget.transform.position, this.transform.position) >= Vector3.Distance(oldTarget.transform.position, this.transform.position))
                        {
                            finalTarget = oldTarget;
                        }
                    }
                }
            }

        oldTarget = null;

        return finalTarget;
    }

    private void InteractWithObject(GameObject obj)
    {
        //obj.GetType()

        //switch ()
        //{
        //    case new GameObject():

        //        break;

        //}
    }


    public enum Test
    {
        test = 0,
    }
}
