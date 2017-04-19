using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class DrawGUI : MonoBehaviour {
    // Use this for initialization
    public GameObject overlay;
        private void Update()
        {
            if (Input.GetButtonDown("PS4_TRIANGLE"))
            {
                overlay.gameObject.SetActive(false);
            }

        }

    }
