using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CharacterManager : MonoBehaviour
{
    public List<BaseCharacter> characters = new List<BaseCharacter>();
    
    public string changeButton = "x";

    public int selectedIndex = 0;

    private SkinnedMeshRenderer currentMesh;

    private Avatar currentAvatar;

    private Animator characterAnimator;

    public SkinnedMeshRenderer meshRenderer;

    private RPGCharacterControllerFREE characterController;

    // Use this for initialization
    void Start()
    {


        characterAnimator = GetComponent<Animator>();

        currentAvatar = characterAnimator.avatar;

        characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<RPGCharacterControllerFREE>();

        UpdateCurrentCharacter(selectedIndex);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if we got character controller attached to game object
        if (characterController == null)
        {
            Debug.LogWarning("CharacterManager.cs: Missing character controller!");
            return; //Exit from Update
        }

        if (Input.GetKeyDown(changeButton))
        {
            currentMesh = null; //Clear mesh
            currentAvatar = null; //Clear avatar

            StartCoroutine(characterController._LockMovementAndAttack(0, 0.25f));

            PlayTransitionEffect(); //Play transition effects before switch characters

            UpdateCurrentCharacter(selectedIndex);

            selectedIndex += 1;
        }

        /*
         * Clamp selectedIdex maximum to characters.Count
         * Use this function if -select character- menu is disabled or character number is <= 2.
        */
        if(selectedIndex > characters.Count - 1)
        {
            selectedIndex = 0;
        }

    }

    void UpdateCurrentCharacter(int index)
    {
        meshRenderer = characters[index].characterMesh;

        currentAvatar = characters[index].characterAvatar;

    }

    void PlayTransitionEffect()
    {

    }
}
