using UnityEngine;

[System.Serializable]
public class BaseCharacter : MonoBehaviour
{
    public string characterName = "";

    public float health;
    public float walkSpeed;
    public float runSpeed;
    public float jumpSpeed;
    public float jumpForce;

    public bool isSelected;

    public SkinnedMeshRenderer characterMesh;

    public Avatar characterAvatar;

    public BaseWeapon[] weaponArray;

    private void Start()
    {
        characterMesh = this.GetComponent<SkinnedMeshRenderer>();

        characterAvatar = this.GetComponent<Avatar>();

    }

    private void Update()
    {
        isSelected = this.enabled;
    }

}
