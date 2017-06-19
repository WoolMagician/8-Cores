using UnityEngine;
using System.Collections;

[System.Serializable]
public class SavedCharacter : SavedBaseClass
{
    public string characterName = "";
    public float health;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float jumpForce;
    public bool isSelected;

}
