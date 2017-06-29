using UnityEngine;

[System.Serializable]
public class SavedBaseClass
{
    private float _posX;
    private float _posY;
    private float _posZ;
    private float _rotX;
    private float _rotY;
    private float _rotZ;

    #region Properties

    /// <summary>
    /// 
    /// </summary>
    public float PosX
    {
        get
        {
            return _posX;
        }

        set
        {
            _posX = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float PosY
    {
        get
        {
            return _posY;
        }

        set
        {
            _posY = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float PosZ
    {
        get
        {
            return _posZ;
        }

        set
        {
            _posZ = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float RotX
    {
        get
        {
            return _rotX;
        }

        set
        {
            _rotX = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float RotY
    {
        get
        {
            return _rotY;
        }

        set
        {
            _rotY = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public float RotZ
    {
        get
        {
            return _rotZ;
        }

        set
        {
            _rotZ = value;
        }
    }

#endregion

    #region Get / Set

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return new Vector3(this._posX, this._posY, this._posZ);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRotation()
    {
        return new Vector3(this._rotX, this._rotY, this._rotZ);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void SetPosition(float x, float y, float z)
    {
        this._posX = x;
        this._posY = y;
        this._posZ = z;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void SetRotation(float x, float y, float z)
    {
        this._rotX = x;
        this._rotY = y;
        this._rotZ = z;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="position"></param>
    public void SetPosition(Vector3 position)
    {
        this._posX = position.x;
        this._posY = position.y;
        this._posZ = position.z;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation"></param>
    public void SetRotation(Vector3 rotation)
    {
        this._rotX = rotation.x;
        this._rotY = rotation.y;
        this._rotZ = rotation.z;
    }

    #endregion

}
