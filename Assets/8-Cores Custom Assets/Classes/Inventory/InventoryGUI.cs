using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour {

    [HideInInspector]
    public Inventory inventory;

    public GameObject blurredOverlay;
    public Material blurredOverlayMaterial;

    public Texture emptyTex;
    int iconWidthHeight = 100;
    Rect rect = new Rect(0, 0, Screen.width, Screen.height);

    public GameObject characterWeapon;

    [HideInInspector]
    public GameObject inventoryWeapon;

    public Vector3 screenCenter;

    private void Start()
    {
        inventory = this.GetComponent<Inventory>();
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 3f));
    }

    // Create the an 8x8 Texture-Array 
    public void Awake()
    {
        Renderer blurredOverlayRenderer;

        blurredOverlay = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Plane);
        blurredOverlay = Instantiate(blurredOverlay, screenCenter + new Vector3(0,0,1f), Quaternion.identity);
        blurredOverlay.transform.localScale = new Vector3(2,0,2);
        blurredOverlay.transform.LookAt(Camera.main.transform);

        blurredOverlayRenderer = blurredOverlay.GetComponent<Renderer>();

        blurredOverlayRenderer.material = blurredOverlayMaterial;

        ////emptyTex = new Texture2D(Screen.width, Screen.height);
        //inventory = new Texture[5][];

        //for (int i = 0; i < inventory.Length; i++)
        //{
        //    inventory[i] = new Texture[6];
        //}
    }

    private void DrawWeapon()
    {
        

        inventoryWeapon = Instantiate(characterWeapon, screenCenter, Quaternion.identity);
    }

    public void OnGUI()
    {
        if (inventory.isOpened)
        {
            //GUI.Box(rect, "This is a box");
            

            if (inventoryWeapon == null)
            {
                DrawWeapon();
            }

        }
        else
        {
            ;

            if (inventoryWeapon != null)
            {
                Destroy(inventoryWeapon);
            }

        }

        //Texture texToUse;

        //int texto = 0;

        //if (inv.inventoryMatrix.Count > 0)
        //{
        //  texto = inv.inventoryMatrix[0].currentSlotValue;
        //}


        ////Go through each row 
        //for (int i = 0; i < inventory.Length; i++)
        //{
        //    // and each column 
        //    for (int k = 0; k < inventory[i].Length; k++)
        //    {
        //        texToUse = emptyTex;

        //        //if there is a texture in the i-th row and the k-th column, draw it 
        //        if (inventory[i][k] != null)
        //        {
        //            texToUse = inventory[i][k];
        //        }

        //        GUI.Label(new Rect(k * iconWidthHeight, i * iconWidthHeight, iconWidthHeight, iconWidthHeight), texToUse);
        //        GUI.Label(new Rect(k * iconWidthHeight + 50, i * iconWidthHeight + 45, 100,100), string.Format("{0}",texto));
        //    }
        //}
    }
}
