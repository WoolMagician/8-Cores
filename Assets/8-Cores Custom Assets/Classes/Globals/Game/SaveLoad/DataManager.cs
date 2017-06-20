using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class DataManager
{
    //All savable classes
    private SavedCharacter tempCharacter;
    private SavedInventory tempInventory;
    private SavedEnvironment tempEnvironment;
    private SavedSettings tempSettings;

    private GameManager gameManager;
    private BinaryFormatter binFormatter = new BinaryFormatter();
    private FileStream file = null;
    private static string settingsPath = Application.persistentDataPath + "/settings.ecd";
    private static string savesPath = Application.persistentDataPath + "/Saves";
    private static string dateTimeFormatString = "ddMMyyyyHHmmss";
    private static List<GameSession> savedSessions = new List<GameSession>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gm"></param>
    public DataManager(GameManager gm)
    {
        this.gameManager = gm;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Initialize()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        if (!File.Exists(Application.persistentDataPath + "/settings.ecd"))
        {
            File.Create(Application.persistentDataPath + "/settings.ecd");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="session"></param>
    public void Save(GameSession session)
    {
        try
        {
            session.PrepareSessionForSaving(this);

            savedSessions.Clear();
            savedSessions.Add(session);

            file = File.Create(savesPath + "/SAV_" + DateTime.Now.ToString(dateTimeFormatString) + ".ecd");

            binFormatter.Serialize(file, savedSessions);

            file.Close();

            Debug.Log("Saved new session -ID: " + savedSessions[0].ID +  "- to path: " + savesPath + "/SAV_" + DateTime.Now.ToString(dateTimeFormatString) + ".ecd");

        }
        catch (System.Exception)
        {
            Debug.LogWarning("Failed to save session.");
            throw;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GameSession Load(int index)
    {
        DirectoryInfo dir = new DirectoryInfo(savesPath);
        FileInfo[] fi = dir.GetFiles();
        List<GameSession> tempSessionList;
        GameSession returnGameSession = null;

        int count = fi.Length;

        if (Directory.Exists(savesPath))
        {
            Debug.Log("Loading save file...");

            for (int i = 0; i < count; i++)
            {
                string fileName = fi[i].FullName;

                if (File.Exists(fileName))
                {
                    file = File.Open(fileName, FileMode.Open);

                    tempSessionList = ((List<GameSession>)binFormatter.Deserialize(file));

                    if (tempSessionList[0].ID == index)
                    {
                        Debug.Log("Loaded: " + fileName);
                        returnGameSession =  tempSessionList[0];

                        continue;
                    }
                    
                }
                else
                {
                    //WE SHOULD NEVER GET THERE PORCODIO!!!
                    Debug.LogWarning("Error while loading file: File '" + fileName + "' not found.");
                    break;
                }

            }
            file.Close();
        }
        else
        {
            Debug.LogWarning("Error while searching files: Directory '" + savesPath + "' not found.");
        }

        return returnGameSession;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<GameSession> LoadAll()
    {
        DirectoryInfo dir = new DirectoryInfo(savesPath);
        FileInfo[] fi = dir.GetFiles();

        List<GameSession> tempSessionList;

        int count = fi.Length;

        savedSessions.Clear();

        if (Directory.Exists(savesPath))
        {
            Debug.Log("Updating save files list...");

            for (int i = 0; i < count; i++)
            {
                string fileName = fi[i].FullName;

                if (File.Exists(fileName))
                {
                    file = File.Open(fileName, FileMode.Open);

                    tempSessionList = ((List<GameSession>)binFormatter.Deserialize(file));
                    savedSessions.Add(tempSessionList[0]);
                    tempSessionList.Clear();

                    file.Close();
                    Debug.Log("Found: " + fileName);
                }
                else
                {
                    //WE SHOULD NEVER GET THERE PORCODIO!!!
                    Debug.LogWarning("Error while loading file: File '" + fileName + "' not found.");
                }

            }

            Debug.Log("Done updating list!");
        }
        else
        {
            Debug.LogWarning("Error while searching files: Directory '"+ savesPath +"' not found.");
        }

        return savedSessions;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settings"></param>
    public void SaveSettings(GameSettings settings)
    {
        tempSettings = new SavedSettings();

        MergeClassProperties(settings, tempSettings);

        if (File.Exists(settingsPath))
        {
            File.Delete(settingsPath);
        }

        file = File.Create(settingsPath);

        binFormatter.Serialize(file, tempSettings);

        file.Close();

        Debug.Log("Data settings to path: " + settingsPath);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GameSettings LoadSettings()
    {
        GameSettings retSettings = new GameSettings();

        tempSettings = null;

        if (File.Exists(settingsPath))
        {
            file = File.Open(settingsPath, FileMode.Open);
            tempSettings = (SavedSettings)binFormatter.Deserialize(file);
            file.Close();

        }
        else
        {
            tempSettings = new SavedSettings();
            Debug.LogWarning("No settings found, default ones loaded");
        }

        MergeClassProperties(tempSettings, retSettings);

        return retSettings;
    }

    /// <summary>
    /// Function used to merge different class types with same properties.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public void MergeClassProperties(System.Object from, System.Object to)
    {
        ClassMerger.MergeClassProperties(from, to);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetSavesNumber()
    {
        DirectoryInfo dir = new DirectoryInfo(savesPath);
        FileInfo[] fi = dir.GetFiles();

        return fi.Length;
    }
}

/// <summary>
/// 
/// </summary>
public class ClassMerger
{
    /// <summary>
    /// Function used to merge different class types with same properties.
    /// </summary>
    /// <param name="copyFrom"></param>
    /// <param name="copyTo"></param>
    public static void MergeClassProperties(object copyFrom, object copyTo)
    {
        BindingFlags flags;

        Dictionary<string, FieldInfo> targetDic;

        //Check if copyFrom or copyTo are null.
        if ((copyFrom == null) || (copyTo == null))
        {
            return;

        }

        //Assign binding flags for property search.
        flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        targetDic = copyTo.GetType().GetFields(flags).ToDictionary(f => f.Name);

        //Cicle trough each property of copyFrom, check if copyTo contains property then copy.
        foreach (FieldInfo field in copyFrom.GetType().GetFields(flags))
        {

            if (targetDic.ContainsKey(field.Name))
                targetDic[field.Name].SetValue(copyTo, field.GetValue(copyFrom));

            else
                Debug.LogWarning(string.Format("The field “{0}” has no corresponding field in the type “{1}”. Skipping field.", field.Name, copyTo.GetType().FullName));

        }
    }

    //PORCODDIO TROPPO CODICE INUTILE

    //public static void MergeToFrom(ref object copyTo, object copyFrom)
    //{
    //    var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    //    var targetDic = copyTo.GetType().GetFields(flags).ToDictionary(f => f.Name);

    //    foreach (var f in copyFrom.GetType().GetFields(flags))
    //    {
    //        if (targetDic.ContainsKey(f.Name))
    //            targetDic[f.Name].SetValue(copyTo, f.GetValue(copyFrom));

    //        else
    //            throw new InvalidOperationException(string.Format("The field “{0}” has no corresponding field in the type “{1}”.", f.Name, copyTo.GetType().FullName));

    //    }
    //}

    //public static TTarget MergeMonoBehaviour<TTarget>(object copyFrom, object copyTo) where TTarget : new()
    //{
    //    var flags = BindingFlags.Instance | BindingFlags.Public |
    //            BindingFlags.NonPublic;

    //    var targetDic = typeof(TTarget).GetFields(flags).ToDictionary(f => f.Name);


    //        foreach (var f in copyFrom.GetType().GetFields(flags))
    //        {
    //            if (targetDic.ContainsKey(f.Name))
    //                targetDic[f.Name].SetValue(copyTo, f.GetValue(copyFrom));
    //            else
    //                throw new InvalidOperationException(string.Format(
    //                    "The field “{0}” has no corresponding field in the type “{1}”.",
    //                    f.Name, typeof(TTarget).FullName));
    //        }

    //    return 0;
    //}

    //var flags = BindingFlags.Instance | BindingFlags.Public |
    //        BindingFlags.NonPublic;
    //var targetDic = typeof(TTarget).GetFields(flags).ToDictionary(f => f.Name);

    //    if (isMono)
    //    {
    //        //var ret = new TTarget();

    //        UnityEngine.Object ret2;


    //        foreach (var f in copyFrom.GetType().GetFields(flags))
    //        {
    //            if (targetDic.ContainsKey(f.Name))
    //                targetDic[f.Name].SetValue(ret, f.GetValue(copyFrom));
    //            else
    //                throw new InvalidOperationException(string.Format(
    //                    "The field “{0}” has no corresponding field in the type “{1}”.",
    //                    f.Name, typeof(TTarget).FullName));
    //        }

    //        return ret;
    //    }
}
