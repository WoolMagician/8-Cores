[System.Serializable]
public class SavedCharacter: SavedBaseClass
{
    private string _characterName;
    private float _health;
    private float _walkSpeed;
    private float _runSpeed;
    private float _jumpSpeed;
    private float _jumpForce;
    private bool _isSelected;


    #region Properties

    /// <summary>
    /// Property used to get / set character name provided by user.
    /// </summary>
    public string Name
    {
        get
        {
            return _characterName;
        }
        set
        {
            _characterName = value;
        }
    }

    /// <summary>
    /// Property used to get / set current health value of character.
    /// </summary>
    public float Health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;
        }
    }

    /// <summary>
    /// Property used to get / set current health value of character.
    /// </summary>
    public float WalkSpeed
    {
        get
        {
            return _walkSpeed;
        }

        set
        {
            _walkSpeed = value;
        }
    }

    /// <summary>
    /// Property used to get / set current run speed value of character.
    /// </summary>
    public float RunSpeed
    {
        get
        {
            return _runSpeed;
        }

        set
        {
            _runSpeed = value;
        }
    }

    /// <summary>
    /// Property used to get / set current jump speed value of character.
    /// </summary>
    public float JumpSpeed
    {
        get
        {
            return _jumpSpeed;
        }

        set
        {
            _jumpSpeed = value;
        }
    }

    /// <summary>
    /// Property used to get / set current jump force value of character.
    /// </summary>
    public float JumpForce
    {
        get
        {
            return _jumpForce;
        }

        set
        {
            _jumpForce = value;
        }
    }

    /// <summary>
    /// Property used to get if character is selected or not.
    /// </summary>
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
        }
    }

    #endregion
}
