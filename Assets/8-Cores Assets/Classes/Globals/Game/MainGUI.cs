using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainGUI : MonoBehaviour
{
    //MAIN MENU VARIABLES.
    public Button newGameButton;
    public Button continueGameButton;
    public Button optionsButton;
    public Canvas optionsMenu;

    //LOAD MENU VARIABLES.
    public ScrollRect savesScrollableList;
    public SaveSlotGUI[] savesSlotList;

    public void PopulateSaveList(GameManager gameManager)
    {
        int count;

        count = gameManager.allSessions.Length;

        //Clamp max saves that can be loaded, temporaneo prob va tolto & generate le slot progressivamente ma non ho sbatti.
        if (count >= 10)
        {
            count = 10;
        }

        for(int i = 0; i < count; i++)
        {
            //ADD OTHER INFOS;
            SaveSlotGUI tempSaveSlotGUI;

            Texture2D tempMiniature = new Texture2D(100,100);

            tempSaveSlotGUI = savesSlotList[i];

            tempMiniature.LoadImage(gameManager.allSessions[i].miniature);

            tempSaveSlotGUI.miniature.texture = tempMiniature;

            tempSaveSlotGUI.miniature.color = Color.white;

            tempSaveSlotGUI.characterName.text = "Character: " + gameManager.allSessions[i].character.characterName;

            tempSaveSlotGUI.dateTime.text = "Save date: " + gameManager.allSessions[i].lastActivityDate;

        }
    }
}
