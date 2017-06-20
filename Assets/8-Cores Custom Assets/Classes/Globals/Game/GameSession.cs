using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class GameSession
{
    public int ID;
    //public Texture2D screenShot;
    public DateTime saveDate;
    public InventoryData savedInventory;
    public SavedCharacter savedCharacter;
    public SavedEnvironment savedEnvironment;
    //Add other classes to be saved.

    public void Update(DataManager dataManager)
    {
        BaseCharacter tempCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        //Inventory tempInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        dataManager.MergeClassProperties(savedCharacter, tempCharacter);
        //dataManager.MergeClassProperties(savedInventory, tempInventory);

        saveDate = DateTime.Now;
        //this.ID = dataManager.LoadAll().Count;
    }

    public void PrepareSessionForSaving(DataManager dataManager)
    {
        BaseCharacter tempCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        //Inventory tempInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();

        dataManager.MergeClassProperties(tempCharacter, savedCharacter);
        //dataManager.MergeClassProperties(tempInventory, savedInventory);

        saveDate = DateTime.Now;
        //this.ID = dataManager.LoadAll().Count;
    }

}
