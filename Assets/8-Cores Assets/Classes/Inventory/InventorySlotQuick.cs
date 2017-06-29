/// <summary>
/// - Data structure
/// </summary>
[System.Serializable]
public class InventorySlotQuick : InventorySlot
{
    private string _keyOpen;
    private string _buttonOpen;

    /// <summary>
    /// 
    /// </summary>
    public string KeyOpen
    {
        get
        {
            return _keyOpen;
        }
        set
        {
            _keyOpen = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string ButtonOpen
    {
        get
        {
            return _buttonOpen;
        }
        set
        {
            _buttonOpen = value;
        }
    }
}
