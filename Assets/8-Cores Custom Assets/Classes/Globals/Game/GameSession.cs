using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class GameSession
{
    public int ID;
    public Texture2D screenShot;
    public DateTime saveDate;
    public SavedInventory inventory;
    public SavedCharacter character;
    public SavedEnvironment environment;
    //Add other classes to be saved.
}
