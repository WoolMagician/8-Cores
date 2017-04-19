using UnityEngine;

[System.Serializable]
public class BaseCollectibleItem : MonoBehaviour
{
    public string itemName;
    public int itemID = -1;
    public int maxStackValue;
    public int maxStackNumber;
    public int currentStackValue = 1;
    public int dropRatePercent;
    public Inventory inventory;

    public GameObject itemMaterial;
    public GameObject item2DTexture;
    public GameObject itemMesh;


    //Overrides drop rate percent
    public bool randomDropRate;

    public Rarity rarity;
    public Type type;


    public void Update()
    {

    }

    public Color GetColorFromRarity()
    {
        switch (rarity)
        {
            case Rarity.Common:
                return Color.green;

            case Rarity.Uncommon:
                return Color.cyan;

            default:
                return Color.grey;
        }
    }

    public enum Rarity
    {
        Common = 1,
        Uncommon = 70,
        Rare = 50,
    }

    public enum Type
    {
        Battery = 0,
        Health = 11,
        Junk = 1,
        BrokenRubberBand = 2,
        Nail = 3,

        Spring = 4,
        Screw = 5,
        CopperPlate = 6,

        IronPlate = 7,
        SilverPlate = 8,
        GoldPlate = 9,
        PlatinumPlate = 10,
    }
}