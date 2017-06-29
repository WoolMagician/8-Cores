using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameManager"></param>
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

            tempMiniature.LoadImage(gameManager.allSessions[i].miniatureBytes);

            tempSaveSlotGUI = savesSlotList[i];
            tempSaveSlotGUI.miniature.texture = tempMiniature;
            tempSaveSlotGUI.miniature.color = Color.white;
            tempSaveSlotGUI.characterName.text = "Character: " + gameManager.allSessions[i].character.Name;
            tempSaveSlotGUI.dateTime.text = "Save date: " + gameManager.allSessions[i].lastSaveDate;

        }
    }
}
