using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public BaseCollectibleItem itemOld = null;
    public BaseCollectibleItemData item = null;
    public int ID;
    public int currentSlotValue = 0;
    public bool slotFull = false;
    public bool quickAccess = false;
    public string quickAccessKey = "";

    public int slotWidth = 20;
    public int slotHeight = 20;
    public int slotPosX = 0;
    public int slotPosY = 0;
    
    //public Texture slotTexture;

    public void CheckMaxValue()
    {
        if (currentSlotValue < item.maxStackValue)
        {
            slotFull = false;
        }
        else
        {
            slotFull = true;
        }
    }
}
