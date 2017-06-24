using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_Snap : MonoBehaviour {

    public bool useIK;

    public bool leftHandIK;
    public bool rightHandIK;
    public bool enableDebug;

    public Vector3 leftHandPos;
    public Vector3 rightHandPos;

    private Animator anim;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();

	}

    private void Update()
    {
        if (enableDebug)
        {
            Debug.DrawRay(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(-0.5f, 0.0f, 0.0f), Color.green);
            Debug.DrawRay(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(0.5f, 0.0f, 0.0f), Color.green);
        }

    }

    // Update is called once per frame
    void FixedUpdate () {
        RaycastHit LHit;
        RaycastHit RHit;

        //LeftHandIKCheck
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(-0.5f, 0.0f, 0.0f), out LHit, 1f))
        {

            leftHandPos = LHit.point;
            leftHandIK = true;

        }
        else
        {

            leftHandIK = false;
            leftHandPos = Vector3.zero;

        }

        //RightHandIKCheck
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 2.0f, 0.5f), -transform.up + new Vector3(0.5f, 0.0f, 0.0f), out RHit, 1f))
        {

            rightHandPos = RHit.point;
            rightHandIK = true;

        }
        else
        {

            rightHandIK = false;
            rightHandPos = Vector3.zero;

        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (useIK)
        {
            if (leftHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPos);
            }

            if (rightHandIK)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandPos);
            }
        }
    }
}
