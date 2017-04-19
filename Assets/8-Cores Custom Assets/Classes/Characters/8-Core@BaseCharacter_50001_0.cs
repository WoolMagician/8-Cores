using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public string characterName = "";

    public float health;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float jumpForce;

    public Mesh characterMesh;

    public Avatar characterAvatar;

    public BaseWeapon[] weaponArray;

    private void Start()
    {
        characterMesh = this.GetComponent<Mesh>();

        characterAvatar = this.GetComponent<Avatar>();
    }

}