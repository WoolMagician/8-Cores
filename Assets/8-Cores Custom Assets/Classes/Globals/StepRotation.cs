using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepRotation : MonoBehaviour {

    private bool finishedStep = true;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
        //if (finishedStep)
        //{
        //    finishedStep = false;
        //    StartCoroutine(PerformStep());
        //    Debug.Log("Test");
        //}
        transform.Rotate(0, 0, 1 * 1 * Time.deltaTime * 300);
    }

    IEnumerator PerformStep()
    {
        yield return new WaitForSeconds(0.9f);
        transform.Rotate(0, 0, 1 * 1 * Time.deltaTime * 300);
        finishedStep = true;
    }
    void Start()
    {
        //StartCoroutine(GreenYellowRed());
    }


}
