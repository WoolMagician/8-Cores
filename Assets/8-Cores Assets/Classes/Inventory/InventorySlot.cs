using UnityEngine;

/// <summary>
/// -Data structure
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public BaseCollectibleItem itemOld = null;
    public BaseCollectibleItemData item = null;

    private int _ID;
    private int _currentSlotValue = 0;
    private float _slotWidth = 20;
    private float _slotHeight = 20;
    private float _slotPosX = 0;
    private float _slotPosY = 0;
    private bool _isSlotFull;

    /// <summary>
    /// 
    /// </summary>
    public int ID
    {
        get
        {
            return _ID;
        }
        set
        {
            _ID = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int value
    {
        get
        {
            return _currentSlotValue;
        }
        set
        {
            _currentSlotValue = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float width
    {
        get
        {
            return _slotWidth;
        }
        set
        {
            _slotWidth = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float height
    {
        get
        {
            return _slotHeight;
        }
        set
        {
            _slotHeight = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float positionX
    {
        get
        {
            return _slotPosX;
        }
        set
        {
            _slotPosX = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float positionY
    {
        get
        {
            return _slotPosY;
        }
        set
        {
            _slotPosY = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool isFull
    {
        get
        {
            if (_currentSlotValue < itemOld.maxStackValue)
            {
                _isSlotFull = false;
            }
            else
            {
                _isSlotFull = true;
            }

            return _isSlotFull;
        }
        set
        {
            _isSlotFull = value;
        }
    }
}
