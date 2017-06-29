using UnityEngine;

/// <summary>
/// -Base class for all characters.
/// -Data structure
/// </summary>
public class BaseCharacter : MonoBehaviour
{
    private string _characterName;
    private float _health;
    private float _walkSpeed;
    private float _runSpeed;
    private float _jumpSpeed;
    private float _jumpForce;

    /// <summary>
    /// Property used to get / set character name provided by user.
    /// </summary>
    public string name
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
    public float health
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
    /// Property used to get / set current walk speed value of character.
    /// </summary>
    public float walkSpeed
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
    public float runSpeed
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
    public float jumpSpeed
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
    public float jumpForce
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
    public bool isSelected
    {
        get
        {
            return this.enabled;
        }
    }

}
