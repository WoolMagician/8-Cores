using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour {

    [HideInInspector]
    public Inventory inventory;

    public GameObject blurredOverlay;
    public Material blurredOverlayMaterial;

    public Texture emptyTex;
    int iconWidthHeight = 75;
    Rect rect = new Rect(0, 0, Screen.width, Screen.height);

    //public GameObject characterWeapon;

    [HideInInspector]
    //public GameObject inventoryWeapon;

    public Vector3 screenCenter;

    private void Start()
    {
        inventory = this.GetComponent<Inventory>();
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 3f));
    }

    // Create the an 8x8 Texture-Array 
    public void Awake()
    {
        //Renderer blurredOverlayRenderer;

        //blurredOverlay = UnityEngine.GameObject.CreatePrimitive(PrimitiveType.Plane);
        //blurredOverlay = Instantiate(blurredOverlay, screenCenter + new Vector3(0,0,1f), Quaternion.identity);
        //blurredOverlay.transform.localScale = new Vector3(2,0,2);
        //blurredOverlay.transform.LookAt(Camera.main.transform);

        //blurredOverlayRenderer = blurredOverlay.GetComponent<Renderer>();

        //blurredOverlayRenderer.material = blurredOverlayMaterial;

        ////emptyTex = new Texture2D(Screen.width, Screen.height);
        //inventory = new Texture[5][];

        //for (int i = 0; i < inventory.Length; i++)
        //{
        //    inventory[i] = new Texture[6];
        //}
    }

    private void DrawWeapon()
    {
        

        //inventoryWeapon = Instantiate(characterWeapon, screenCenter, Quaternion.identity);
    }

    public void OnGUI()
    {
        if (inventory.isInvOpened)
        {
            GUI.Box(rect, "Inventory");
            GUI.Label(new Rect(10, 10, iconWidthHeight, iconWidthHeight), emptyTex);


            //if (inventoryWeapon == null)
            //{
            //    DrawWeapon();
            //}

        }
        else
        {
            //;

            //if (inventoryWeapon != null)
            //{
            //    Destroy(inventoryWeapon);
            //}

        }


        if (inventory.isQuickOpened)
        {
            PopulateQuickAccessSlots(inventory.quickAccessSlotNumber);
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

    public void PopulateQuickAccessSlots(int slotNumber)
    {
        int numberOfPoints = 0;
        int circleRadius = 55;
        float angleIncrement = 0;
        InventorySlot slot;
        Vector2 p = new Vector2();

        numberOfPoints = slotNumber;
        angleIncrement = 360 / numberOfPoints;

        for (int i = 0; i < inventory.maxSlotNumber; i++)
        {

            p.x = (circleRadius * Mathf.Cos((angleIncrement * i) * (Mathf.PI / 180)));
            p.y = (circleRadius * Mathf.Sin((angleIncrement * i) * (Mathf.PI / 180)));

            if (inventory.slotList.Count > i)
            {
                slot = inventory.slotList[i];

                if (slot.quickAccess)
                {
                    if (slot.item != null)
                    {
                        GUI.depth = 1;
                        GUI.Label(new Rect((150 + p.x) - (iconWidthHeight / 2), (((Screen.height - 150) + p.y) - (iconWidthHeight / 2)), iconWidthHeight, iconWidthHeight), slot.item.item2DTexture);
                        GUI.Label(new Rect((150 + p.x) + (iconWidthHeight / 2) - 20, ((Screen.height - 150) + p.y) + (iconWidthHeight / 2) - 25, iconWidthHeight, iconWidthHeight), slot.currentSlotValue.ToString());
                    }
                }
            }

            p.x = 0f;
            p.y = 0f;
            slot = null;

        }
    }
}
