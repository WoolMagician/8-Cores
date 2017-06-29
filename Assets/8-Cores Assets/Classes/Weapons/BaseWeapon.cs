using UnityEngine;

/// <summary>
/// -Data structure
/// </summary>
[System.Serializable]
public class BaseWeapon : MonoBehaviour
{
    private int _attackValue;
    private string _weaponName;
    private int _weaponID; //Always >= 0
    private int _batterySlots;

    /// <summary>
    /// 
    /// </summary>
    public int attackValue
    {
        get
        {
            return _attackValue;
        }
        set
        {
            _attackValue = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string name
    {
        get
        {
            return _weaponName;
        }
        set
        {
            _weaponName = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int ID
    {
        get
        {
            return _weaponID;
        }
        set
        {
            _weaponID = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int batterySlots
    {
        get
        {
            return _batterySlots;
        }
        set
        {
            _batterySlots = value;
        }
    }

}
