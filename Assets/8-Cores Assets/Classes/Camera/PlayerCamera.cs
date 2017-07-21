using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour {

    public float cameraSpeed = 10.0f;
    public GameObject player;

    private float orbitHorizontal;
    private float verticalShift;

    public float distance = 7.0f;
    public float height = 3.0f;

    public float rotationDamping;   // NON USARE
    public float heightDamping;     // NEANCHE QUESTO

    private void Update()
    {
        // Rileva l'input
        orbitHorizontal = Input.GetAxis("Mouse X") * Time.deltaTime;
        verticalShift = Input.GetAxis("Mouse Y") * Time.deltaTime * -1;

        // Movimento orizzontale
        transform.RotateAround(player.transform.position, Vector3.up, orbitHorizontal * (cameraSpeed * 100));

        // Movimento verticale
        transform.Rotate(Vector3.right, verticalShift * (cameraSpeed * 100));

    }

    void LateUpdate() {

        followPlayer();

    }

    private void followPlayer()
    {
        var wantedRotationAngle = player.transform.eulerAngles.y;
        var wantedHeight = player.transform.position.y + height;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        // Calcola rotazione e altezza
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 1);

        // Trasforma angolo in rotazione
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Calcola le distanze col player
        transform.position = player.transform.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Aggiusta l'altezza
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
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
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayer))
                    return true;
            }

            return false;
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera)
                print("no camera atached to collision");
                return;

            // pulisci intoArray
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.14f) * z;
            float y = x / camera.aspect;

            // calcolo dei punti
            // alto a sinistra
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;    // sommato il punto della camera a una rotazione
            // alto a destra
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;
            // basso a sinistra
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
            // basso a destra
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;

            // posizione della camera
            intoArray[4] = cameraPosition - camera.transform.forward;
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i< desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
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
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
                colliding = true;

            else
                colliding = false;
        }
    }
}

// Cose che mancano:                                -separazione della camera tra combattimento e non combattimento
//                                                  -occlusione
//                                                  -collisioni
//                                                  -limite di visuale su e giu 

https://www.youtube.com/watch?v=7BcxyHi4Jwo