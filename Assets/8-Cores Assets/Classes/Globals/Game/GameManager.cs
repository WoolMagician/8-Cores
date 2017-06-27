using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityStandardAssets.ImageEffects;


public class GameManager : MonoBehaviour
{
    //DA FINIRE LOADING SESSION
    public GameSession[] allSessions = new GameSession[5];    //Tutti i salvataggi fatti fin'ora
    private GameSession tempSession;    //Sessione temporanea di appoggio per il salvataggio/caricamento dati.
    private GameSettings gameSettings = new GameSettings();
    private BaseCharacter tempChar;

    private int screenWidth = Screen.width;
    private int screenHeight = Screen.height;
    private int sessionToLoadIndex;

    [HideInInspector]
    public bool newGame = false;

    [HideInInspector]
    public bool isLoading = false;

    [HideInInspector]
    public bool isSaving = false;

    //Da commentare
    public static DataManager dataManager;
    public MainGUI mainGUI;
    public LoadingScreen loadingScreen;
    public GameSession currentSession;

    //DATAMANGER VARS
    //All savable classes
    private SavedCharacter tempCharacter;
    private SavedInventory tempInventory;
    private SavedEnvironment tempEnvironment;
    private SavedSettings tempSettings;

    private BinaryFormatter binFormatter = new BinaryFormatter();
    private FileStream file = null;
    private static string settingsPath;
    private static string savesPath;
    private static string dateTimeFormatString = "ddMMyyyyHHmmss";
    private static List<GameSession> savedSessions = new List<GameSession>();
    public RenderTexture test;

    private Camera mainCamera;


    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        mainCamera = Camera.main;
        settingsPath = Application.persistentDataPath + "/settings.ecd";
        savesPath = Application.persistentDataPath + "/Saves";

        //Creates Saves directory and base settings file.
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        if (!File.Exists(Application.persistentDataPath + "/settings.ecd"))
        {
            File.Create(Application.persistentDataPath + "/settings.ecd");
        }

        //Load all sessions, user will chose wich one to load.
        allSessions = LoadAllSessions().ToArray();

        //Load settings.
        gameSettings = LoadSettings();

        sessionToLoadIndex = -1;

        //Check if there are any save files and let user chose.
        if (allSessions.Length < 1)
        { 
            //NEW GAME NEEDED.
            newGame = true;
            mainGUI.continueGameButton.gameObject.SetActive(false);

        }

        //texture2.LoadImage(texture.EncodeToPNG());

        //mainMenu.optionsButton.onClick.AddListener(ShowOptionsMenu);
        //UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/8-Cores Custom Assets/Prefabs/LoadingScreen.prefab", typeof(GameObject));
        //GameObject clone = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        //clone.name = "LoadingScreen";

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            StartCoroutine(Save());

        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            SaveSettings(gameSettings);

        }

        else if (Input.GetKeyDown(KeyCode.F12))
        {
            gameSettings = LoadSettings();

        }

    }

    /// <summary>
    /// 
    /// </summary>
    private void NewGame()  //FUNZIONE DEL RESET TOTALE GLOBALE DE CRISTO
    {
        if (newGame)
        {
            currentSession = new GameSession();

            //currentSession.character = new SavedCharacter();

            tempSession = currentSession;
            SaveSession(tempSession);

            Debug.Log("New game created!");
        }
        else
        {
            //WE SHOULD NEVER GET HERE!
            Debug.LogError("GameManager.cs: NewGame() function error!");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadGame()
    {
        if(sessionToLoadIndex > -1)
        {
            currentSession = LoadSession(sessionToLoadIndex);
            currentSession.Update(this);

        }
        else
        {
            //WE SHOULD NEVER GET HERE!
            Debug.LogError("GameManager.cs: LoadGame() function error!");
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="flag"></param>
    private void ShowOptionsMenu(bool flag)
    {
        //mainMenu.optionsMenu.gameObject.SetActive(!);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="flag"></param>
    private void ShowSelectSaveMenu(bool flag)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator Save()   //WE SHOULD CHECK SAVING FUNCTION ACKNOWLEDGE MADONNADDIO
    {
        //PUT FUNC SHOW SAVING IN PROGRESS.

        isSaving = true;

        //Using lambda expression to return values from SaveMiniature iterator.
        yield return SaveMiniature(value => currentSession.miniature = value);

        //Using a temporary session for saving.
        tempSession = currentSession;

        //Save session with index increased.
        SaveSession(tempSession);

        //Update sessions list.
        allSessions = LoadAllSessions().ToArray();

        isSaving = false;

        //PUT FUNC SHOW SAVING COMPLETE.
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public IEnumerator SaveMiniature(System.Action<byte[]> result)
    {
        RenderTexture renderTexture;
        Texture2D miniature;
        BlurOptimized blur;

        //Waiting for end of current frame.
        yield return new WaitForEndOfFrame();

        blur = mainCamera.GetComponent<BlurOptimized>();

        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        renderTexture.useMipMap = false;
        renderTexture.antiAliasing = 1;

        RenderTexture.active = renderTexture;
        mainCamera.targetTexture = renderTexture;

        miniature = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        blur.enabled = false; //Disable blur temporarly just for shot.

        mainCamera.Render();
        miniature.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);

        blur.enabled = true;
        miniature.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        //Return image encoded in byte array.
        result(miniature.EncodeToPNG());

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="session"></param>
    public void SaveSession(GameSession session)
    {
        try
        {
            session.PrepareSessionForSaving(this);

            savedSessions.Clear();
            savedSessions.Add(session);

            file = File.Create(savesPath + "/SAV_" + DateTime.Now.ToString(dateTimeFormatString) + ".ecd");

            binFormatter.Serialize(file, savedSessions);

            file.Close();

            Debug.Log("Saved new session -ID: " + savedSessions[0].ID + "- to path: " + savesPath + "/SAV_" + DateTime.Now.ToString(dateTimeFormatString) + ".ecd");

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
    public GameSession LoadSession(int index)
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
                        returnGameSession = tempSessionList[0];

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
    public List<GameSession> LoadAllSessions()
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
            Debug.LogWarning("Error while searching files: Directory '" + savesPath + "' not found.");
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void UpdateSessionToLoadIndex(int index)
    {
        sessionToLoadIndex = index;
    }

    public void PopulateSaveList()
    {
        mainGUI.PopulateSaveList(this);
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
                Debug.LogWarning(string.Format("The field �{0}� has no corresponding field in the type �{1}�. Skipping field.", field.Name, copyTo.GetType().FullName));

        }
    }
}
