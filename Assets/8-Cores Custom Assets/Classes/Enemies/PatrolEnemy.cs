using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class PatrolEnemy : BaseEnemy
{
    public GameObject player;

    public Light lighting;

    public int wanderArea = 10;

    public bool DEBUG_ACTIVE = true;

    public Animation wanderAnimation;
    public Animation wakeAnimation;
    public Animation chaseAnimation;

    //public Vector3[] patrolPoints; //Add in inspector

    //private int patrolPoint = 0;
    private NavMeshAgent agent;
    //private Animator animator;
    //public Light light;
    private Vector3 startPosition;  //Give it a startPosition so it knows where it's 'home' location is.

    private bool wandering = true;  //Set a bool or state so it knows if it's wandering or chasing a player
    private bool repeat = false;
    private bool wake = false;
    private bool chasing = false;
    private float wanderSpeed = 1.5f;  //Give it the movement speeds
    private float timeLeft = 1f;
    private GameObject target;  //The target you want it to chase

    void Awake()
    {
        //Get the NavMeshAgent so we can send it directions and set start position to the initial location
        agent = GetComponent("NavMeshAgent") as NavMeshAgent;
        agent.speed = wanderSpeed;
        startPosition = this.transform.position;

        //Start Wandering
        InvokeRepeating("Wander", 1f, 5f);
        InvokeRepeating("CheckCollision", 0.1f, 0.1f);
    }
    private void Update()
    {
        if (DEBUG_ACTIVE)
        {
            DrawDebug();
        }

        if (wake)
        {
            Wake();
        }

        if (chasing)
        {
            Chase();
        }

        if (Input.GetKeyDown("k"))
        {
            Destroy(this.gameObject);
        }
    }

    private void Wander()
    {
        //Pick a random location within wander-range of the start position and send the agent there
        if (wandering)
        {
            //animator.Play("New State");
            lighting.color = Color.cyan;
            Vector3 destination = startPosition + new Vector3(UnityEngine.Random.Range(-10, 10),
                                                  0,
                                                  UnityEngine.Random.Range(-10, 10));
            NewDestination(destination);
        }
    }

    private void Wake()
    {
        lighting.color = Color.yellow;
        agent.SetDestination(this.transform.position);
    }

    private void Chase()
    {
            NewDestination(player.transform.position);
            lighting.color = Color.red;
            agent.autoBraking = false;
            agent.speed = 3.5f;
    }

    private void CheckCollision()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < 2.5f)
        {
            //Destroy(this.gameObject);
        }
    }


    public void NewDestination(Vector3 targetPoint)
    {
        //Sets the agents new target destination to the position passed in
        agent.SetDestination(targetPoint);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            wake = true;
            wandering = !wake;
            chasing = !wake;

            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                chasing = true;
                wandering = !chasing;
                wake = !chasing;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            agent.SetDestination(this.transform.position);
            agent.autoBraking = true;
            //agent.speed = wanderSpeed;
            timeLeft = 1f;
            wandering = true;
            wake = !wandering;
            chasing = !wandering;
            Wander();
        }
    }

    private void DrawDebug()
    {
        //Draw square diagonals green
        Debug.DrawLine(startPosition,
    startPosition + ((Vector3.forward + Vector3.right) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.green);

        Debug.DrawLine(startPosition,
            startPosition + ((Vector3.forward + Vector3.left) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.green);

        Debug.DrawLine(startPosition,
            startPosition + ((Vector3.back + Vector3.right) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.green);

        Debug.DrawLine(startPosition,
            startPosition + ((Vector3.back + Vector3.left) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.green);

        //Draw square corners red
        Debug.DrawLine(startPosition + ((Vector3.forward + Vector3.right) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2),
    startPosition + ((Vector3.forward + Vector3.left) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.red);

        Debug.DrawLine(startPosition + ((Vector3.forward + Vector3.left) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2),
    startPosition + ((Vector3.back + Vector3.left) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.red);

        Debug.DrawLine(startPosition + ((Vector3.back + Vector3.left) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2),
    startPosition + ((Vector3.back + Vector3.right) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.red);

        Debug.DrawLine(startPosition + ((Vector3.back + Vector3.right) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2),
    startPosition + ((Vector3.forward + Vector3.right) * ((Convert.ToInt32(Math.Sqrt(Math.Pow(wanderArea, 2) + Math.Pow(wanderArea, 2))))) / 2), Color.red);

    }
}