using UnityEngine;

public class InventorySlot
{
    public BaseCollectibleItem item = null;
    public int currentSlotValue = 0;
    public bool slotFull = false;
    
    public int slotWidth = 20;
    public int slotHeight = 20;
    public int slotPosX = 0;
    public int slotPosY = 0;
    
    public Texture slotTexture;

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