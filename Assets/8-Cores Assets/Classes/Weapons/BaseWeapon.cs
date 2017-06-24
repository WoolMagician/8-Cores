using UnityEngine;


[System.Serializable]
public class BaseWeapon : MonoBehaviour
{
    public int attackValue;
    public string weaponName;
    public int weaponID; //Always >= 0
    public int batterySlots;

    private void Start()
    {

    }

}
