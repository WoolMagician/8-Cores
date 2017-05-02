using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CharacterManager : MonoBehaviour
{
    public List<BaseCharacter> characters = new List<BaseCharacter>(5);
    
    public string changeButton = "x";

    private int selectedCharIndex = 0;

    //Change class type with current character controller. 
    private RPGCharacterControllerFREE charController;

    //Reference game object for transition effects.
    private GameObject transitionEffect;


    void Start()
    {
        if (charController == null)
        {
            Debug.LogWarning("CharacterManager.cs: Missing character controller script!");
        }

        if (transitionEffect == null)
        {
            Debug.LogWarning("CharacterManager.cs: Missing transition effect game object!");
        }
    }

    void Update()
    {
        if (characters.Count > 0)
        {
            if (Input.GetKeyDown(changeButton))
            {
                //Lock character movement.
                //Change coroutine according to current character controller script.
                StartCoroutine(charController._LockMovementAndAttack(0, 0.25f));

                //Play transition effect.
                StartCoroutine(PlayTransitionEffect(0, 1f));

                //Update character.
                UpdateCurrentCharacter(selectedCharIndex);

            }
        }
    }

    void UpdateCurrentCharacter(int index)
    {
        int indexRollback = selectedCharIndex;

        //Deactivate old character
        if (characters[selectedCharIndex].isSelected)
        {
            characters[selectedCharIndex].gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("CharacterManager.cs: Error while trying to deactivate old character.");
            return;
        }

        //Clamp selectedCharIndex maximum to characters.Count
        if (selectedCharIndex > characters.Count - 1)
        {
            //Reset index.
            selectedCharIndex = 0;
        }
        else
        {
            //Update index.
            selectedCharIndex += 1;
        }

        if (!characters[selectedCharIndex].isSelected)
        {
            //Activate new character
            characters[selectedCharIndex].gameObject.SetActive(true);
        }       
        else
        {
            Debug.LogWarning("CharacterManager.cs: Error while trying to activate new character.");

            //Rollback index on failed switch
            selectedCharIndex = indexRollback;

            return;
        }
    }

    IEnumerator PlayTransitionEffect(float delayTime, float effectTime)
    {
        yield return new WaitForSeconds(delayTime);
        transitionEffect.SetActive(true);

        yield return new WaitForSeconds(effectTime);
        transitionEffect.SetActive(false);
    }
}
