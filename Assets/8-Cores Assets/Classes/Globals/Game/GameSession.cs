using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class GameSession
{
    public int ID;
    public string lastActivityDate;
    public InventoryData inventory;
    public SavedCharacter character;
    public SavedEnvironment environment;

    [HideInInspector]
    public byte[] miniature;

    public GameSession()
    {
        this.ID = 0;
        this.lastActivityDate = DateTime.Now.ToString();
        this.inventory = new InventoryData(20, 4);
        this.character = new SavedCharacter();
        this.miniature = new byte[] { 0 };
        this.environment = new SavedEnvironment();
    }

    public void Update(GameManager gameManager)
    {
        BaseCharacter tempCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        gameManager.MergeClassProperties(character, tempCharacter);
        
    }

    public void PrepareSessionForSaving(GameManager gameManager)
    {
        BaseCharacter tempCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        gameManager.MergeClassProperties(tempCharacter, character);

        this.lastActivityDate = DateTime.Now.ToString();
        this.ID = gameManager.GetSavesNumber();

        //dataManager.gameManager.TakeSessionSaveScreenshot(this);
        //dataManager.gameManager.StartCoroutine(dataManager.gameManager.wait());
        //Debug.Log("PORCODDIO SESSIONE"+ savedScreenShot.Length);
    }


}
