using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
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
        //savedSessions = LoadAll();

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
            //tempCharacter = new SavedCharacter();
            //MergeClassProperties(GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>(), tempCharacter);
            //session.savedCharacter = tempCharacter;
            //session.saveDate = DateTime.Now;
            //session.ID = LoadAll().Count;

            session.PrepareSessionForSaving(this);

            savedSessions.Clear();
            savedSessions.Add(session);

            //Check if save file already exists, if so append saved sessions.
            //if (File.Exists(savesPath))
            //{
            Debug.Log("Found save file, appending last save.");    //Need to check if we are in debug mode.

            //file = File.Open(savesPath, FileMode.Append, FileAccess.Write);

            file = File.Create(savesPath + "/SAV_" + DateTime.Now.ToString(dateTimeFormatString) + ".ecd");
            //}
            //else
            //{
            //    Debug.Log("Save file not found, creating a new one and appending last save."); 

            //    file = File.Create(savesPath);

            //}

            binFormatter.Serialize(file, savedSessions);

            file.Close();

            Debug.Log("Saved new session -ID: " + savedSessions[0].ID +  "- to path: " + savesPath + "/SAV_" + DateTime.Now.ToString(dateTimeFormatString) + ".ecd");

        }
        catch (System.Exception)
        {
            throw;
            Debug.LogWarning("Failed to save session.");

        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GameSession Load()   //POSSIBILE RIMOZIONE FUNZIONE, RIMPIAZZO CON 'Load(int sessionIndex)'
    {
        savedSessions.Clear();

        if (File.Exists(savesPath))
        {
            file = File.Open(savesPath, FileMode.Open);
            savedSessions = (List<GameSession>)binFormatter.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.LogWarning("Error while loading file: File not found.");
        }

        Debug.Log("Loaded: " + savesPath);

        return savedSessions[0];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionIndex"></param>
    /// <returns></returns>
    public GameSession Load(int sessionIndex)
    {
        savedSessions.Clear();

        if (Directory.Exists(savesPath))
        {
            Debug.Log("Loading save file...");

            if (File.Exists(savesPath + "/SAV_" + sessionIndex + ".ecd"))
            {
                file = File.Open(savesPath + "/SAV_" + sessionIndex + ".ecd", FileMode.Open);
                savedSessions = (List<GameSession>)binFormatter.Deserialize(file);
                file.Close();

                Debug.Log("Loaded file: " + savesPath + "/SAV_" + sessionIndex + ".ecd");
            }
            else
            {
                Debug.LogWarning("Error while loading file: File '" + savesPath + "/SAV_" + sessionIndex + ".ecd" + "' not found.");
            }
        }
        else
        {
            Debug.LogWarning("Error while loading file: Directory '" + savesPath + "' not found.");
        }

        return savedSessions[0];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public List<GameSession> LoadAll()
    {
        DirectoryInfo dir = new DirectoryInfo(savesPath);
        FileInfo[] fi = dir.GetFiles();

        int count = fi.Length;

        savedSessions.Clear();

        if (Directory.Exists(savesPath))
        {
            Debug.Log("Loading save files...");

            for (int i = 0; i < count - 1; i++)
            {
                string fileName = fi[i].Directory.FullName;

                if (File.Exists(fileName))
                {
                    file = File.Open(fileName, FileMode.Open);
                    savedSessions.Add(((GameSession)binFormatter.Deserialize(file)));
                    file.Close();
                    Debug.Log(" -" + fileName);
                }
                else
                {
                    Debug.LogWarning("Error while loading file: File '" + fileName + "' not found.");
                }

            }

            Debug.Log("Done!");
        }
        else
        {
            Debug.LogWarning("Error while loading file: Directory '"+ savesPath +"' not found.");
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
    /// <param name="camera"></param>
    /// <returns></returns>
    private Texture2D CameraScreenshot(Camera camera)
    {
        // 4k = 3840 x 2160   FHD = 1920 x 1080
        int resWidth = 1920;
        int resHeight = 1080;
        Texture2D screenShot;
        RenderTexture renderTexture;

        renderTexture = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = renderTexture;
        screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        return screenShot;
    }    //DA SPOSTARE NELLO SCRIPT DI GESTIONE TELECAMERA E/O IN GLOBALS

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
