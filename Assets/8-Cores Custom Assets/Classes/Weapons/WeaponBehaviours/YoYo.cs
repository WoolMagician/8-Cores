using UnityEngine;
using System.Collections;

[System.Serializable]
public class YoYo : MonoBehaviour {
	public Transform playerObj;
    public Transform handTransform;
    public GameObject enemy ;


    public string yoyoThrowButton = "PS4_SQUARE";
    public string yoyoThrowKey = "r";

    //public float throwSpeed = 1f;
	private float throwTime = 0f;

    //public LayerMask hitCheckLayers;

	private Vector3 beginPoint = new Vector3(0f, 0f, 0f);
	private Vector3 finalPoint = new Vector3(0f, 0f, 30f);
	private Vector3 centerPoint = new Vector3(0f, 0f, 0f);
    public Vector3 offset = Vector3.zero;
    private Vector3 origPos;

    private RaycastHit hit;

    private bool collided = false;

    private bool playerHasYoyo = true;

    public GameObject effects;

    public float posX = 0f;
    public float posZ = 0f;

    public Vector3 test;

    [HideInInspector]
    public Rigidbody rigidBody;

    [HideInInspector]
    public MeshCollider meshCollider;

    [HideInInspector]
    public TrailRenderer trailRenderer;

    void Start ()
    {
		origPos = transform.position;

        trailRenderer = this.GetComponent<TrailRenderer>();

        rigidBody = this.GetComponent<Rigidbody>();

        meshCollider = this.GetComponent<MeshCollider>();

        trailRenderer.enabled = false;

        offset = new Vector3(-0.03f, 0f, 0.06f);

        enemy  = new GameObject();

        ResetYoyoState();

        Charging(false);

    }

    private void FixedUpdate()
    {
        enemy = playerObj.gameObject.GetComponent<RPGCharacterControllerFREE>().currentTarget;

        if (enemy != null)
        {
            beginPoint = enemy.transform.position;
        }
        else if (playerHasYoyo)
        {

            beginPoint = new Vector3(playerObj.position.x, this.transform.position.y, playerObj.position.z) + playerObj.forward * 5f;

        }

        //new Vector3(posX + 0.15f, transform.position.y, posZ) + (Vector3.forward * 5f);//enemy.position;

        finalPoint = beginPoint;
        finalPoint.z = -finalPoint.z;

        centerPoint = Vector3.Lerp(beginPoint, finalPoint, 0.1f);

        meshCollider.enabled = !playerHasYoyo;

    }

    void Update()
    {
        //if (Vector3.Distance(transform.position, playerObj.position) >= 10f && !playerHasYoyo)
        //{
        //    //ResetYoyoState();
        //}

        //Check if player has yoyo and if it is thrown.
        if (playerHasYoyo && Input.GetButtonDown(yoyoThrowButton))
        {
            Charging(true);

            trailRenderer.enabled = true;
        }

        //Check if player has yoyo and if it is thrown.
        if (playerHasYoyo && Input.GetButtonUp(yoyoThrowButton))
        {
            posX = playerObj.position.x;

            posZ = playerObj.position.z;

            Charging(false);

            SetYoyoState(false); //Throw yoyo.

            transform.rotation = Quaternion.Euler(0, 0, 90); //Used to fix Z rotation.
        }

        if (playerHasYoyo)
        {
            if (throwTime == 0)
            {
                //Keep yoyo into player's hand.
                transform.position = handTransform.position + offset;
                transform.rotation = handTransform.rotation;
            }

            return;
        }

        //Increase throwTime by deltaTime
        throwTime += Time.deltaTime;

		if (throwTime > 0f && throwTime < 1f)
        {
            //Move yoyo at desired position
			transform.position = Vector3.Lerp(transform.position, beginPoint, throwTime);
		}

        else if (throwTime >= 1f && throwTime <= 2f)
        { 
            //Random parabolic before return
			if (beginPoint.z > 0f)
            {
                transform.RotateAround(centerPoint, Vector3.left, 10 * Time.deltaTime);
			}

            else
            {
				transform.RotateAround(centerPoint, Vector3.right, 10 * Time.deltaTime);
			}
        
		}
        else if (throwTime > 2f && throwTime < 3f)
        {
            //Get yoyo back in hand smoothly with lerp function.
			transform.position = Vector3.Lerp(transform.position, handTransform.position + offset, throwTime - 2f);
		}

		if (throwTime >= 2.4f)
        {
            trailRenderer.enabled = false;

            if (!playerHasYoyo)
            {
                //Reset yoyo state.
                ResetYoyoState();
            }

		}
        else
        {
            //Rotate yoyo 'till it returns to player's hand.
            transform.Rotate(Vector3.up, throwTime * 50f);
        }
	}

    public void SetYoyoState(bool isYoyoInHand)
    {
        playerHasYoyo = isYoyoInHand;
    }

    public void ResetYoyoState()
    {
        playerHasYoyo = true;

        transform.rotation = handTransform.rotation;

		collided = false;

        throwTime = 0f;

        rigidBody.useGravity = false;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
	}

    public void Charging(bool isCharging)
    {
        if (isCharging)
        {
            effects.SetActive(true);
        }
        else
        {
            effects.SetActive(false);
        }
    }

    //Used to check if yoyo hits something.
	void OnTriggerEnter(Collider other)
    {
        if (!playerHasYoyo)
        {
            //Check if hitted gameobject is an Enemy.
            if (other.transform.tag == "Enemy")
            {
                //Change CapsuleCollider with body collider of enemy.
                if (other.GetType() == typeof(CapsuleCollider))
                {
                    Destroy(other.transform.parent.gameObject);

                    //Return yoyo into player's hand.
                    throwTime = 2f;
                }
            }
        }
    }
}
