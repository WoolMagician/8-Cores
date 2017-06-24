using UnityEngine;


using System.Collections;

public class CameraController : MonoBehaviour
{
    private GameSettings input;

    [Header("Camera Properties")]
    public float DistanceAway;                     //how far the camera is from the player.
    public float DistanceUp;                    //how high the camera is above the player
    public float smooth = 4.0f;                    //how smooth the camera moves into place
    public float rotateAround = 70f;            //the angle at which you will rotate the camera (on an axis)

    [Header("Player to follow")]
    public Transform target;                    //the target the camera follows

    [Header("Layer(s) to include")]
    public LayerMask CamOcclusion;                //the layers that will be affected by collision
    RaycastHit hit;
    float cameraHeight = 55f;
    float cameraPan = 0f;
    float camRotateSpeed = 180f;
    Vector3 camPosition;
    Vector3 camMask;
    Vector3 followMask;

    Inventory inv;
    Vector3 StartPos; //Camera position at start
    Vector3 targetOffset;
    // Use this for initialization
    void Start()
    {
        //input = FindObjectOfType<GameSettings>().GetComponent<GameSettings>();
        //the statement below automatically positions the camera behind the target.
        rotateAround = target.eulerAngles.y - 45f;

        StartPos = transform.localPosition;
    }
    void Update()
    {
        //CameraCall();
    }
    // Update is called once per frame

    void LateUpdate()
    {
        //Offset of the targets transform (Since the pivot point is usually at the feet).

        Quaternion rotation = Quaternion.Euler(cameraHeight, rotateAround, cameraPan);
        Vector3 vectorMask = Vector3.one;
        Vector3 rotateVector = rotation * vectorMask;
        //this determines where both the camera and it's mask will be.
        //the camMask is for forcing the camera to push away from walls.
        camPosition = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
        camMask = targetOffset + Vector3.up * DistanceUp - rotateVector * DistanceAway;
        smooth = 20;
        occludeRay(ref targetOffset);
        smooth = 4;
        smoothCamMethod();

        transform.LookAt(target);

        #region wrap the cam orbit rotation
        if (rotateAround > 360)
        {
            rotateAround = 0f;
        }
        else if (rotateAround < 0f)
        {
            rotateAround = (rotateAround + 360f);
        }
        #endregion

        rotateAround += Input.GetAxis("PS4_PAD_RIGHT_X") * -70 * camRotateSpeed * Time.deltaTime;
        DistanceUp = Mathf.Clamp(DistanceUp += Input.GetAxis("PS4_PAD_RIGHT_Y") * -70, -2.50f, 5f);
    }
    private void FixedUpdate()
    {
        targetOffset = new Vector3(target.position.x, (target.position.y + 2f), target.position.z);
        //occludeRay(ref targetOffset);
        CameraCall();
    }
    void smoothCamMethod()
    {
        //smooth = 4f;
        transform.position = Vector3.Lerp(transform.position, camPosition, Time.deltaTime * smooth);
    }
    void occludeRay(ref Vector3 targetFollow)
    {
        #region prevent wall clipping
        //declare a new raycast hit.
        RaycastHit wallHit = new RaycastHit();

        Debug.DrawLine(targetFollow, transform.position);
        //linecast from your player (targetFollow) to your cameras mask (camMask) to find collisions.

        float val = 3.41f;
        float z = Camera.main.nearClipPlane;
        float x = Mathf.Tan(Camera.main.fieldOfView / val) * z;
        float y = x / Camera.main.aspect;
        Vector3 cameraPosition = Camera.main.transform.position;
        Quaternion atRotation = Camera.main.transform.rotation;

        Vector3 pos1;
        Vector3 pos2;
        Vector3 pos3;
        Vector3 pos4;

        //top left
        pos1 = (atRotation * new Vector3(-x, y, z)) + cameraPosition; //added and rotated the point relative to camera

        //top right
        pos2 = (atRotation * new Vector3(x, y, z)) + cameraPosition; //added and rotated the point relative to camera

        //bottom left
        pos3 = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; //added and rotated the point relative to camera

        //bottom right
        pos4 = (atRotation * new Vector3(x, -y, z)) + cameraPosition; //added and rotated the point relative to camera


        //if (Physics.Linecast(targetFollow, pos1, out wallHit, CamOcclusion))
        //{
        //    //the smooth is increased so you detect geometry collisions faster.
        //    smooth = 10f;
        //    //the x and z coordinates are pushed away from the wall by hit.normal.
        //    //the y coordinate stays the same.
        //    camPosition = new Vector3(camPosition.x, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.01f * -Vector3.forward.z);
        //}

        //if (Physics.Linecast(targetFollow, pos2, out wallHit, CamOcclusion))
        //{
        //    //the smooth is increased so you detect geometry collisions faster.
        //    smooth = 10f;
        //    //the x and z coordinates are pushed away from the wall by hit.normal.
        //    //the y coordinate stays the same.
        //    camPosition = new Vector3(camPosition.x, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.01f * -Vector3.forward.z);
        //}

        //if (Physics.Linecast(targetFollow, pos3, out wallHit, CamOcclusion))
        //{
        //    //the smooth is increased so you detect geometry collisions faster.
        //    smooth = 10f;
        //    //the x and z coordinates are pushed away from the wall by hit.normal.
        //    //the y coordinate stays the same.
        //    camPosition = new Vector3(camPosition.x, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.01f * -Vector3.forward.z);
        //}

        //if (Physics.Linecast(targetFollow, pos4, out wallHit, CamOcclusion))
        //{
        //    //the smooth is increased so you detect geometry collisions faster.
        //    smooth = 10f;
        //    //the x and z coordinates are pushed away from the wall by hit.normal.
        //    //the y coordinate stays the same.
        //    camPosition = new Vector3(camPosition.x, camPosition.y, wallHit.point.z + wallHit.normal.z * 0.01f * -Vector3.forward.z);
        //}

        //Debug.DrawLine(targetFollow, pos1);

        //Debug.DrawLine(targetFollow, pos2);

        //Debug.DrawLine(targetFollow, pos3);

        //Debug.DrawLine(targetFollow, pos4);
        #endregion
    }

    void CameraCall() //The function handling camera movement back into position, handles the raycast as well
    {
        RaycastHit RayHit; //The name within this script of our raycast
        Debug.DrawRay(transform.position, -transform.forward, Color.red, .001f); //draws the raycast in the scene so we can make sure its working properly
        Debug.DrawRay(transform.position, transform.right, Color.red, .001f);
        Debug.DrawRay(transform.position, -transform.right, Color.red, .001f);
        if ((Physics.Raycast(transform.position, -transform.forward, out RayHit, 10f)) ||
            (Physics.Raycast(transform.position, transform.right, out RayHit, 10f )) ||
            (Physics.Raycast(transform.position, -transform.right, out RayHit, 10f))) //sends out raycast and returns bool value
        {
            if (RayHit.distance < 0f) //Default value .25f for this check, and .1f for the movement, .5f also works well
            {
                //transform.localPosition += new Vector3(0, 0, .05f); //SpeedMult * Time.deltaTime); //moves the camera closer to the player on collision with wall
                //transform.localPosition += new Vector3(0, 0, .1f);
            }
            else if (RayHit.distance > .1f) //Default value .35f, and .1f for the movement .8f works well
            {
                transform.localPosition -= new Vector3(0, 0, .05f); //SpeedMult * Time.deltaTime); //Moves the camera back into position// .05f works very well
            }
        }
        if (Physics.Raycast(transform.position, transform.up, out RayHit, 10f, 3, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
        {
            if (RayHit.distance < -5f)
            {
                transform.localPosition -= new Vector3(0, .005f, 0); //SpeedMult * Time.deltaTime); //moves the camera closer to the player on collision with wall
            }
            //else if (RayHit.distance > .65f) //Checks to make sure that the camera doesn't go further than original position, also checks for raycast distance from wall
            //{
            //    transform.localPosition += new Vector3(0, .005f, 0); //SpeedMult * Time.deltaTime); //Moves the camera back into position
            //}
        }
    }
}

//public class CollisionHandler
//{
//    RaycastHit topRightHit = new RaycastHit();
//    RaycastHit topLeftHit = new RaycastHit();
//    RaycastHit bottomRightHit = new RaycastHit();
//    RaycastHit bottomLeftHit = new RaycastHit();

//    LayerMask collisionLayer;

//    CameraClipPointArray clipPoints;

//    public bool isColliding = false;

//    public void Initialize(LayerMask collisionLayer)
//    {
//        this.collisionLayer = collisionLayer;
//    }

//    public bool CheckColliding(Vector3 fromPosition)
//    {
//        for (int i = 0; i < clipPoints.CCPoints.Length; i++)
//        {
//            Ray ray = new Ray(fromPosition, clipPoints.CCPoints[i] - fromPosition);

//            float distance = Vector3.Distance(clipPoints.CCPoints[i], fromPosition);

//            if (Physics.Raycast(ray, distance, collisionLayer))
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//}

//public class CameraClipPointArray
//{
//    public Vector3[] CCPoints;
    
//}

