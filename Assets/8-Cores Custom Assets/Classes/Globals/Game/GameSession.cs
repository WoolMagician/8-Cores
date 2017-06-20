using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class GameSession
{
    public int ID;
    public DateTime creationDate;
    public InventoryData inventory;
    public SavedCharacter savedCharacter;
    public SavedEnvironment savedEnvironment;
    public byte[] savedScreenShot;

    public void Update(DataManager dataManager)
    {
        BaseCharacter tempCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        dataManager.MergeClassProperties(savedCharacter, tempCharacter);
        
    }

    public void PrepareSessionForSaving(DataManager dataManager)
    {
        BaseCharacter tempCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        dataManager.MergeClassProperties(tempCharacter, savedCharacter);

        this.creationDate = DateTime.Now;
        this.ID = dataManager.GetSavesNumber();

    }
}
