using UnityEngine;

public class JunkCollectibleItem : BaseCollectibleItem
{
    public int junkValue;

    [HideInInspector]
    public GameObject inventoryGameObject;

    [HideInInspector]
    public Inventory inventoryCopy;

    private void Start()
    {
        inventoryGameObject = GameObject.Find("Inventory");
        inventoryCopy = (Inventory)inventoryGameObject.GetComponent(typeof(Inventory));
        inventory = inventoryCopy;
    }

    public enum JunkType
    {    
        Junk = 0,
        BrokenRubberBand = 0,
        Nail = 0,
        
        Spring = 1,
        Screw = 1,
        CopperPlate = 1,
    
        IronPlate = 6,
        SilverPlate = 7,
        GoldPlate = 8,
        PlatinumPlate = 9,
        

        //Other collectible types
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (inventory.AddItem(this) == Inventory.ActionResult.Success)
            {
                Destroy(this.gameObject);
            }
        }

    }
}