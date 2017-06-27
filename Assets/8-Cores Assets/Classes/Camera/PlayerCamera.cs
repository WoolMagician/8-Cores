using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour {

	public float cameraSpeed = 10.0f;
    public float cameraHeight = 3.07f;
	public GameObject player;

    private float orbitHorizontal;
    private float verticalShift;

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

    void LateUpdate () {
        FollowPlayer();
	}

    private void FollowPlayer()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 8.0f; //Gli 8 sono la velocità dello spostamento del personaggio. Giuro che lo avrei scritto meglio ma sono 10 volte che lo scrivo
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 8.0f; // e mi sono anche un po' cagato il cazzo. Comunque sarebbe carino implementalo in una funzione in comune che ritorna il movimento
                                                                   // prima o poi lo farò. 
        transform.Translate(x, 0, z);
        transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z); // Resetta l'altezza a posizione normale
    }
}

// Cose che mancano:                                -separazione della camera tra combattimento e non combattimento
//                                                  -occlusione
//                                                  -collisioni
//                                                  -casi particolari (arrampicata ecc ecc)