using System;
using UnityEngine;

/// <summary>
/// -Data structure
/// </summary>
[System.Serializable]
public class GameSession
{
    private int _ID;
    private string _lastSaveDate;
    private InventoryData _inventory;
    private SavedCharacter _character;
    private SavedEnvironment _environment;
    private int _sceneIndex;
    private byte[] _miniature; //Miniature image stored in bytes, needed for serialization.

    /// <summary>
    /// 
    /// </summary>
    public GameSession()
    {
        this._ID = 0;
        this._lastSaveDate = DateTime.Now.ToString();
        this._inventory = new InventoryData(20, 4);
        this._character = new SavedCharacter();
        this._environment = new SavedEnvironment();
        this._miniature = new byte[] { 0 };
    }

    /// <summary>
    /// Property used to get / set session ID.
    /// </summary>
    public int ID
    {
        get
        {
            return this._ID;
        }
        set
        {
            this._ID = value;
        }
    }

    /// <summary>
    /// Property used to get / set session ID.
    /// </summary>
    public string lastSaveDate
    {
        get
        {
            return this._lastSaveDate;
        }
        set
        {
            this._lastSaveDate = value;
        }
    }

    /// <summary>
    /// Property used to get / set session ID.
    /// </summary>
    public InventoryData inventory
    {
        get
        {
            return this._inventory;
        }
        set
        {
            this._inventory = value;
        }
    }

    /// <summary>
    /// Property used to get / set session ID.
    /// </summary>
    public SavedCharacter character
    {
        get
        {
            return this._character;
        }
        set
        {
            this._character = value;
        }
    }

    /// <summary>
    /// Property used to get / set session ID.
    /// </summary>
    public SavedEnvironment environment
    {
        get
        {
            return this._environment;
        }
        set
        {
            this._environment = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public int SceneIndex
    {
        get
        {
            return _sceneIndex;
        }
        set
        {
            _sceneIndex = value;
        }
    }

    /// <summary>
    /// Property used to get / set session ID.
    /// </summary>
    public byte[] miniatureBytes
    {
        get
        {
            return this._miniature;
        }
        set
        {
            this._miniature = value;
        }
    }
}
