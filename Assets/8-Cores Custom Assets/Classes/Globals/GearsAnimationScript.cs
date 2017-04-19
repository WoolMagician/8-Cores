using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearsAnimationScript : MonoBehaviour {
    public GameObject Gear1;
    public GameObject Gear2;
    public GameObject Gear3;
    public GameObject GearX;

    private Vector3 Gear1_Angle;
    private Vector3 Gear2_Angle;
    private Vector3 Gear3_Angle;
    private Vector3 OldGearXAngle;

    //public GameObject Lens1;
    //public GameObject Lens2;
    public GameObject Particle1;
    public GameObject Particle2;
    public GameObject Particle3;

    private bool flag = false;
    private bool step = false;
    private bool refresh = false;
    private int step_number = 1;
	// Use this for initialization
	void Start () {
        Particle1.SetActive(false);
        Particle2.SetActive(false);
        Particle3.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        Particle1.SetActive(flag);
        Particle2.SetActive(flag);
        Particle3.SetActive(step);

        if (Input.GetKeyUp("e"))
        {
            if (!refresh)
            {
                StartCoroutine(Count());
                refresh = true;
            }

        }
        if (flag)
        {


            //            Gear1_Angle = new Vector3(
            //    Mathf.LerpAngle(0, 0, Time.deltaTime),
            //    Mathf.LerpAngle(0, 0, Time.deltaTime),
            //    Mathf.LerpAngle(1 * Time.deltaTime * 250, 1 * Time.deltaTime * 250, 1));

            //            Gear2_Angle = new Vector3(
            //Mathf.LerpAngle(0, 0, Time.deltaTime),
            //Mathf.LerpAngle(0, 0, Time.deltaTime),
            //Mathf.LerpAngle(-1 * Time.deltaTime * 400, -1 * Time.deltaTime * 400, 1));

            //            Gear3_Angle = new Vector3(
            //Mathf.LerpAngle(0, 0, Time.deltaTime),
            //Mathf.LerpAngle(-1 * Time.deltaTime * 500, -1 * Time.deltaTime * 500, Time.deltaTime),
            //Mathf.LerpAngle(0, 0, 1));

            //Gear1.transform.eulerAngles = Gear1_Angle;
            //Gear2.transform.eulerAngles = Gear2_Angle;
            //Gear3.transform.eulerAngles = Gear3_Angle;

            Gear1.transform.Rotate(0, 0, 1 * Time.deltaTime * 250);
            Gear2.transform.Rotate(0, 0, -1 * Time.deltaTime * 400);
            Gear3.transform.Rotate(0, 0, -1 * Time.deltaTime * 500);

            //Gear1.transform.rotation = new Quaternion(Gear1.transform.rotation.x, Gear1.transform.rotation.y, 0, 0);
            //Gear2.transform.rotation = new Quaternion(Gear2.transform.rotation.x, Gear2.transform.rotation.y, 0, 0);
            //Gear3.transform.rotation = new Quaternion(Gear3.transform.rotation.x, Gear3.transform.rotation.y, 0, 0);

        }
        if (step)
        {

            GearX.transform.Rotate(0, 0, 1 * Time.deltaTime * 1000);
        }
        //StartCoroutine(RotateGears());
        //Debug.Log(flag);
    }

    IEnumerator PerformStep()
    {

        yield return new WaitForSeconds(0.7f);
        step = true;
        if (step_number == 3)
        {
            step_number = 0;
        }
        else
        {
            step_number += 1;
        }

        //finishedStep = true;
    }

    IEnumerator Count()
    {
        flag = true;

        StartCoroutine(PerformStep());

        yield return new WaitForSeconds(0.878f);

        flag = false;
        step = false;

        //OldGearXAngle = GearX.

        //Gear1.transform.rotation = Quaternion.identity;
        Gear2.transform.rotation = Quaternion.identity;
        GearX.transform.rotation = Quaternion.identity;
        //Gear3.transform.rotation = Quaternion.identity;


        //Gear1.transform.Rotate(new Vector3(0, 180, 0));
        Gear2.transform.Rotate(new Vector3(0, 180, 90));
        GearX.transform.Rotate(new Vector3(0, 180, (90 * step_number) - 45));

        refresh = false;
        //Gear3.transform.Rotate(new Vector3(0, 180, 0));
    }
}
