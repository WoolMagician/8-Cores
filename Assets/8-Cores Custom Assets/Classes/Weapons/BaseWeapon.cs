using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public int attackValue;
    public string weaponName;
    public int weaponID;
    public int batterySlots;

    public Upgrade currentUp;

    private void Start()
    {

        currentUp.upgradeType = Upgrade.UpgradeType.BASE;

    }

}

public class Upgrade
{
    public UpgradeType upgradeType;
    public enum UpgradeType
    { 
        BASE = -1,
        testUp = 0,
    }

}