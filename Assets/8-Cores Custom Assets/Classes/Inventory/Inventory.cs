using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour {
    //[HideInInspector]
    public List<InventorySlot> slotList = new List<InventorySlot>();

    public int currentJunks = 0;

    [HideInInspector]
    public string openInventoryButton = "PS4_TRIANGLE";

    [HideInInspector]
    public string openInventoryKey = "";

    public Texture2D quickSlotTexture;

    public bool isInvOpened = false;

    public bool isQuickOpened = true;

    public int maxSlotNumber;

    public int quickAccessSlotNumber;

    public BlurOptimized blur;

    public BaseCollectibleItem[] items;

    public void Start()
    {
        maxSlotNumber = 20;
        quickAccessSlotNumber = 4;

        blur = Camera.main.GetComponent<BlurOptimized>();
        blur.enabled = isInvOpened;



        for (int i = 0; i < quickAccessSlotNumber; i++)
        {
            InventorySlot tempSlot = new InventorySlot();

            tempSlot.quickAccess = true;

            tempSlot.item = items[i];

            tempSlot.currentSlotValue = Random.Range(1,10);

            slotList.Add(tempSlot);
        }

    }

    public void Update()
    {
        if (Input.GetKeyDown("i")) //|| Input.GetButtonDown(openInventoryButton))
        {
            isQuickOpened = isInvOpened;
            isInvOpened = !isInvOpened;
            blur.enabled = isInvOpened;

            if (isInvOpened)
            {
                Time.timeScale = 0;
            }

            else
            {
                Time.timeScale = 1;
            }
        }





        //if (Input.GetKey("q") && !isInvOpened)//|| Input.GetButtonDown(openInventoryButton))
        //{
        //    isQuickOpened = true;
        //}
        //else
        //{
        //    isQuickOpened = false;
        //}
    }

    public ActionResult AddItem(BaseCollectibleItem objectToAdd)
    {
        bool needNewItemSlot = true;
        
        foreach (InventorySlot slot in slotList)
        {
            if (!slot.quickAccess)
            {
                if (slot.item.type == objectToAdd.type)
                {
                    if (!slot.slotFull)
                    {
                        needNewItemSlot = false;

                        slot.currentSlotValue += 1;

                        slot.CheckMaxValue();

                        Debug.Log("PICKED UP OBJECT!");

                        return ActionResult.Success;
                    }
                }
            }

        }
        
        if (needNewItemSlot)
        {
            int slotCounter = 0;

            if (slotList.Count == maxSlotNumber)
            {
                Debug.Log("INVENTORY FULL!");
                return ActionResult.InventoryFull;
            }

            foreach (InventorySlot slot1 in slotList)
            {
                if (!slot1.quickAccess)
                {
                    if (slot1.item.type == objectToAdd.type)
                    {
                        slotCounter += 1;
                    }
                }
            }

            if (slotCounter < objectToAdd.maxStackNumber)
            {
                if (slotList.Count - 1 < maxSlotNumber)
                {
                    InventorySlot tempSlot = new InventorySlot();

                    tempSlot.currentSlotValue = 1;

                    tempSlot.item = objectToAdd;

                    slotList.Add(tempSlot);

                    Debug.Log("NEW SLOT CREATED!");

                    return ActionResult.Success;
                }
            }
            else
            {
                Debug.Log("MAX STACK NUMBER REACHED!");

                return ActionResult.MaxStack;
            }
        }

        return ActionResult.Fail;
    }

    public ActionResult AddItem(JunkCollectibleItem objectToAdd)
    {
        currentJunks += objectToAdd.junkValue;
        return ActionResult.Success;
    }

    //DA RICONTROLLARE, MODIFICATO IL 12/06/2017
    public ActionResult RemoveItem(BaseCollectibleItem.Type itemToRemove, int quantity)
    {
        foreach (InventorySlot slot in slotList)
        {
            if (slot.item.type == itemToRemove)
            {
                if (slot.currentSlotValue >= quantity)
                {

                    //if (quantity < slot.currentSlotValue)
                    //{
                        slot.currentSlotValue -= quantity;

                        Debug.Log(slot.currentSlotValue);

                        if (slot.currentSlotValue == 0)
                        {
                            slotList.Remove(slot);
                        }

                        return ActionResult.Success;

                    //}


                }
                else
                {
                    Debug.Log("You don't have enought items.");
                    return ActionResult.NotEnoughtItems;
                }

            }

        }
        return ActionResult.NoItemFound;
    }

    [HideInInspector]
    public enum ActionResult
    {
        Fail = -1,
        Success = 0,
        NotEnoughtItems = 1,
        MaxStack = 2,
        InventoryFull = 3,
        NoItemFound = 4,
    }

    private void OnGUI()
    {
        if (isQuickOpened)
        {
            ArrangeRapidAccessSlots();
        }
        
    }

    public void ArrangeRapidAccessSlots()
    {
        int numberOfPoints = 0;
        int circleRadius = 55;
        float angleIncrement = 0;

        numberOfPoints = quickAccessSlotNumber;
        angleIncrement = 360 / numberOfPoints;

        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector2 p = new Vector2();

            p.x = (circleRadius * Mathf.Cos((angleIncrement * i) * (Mathf.PI / 180)));
            p.y = (circleRadius * Mathf.Sin((angleIncrement * i) * (Mathf.PI / 180)));

            GUI.depth = 2;
            GUI.Label(new Rect((150 + p.x) - (20 / 2), (((Screen.height - 150) + p.y) - (20 / 2)), 20, 20), quickSlotTexture);

        }
    }

}
