using UnityEngine;
 using System.Collections;

 public class BoomerangStraight : MonoBehaviour
 {
    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    public float timeTakenDuringLerp = 1f;
    public Transform player;
    private bool keyPressed = false;
    private int dir = 1;
    private float progressPercent = 0f;
    /// <summary>
    /// How far the object should move when 'space' is pressed
    /// </summary>
    public float distanceToMove = 0.3f;
    public bool culo = false;

    //Whether we are currently interpolating or not
    private bool _isLerpingFore;
    private bool _isLerpingBack;

    public LayerMask layerMask;

    //The start and finish positions for the interpolation
    private Vector3 _startPosition;
    private Vector3 _endPosition;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;

    public float barDisplay; //current progress
    public Vector2 pos = new Vector2(20, 40);
    public Vector2 size = new Vector2(60, 20);
    public Texture2D emptyTex;
    public Texture2D fullTex;

    public Texture[] gifPicArray;

    private int counter = 0;
    /// <summary>
    /// Called to begin the linear interpolation
    /// </summary>
    void StartLerping(Vector3 vector)
    {
        _isLerpingFore = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPosition = transform.position;
        _endPosition = vector;
        //distanceToMove = 4;
    }

    void StartLerpingBack()
    {
        _isLerpingBack = true;
        _timeStartedLerping = Time.time;

        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPosition = transform.position;
        _endPosition = player.transform.position + player.transform.forward * 0.4f;
        distanceToMove = 0.3f;

    }
    void Start()
    {
        transform.position = player.transform.position + player.transform.forward * distanceToMove;
        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z );

        //transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void Update()
    {
        //When the user hits the spacebar, we start lerping

        //Debug.Log(barDisplay);
        //transform.LookAt(player.position);
        if (!_isLerpingFore && !_isLerpingBack)
        {
            transform.position = player.transform.position + player.transform.forward * distanceToMove;
            transform.rotation = 
            transform.rotation = Quaternion.LookRotation(transform.position - player.position);
            transform.Rotate(0, 0, 0);
        }

        transform.position = new Vector3(transform.position.x, 0.8f, transform.position.z );

        //transform.position = player.transform.position + Camera.main.transform.forward * 1;
        //if (distanceToMove < 4)
        //{
        //    transform.rotation = Quaternion.LookRotation(transform.position - player.position);
        //}


        //transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles);
        //transform.forward = new Vector3(player.transform.forward.x, 0f, player.transform.forward.z);

        if (Input.GetKeyDown("r") && !_isLerpingFore && !_isLerpingBack)
        {

            progressPercent = 1.5f;
            //StartLerping();

        }

        if (Input.GetKeyUp("r") && !_isLerpingFore && !_isLerpingBack)
        {
            keyPressed = false;
            progressPercent = 0f;
            distanceToMove = barDisplay * 6;

            Vector3 test = new Vector3(0, 0, 0);
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
                Ray ray = new Ray(transform.position, fwd);
                if (Physics.Raycast(ray, out hit, distanceToMove))
                {
                    Vector3 incomingVec = hit.point - transform.position;
                    Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);


                    Debug.DrawLine(transform.position, hit.point, Color.red);
                    Debug.DrawRay(hit.point, reflectVec, Color.green);

                    Debug.Log("Test");
                
                }

            Debug.Log(hit.point);

            if (hit.point == Vector3.zero)
            {
                StartLerping(transform.position + player.transform.forward * distanceToMove);

            }
            else
            {
                StartLerping(hit.point);
            }


            barDisplay = 0f;
        }

        barDisplay += Time.deltaTime * progressPercent * dir;
        if (barDisplay > 1.0f)
        {
            dir = -1;

        }
        if (barDisplay < 0f)
        {
            dir = 1;

        }

        if (counter >= gifPicArray.Length)
        {
            counter = 0; //ninja edit
        }
    }

    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {


        if (_isLerpingFore)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                _isLerpingFore = false;
                StartLerpingBack();
            }

        }
        if (_isLerpingBack && !_isLerpingFore)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);

            //When we've completed the lerp, we set _isLerping to false
            if (percentageComplete >= 1.0f)
            {
                _isLerpingBack  = false;
            }

        }
    }
    void OnGUI()
    {

        //draw the background:
        //gifPicArray[counter++])
        GUI.BeginGroup(new Rect(100, 400, 200, 20));
        GUI.Box(new Rect(0, 0, size.x, size.y), emptyTex);

        //draw the filled-in part:
        GUI.BeginGroup(new Rect(0, 0, size.x * barDisplay, size.y));
        GUI.Box(new Rect(0, 0, 200, 20), fullTex);
        GUI.EndGroup();
        GUI.EndGroup();



    }

}
