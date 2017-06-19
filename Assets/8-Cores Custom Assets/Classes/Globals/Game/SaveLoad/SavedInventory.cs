using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;
using UnityEngine;

[System.Serializable]
public class SavedInventory : SavedBaseClass
{
    public List<InventorySlot> slotList = new List<InventorySlot>();
    public int currentJunks = 0;
    public string openInventoryButton = "";
    public string openInventoryKey = "";
    public Texture2D quickSlotTexture;
    public bool isInvOpened = false;
    public bool isQuickOpened = true;
    public int maxSlotNumber;
    public int quickAccessSlotNumber;
    public BlurOptimized blur;
    public BaseCollectibleItem[] items;

}
