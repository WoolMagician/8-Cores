using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	// Animator
	public Animator anim;

    public GameObject camera;

	// To be exchanged with values from the character
	public float forwardSpeed = 8.0f;
	public float lateralSpeed = 4.0f;
	public float backwardSpeed = 2.0f;

	void Start()
	{
		anim = GetComponent<Animator> (); //prima o poi...
	}

	void Update()
	{
        Move();
	}

    public void Move()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * forwardSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * forwardSpeed;

        if (camera.transform.rotation.eulerAngles.y > 0 && (x != 0 || z !=0))  // So che è un po' accroccato ma è decente. È il codice che ruota il pg se la camera ruota.
        {                                                                      
            if((camera.transform.rotation.eulerAngles.y != transform.rotation.eulerAngles.y))
            {
                transform.Rotate(Vector3.up, (camera.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y)); // Dividendo la sottrazione per un numero (basso se si vuole un movimento rapido, alto se più fulmineo) il pg fa una piacevole rotazione. Il problema è che la camera si perde dopo un po' per ovvi motivi.
            }                                                                                                              
        }

        transform.Translate(x, 0, z);
    }

}


// Cose che mancano:                                -separazione del movimento tra lock e non-lock
//                                                  -movimento col lock
//                                                  -salto
//                                                  -controllo analogico/camminata-corsa

    //                  QUESTO È QUELLO BUONO, NON PLAYER CONTROL CHE È SCRITTO DA PODDAMMERDA