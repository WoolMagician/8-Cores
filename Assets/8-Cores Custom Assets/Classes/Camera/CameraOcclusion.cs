using UnityEngine;

public class CameraOcclusion : MonoBehaviour
{
    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        //public Vector3 targetPosOffset = new Vector3(0.5f, 1.4f, 0);
        public Vector3 targetPosOffset = new Vector3(0, 2f, 0);
        public float distanceFromTarget = -2.7f;
        public float zoomSmooth = 100;
        public float zoomStep = 2;
        public float maxZoom = -2;
        public float minZoom = -15;
        public bool smoothFollow = true;
        public float smooth = 100f;
        
        [HideInInspector]
        public float newDistance = -2; //set by zoom input
        
        [HideInInspector]
        public float adjustmentDistance = -2;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 25;
        public float minXRotation = -50;
        public float vOrbitSmooth = 0.5f;
        public float hOrbitSmooth = 0.5f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string MOUSE_ORBIT = "MouseOrbit";
        public string MOUSE_ORBIT_VERTICAL = "MouseOrbitVertical";
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL = "PS4_PAD_RIGHT_X";
        public string ORBIT_VERTICAL = "PS4_PAD_RIGHT_Y";
        public string ZOOM = "Mouse ScrollWheel";
        public bool INVERT_CAMERA_X = false;
        public bool INVERT_CAMERA_Y = false;
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLine = true;

    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision = new CollisionHandler();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 camVel = Vector3.zero;
    //CharacterController charController;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, mouseOrbitInput, vMouseOrbitInput;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;

    private void Start()
    {
        SetCameraTarget(target);

        vOrbitInput = hOrbitInput = zoomInput = hOrbitSnapInput = mouseOrbitInput = vMouseOrbitInput = 0;

        MoveToTarget();

        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoint);

        previousMousePos = currentMousePos = Input.mousePosition;
    }

    void SetCameraTarget(Transform t)
    {
        target = t;

        if (target == null)
        {
            Debug.LogError("Your camera needs a target");
        }
        
    }

    void GetInput()
    {
        if (input.INVERT_CAMERA_X)
        {
            hOrbitInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL) * -1;
        }
        else
        {
            hOrbitInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
        }

        if (input.INVERT_CAMERA_Y)
        {
            vOrbitInput = (Input.GetAxisRaw(input.ORBIT_VERTICAL)) * -1;
        }
        else
        {
            vOrbitInput = (Input.GetAxisRaw(input.ORBIT_VERTICAL));
        }

        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
        mouseOrbitInput = Input.GetAxisRaw(input.MOUSE_ORBIT);
        vMouseOrbitInput = Input.GetAxisRaw(input.MOUSE_ORBIT_VERTICAL);
    }

    private void Update()
    {
        GetInput();
        //ZoomInOnTarget();
    }

    private void FixedUpdate()
    {
        //moving
        MoveToTarget();

        //rotating
        LookAtTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoint);

        //draw debug lines
        for (int i = 0; i< 5; i++)
        {
            if (debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoint[i], Color.white);
            }

            if (debug.drawAdjustedCollisionLine)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(targetPos); //using raycasts here
        //position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
    }

    void MoveToTarget()
    {
        targetPos = target.position + Vector3.up * position.targetPosOffset.y + Vector3.forward * position.targetPosOffset.z + transform.TransformDirection(Vector3.right * position.targetPosOffset.x);
        
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        //destination += targetPos;

        if (collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;

            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, position.smooth);
            }
            else
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, adjustedDestination.x, 1f * Time.deltaTime), Mathf.Lerp(transform.position.y, adjustedDestination.y, 1f * Time.deltaTime), Mathf.Lerp(transform.position.z, adjustedDestination.z , 1f * Time.deltaTime)); ;
        }
        else
        {
            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, position.smooth);

            }
            else
                transform.position = destination;
        }
    }

    void LookAtTarget()
    {
        //Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 100 * Time.deltaTime);
        transform.LookAt(target);
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;

        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;

        [HideInInspector]
        public Vector3[] desiredCameraClipPoint;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoint = new Vector3[5];

        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera)
                return;

            //clear the contents of intoArray
            intoArray = new Vector3[5];

            float val = 3.41f;
            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / val) * z;
            float y = x / camera.aspect;

            //top left
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; //added and rotated the point relative to camera

            //top right
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition; //added and rotated the point relative to camera

            //bottom left
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; //added and rotated the point relative to camera

            //bottom right
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition; //added and rotated the point relative to camera

            //camera's position
            intoArray[4] = cameraPosition - camera.transform.forward;
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);

                float distance = Vector3.Distance(clipPoints[i], fromPosition);

                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }
            return false;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoint.Length; i++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoint[i] - from);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1)
                        distance = hit.distance;
                    else
                    {
                        if (hit.distance < distance)
                            distance = hit.distance;
                    }
                }
            }

            if (distance == -1)
                return 0;
            else
                return distance;
        }

        public void CheckColliding(Vector3 targetPosition)
        {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoint, targetPosition))
            {
                colliding = true;
            }
            else
                colliding = false;
        }
    }

}