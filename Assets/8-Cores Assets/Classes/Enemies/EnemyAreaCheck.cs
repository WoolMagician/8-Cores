using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaCheck : MonoBehaviour {
    public GameObject enemy;
    public GameObject player;
    public GameObject overHeadLockObj;
    private GameObject overHeadLock;
    private Animator anim;
    private bool lookAtFlag = false;
    private bool flag = false;

    //private void Update()
    //{
    //    //if (Vector3.Distance(player.transform.position, enemy.transform.position) > 10)
    //    //{
    //    //    flag = false;
    //    //    lookAtFlag = false;
    //    //}
    //    //else
    //    //{
    //    //    //
    //    //}
    //}
    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {

        //player.transform.rotation = Quaternion.Euler(0, player.transform.rotation.y, player.transform.rotation.z);
        //if (Vector3.Distance(player.transform.position, enemy.transform.position) > 10)
        //{
        //    flag = false;
        //    lookAtFlag = false;
        //}

        if (lookAtFlag)
        {
            //Debug.DrawLine(player.transform.position, enemy.transform.position);

            player.transform.LookAt(enemy.transform);

            //Debug.Log(Vector3.Distance(player.transform.position, enemy.transform.position));

            if (Input.GetKey("d"))

            {
                anim.Play("Unarmed-Strafe-Right");
                player.transform.Translate(Vector3.right * 2.5f * Time.deltaTime);
            }

            if (Input.GetKey("a"))
            {

                player.transform.Translate(Vector3.left * 2.5f * Time.deltaTime);
                anim.Play("Unarmed-Strafe-Left");
            }

            if(Input.GetKey("s"))
            {
                anim.Play("Unarmed-Strafe-Backward");
                player.transform.Translate(Vector3.back * 2.5f * Time.deltaTime);
            }

            //if (Input.GetKeyDown("q") && flag == true)
            //{
            //    flag = false;
            //}
        }



        //Debug.Log(lookAtFlag);
    }

    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown("q") && other.gameObject.tag == "Enemy")
        {
            flag = !flag;
        }

        lookAtFlag = flag;
    } 
    private void OnTriggerExit(Collider other)
    {
        flag = false;
        lookAtFlag = false;
    }
}
