using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class SwapWeaponManager : MonoBehaviour
{
    [HideInInspector]
    public BaseWeapon currentSelectedWeapon = null;

    [HideInInspector]
    public bool isSwapping = false;

    [HideInInspector]
    public CameraController camController;

    [HideInInspector]
    public GameObject player;

    public BaseWeapon[] weaponsHolder;

    public GameObject swapEffect;

    public GameObject[] swapEffectIstanceHolder;

    public Material wireframeMat = null;

    private Material weaponMat = null;

    private int currentSelectedWeaponID = 0;

    private List<BaseWeapon> weaponList = new List<BaseWeapon>();

    // Use this for initialization
    void Start()
    {
        weaponList.AddRange(weaponsHolder);

        camController = Camera.main.GetComponent<CameraController>();

        player = camController.target.gameObject;

        if (weaponList.Count > 0)
        {
            currentSelectedWeapon = weaponList[currentSelectedWeaponID];
            weaponMat = currentSelectedWeapon.gameObject.GetComponent<Renderer>().material;
            swapEffect.SetActive(false);

            swapEffectIstanceHolder = new GameObject[weaponList.Count];

            for (int i = 0; i < weaponList.Count; i++)
            {
                swapEffect.transform.position = Vector3.zero;
                swapEffectIstanceHolder[i] = Instantiate(swapEffect, weaponList[i].gameObject.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            isSwapping = true;

            Time.timeScale = 0.25F;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;

            currentSelectedWeapon.gameObject.GetComponent<Renderer>().material = wireframeMat;

            camController.gameObject.GetComponent<VignetteAndChromaticAberration>().enabled = true;

            //THIS CAN BE SMOOTHED OUT
            //camController.target = currentSelectedWeapon.gameObject.transform;

            if (weaponList.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if(currentSelectedWeapon != null)
                    {
                        currentSelectedWeapon.gameObject.GetComponent<Renderer>().material = weaponMat;
                        swapEffectIstanceHolder[currentSelectedWeaponID].SetActive(false);
                    }

                    if (currentSelectedWeaponID == (weaponList.Count - 1))
                    {
                        currentSelectedWeaponID = 0;
                    }
                    else
                    {
                        currentSelectedWeaponID += 1;
                    }

                    currentSelectedWeapon = weaponList[currentSelectedWeaponID];

                    weaponMat = currentSelectedWeapon.gameObject.GetComponent<Renderer>().material;

                    currentSelectedWeapon.gameObject.GetComponent<Renderer>().material = wireframeMat;

                }
            }
        }

        else
        {
            if (currentSelectedWeapon != null)
            {
                currentSelectedWeapon.gameObject.GetComponent<Renderer>().material = weaponMat;
            }

            if (isSwapping)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02F;
                camController.target = player.transform;
                swapEffectIstanceHolder[currentSelectedWeaponID].SetActive(true);
                camController.gameObject.GetComponent<VignetteAndChromaticAberration>().enabled = false;
            }

            isSwapping = false;

        }
    }
}
