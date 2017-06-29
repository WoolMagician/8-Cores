using System;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public bool newGame = false;

    [HideInInspector]
    public bool isLoading = false;

    [HideInInspector]
    public bool isSaving = false;

    public MainGUI mainGUI;
    public LoadingScreen loadingScreen;
    public GameSession currentSession;
    public GameSession[] allSessions = new GameSession[0];    //Tutti i salvataggi fatti fin'ora

    private GameSession _tempSession;    //Sessione temporanea di appoggio per il salvataggio/caricamento dati.
    private GameSettings _gameSettings = new GameSettings();
    private BaseCharacter _tempChar;
    private SavedSettings _tempSettings;
    private BinaryFormatter _binFormatter = new BinaryFormatter();
    private FileStream _file = null;
    private Camera _mainCamera;
    private static List<GameSession> _savedSessions = new List<GameSession>();
    private int _screenWidth = Screen.width;
    private int _screenHeight = Screen.height;
    private int _sessionToLoadIndex;
    private static string _settingsPath;
    private static string _savesPath;
    private static string _dateTimeFormatString = "ddMMyyyyHHmmss";
 
    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        this._mainCamera = Camera.main;
        _settingsPath = Application.persistentDataPath + "/settings.ecd";
        _savesPath = Application.persistentDataPath + "/Saves";

        //Creates Saves directory and base settings file.
        if (!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        if (!File.Exists(Application.persistentDataPath + "/settings.ecd"))
        {
            File.Create(Application.persistentDataPath + "/settings.ecd");
        }

        LoadAndUpdateSessions();

        //Load settings.
        this._gameSettings = LoadSettings();

        this._sessionToLoadIndex = -1;

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

    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            StartCoroutine(Save());

        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            SaveSettings(this._gameSettings);

        }

        else if (Input.GetKeyDown(KeyCode.F12))
        {
            this._gameSettings = LoadSettings();

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

            this._tempSession = currentSession;
            SaveSession(this._tempSession);

            UnityEngine.Debug.Log("New game created!");

            


        }
        else
        {
            //WE SHOULD NEVER GET HERE!
            UnityEngine.Debug.LogError("GameManager.cs: NewGame() function error!");
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
    public void LoadGame()
    {


        StartCoroutine(LoadSceneWithPlayerProgress(currentSession, loadingScreen, false));

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
        yield return SaveMiniature(value => currentSession.miniatureBytes = value);

        //Using a temporary session for saving.
        this._tempSession = currentSession;

        //Save session with index increased.
        SaveSession(this._tempSession);

        //Re-load all sessions.
        LoadAndUpdateSessions();

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

        blur = this._mainCamera.GetComponent<BlurOptimized>();

        renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        renderTexture.useMipMap = false;
        renderTexture.antiAliasing = 1;

        RenderTexture.active = renderTexture;
        this._mainCamera.targetTexture = renderTexture;

        miniature = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        blur.enabled = false; //Disable blur temporarly just for shot.

        this._mainCamera.Render();
        miniature.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);

        //blur.enabled = true;
        miniature.Apply();

        this._mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        //Return image encoded in byte array.
        result(miniature.EncodeToPNG());

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="session"></param>
    private void SaveSession(GameSession session)
    {
        try
        {
            this.PrepareSessionForSaving(session);

            _savedSessions.Clear();
            _savedSessions.Add(session);

            this._file = File.Create(_savesPath + "/SAV_" + DateTime.Now.ToString(_dateTimeFormatString) + ".ecd");

            this._binFormatter.Serialize(this._file, _savedSessions);

            this._file.Close();

            UnityEngine.Debug.Log("Saved new session -ID: " + _savedSessions[0].ID + "- to path: " + _savesPath + "/SAV_" + DateTime.Now.ToString(_dateTimeFormatString) + ".ecd");

        }
        catch (System.Exception)
        {
            UnityEngine.Debug.LogWarning("Failed to save session.");
            throw;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private GameSession LoadSession(int index)
    {
        DirectoryInfo dir = new DirectoryInfo(_savesPath);
        FileInfo[] fi = dir.GetFiles();
        List<GameSession> tempSessionList;
        GameSession returnGameSession = null;

        int count = fi.Length;

        if (Directory.Exists(_savesPath))
        {
            UnityEngine.Debug.Log("Loading save file...");

            for (int i = 0; i < count; i++)
            {
                string fileName = fi[i].FullName;

                if (File.Exists(fileName))
                {
                    this._file = File.Open(fileName, FileMode.Open);

                    tempSessionList = ((List<GameSession>)this._binFormatter.Deserialize(this._file));

                    if (tempSessionList[0].ID == index)
                    {
                        UnityEngine.Debug.Log("Loaded: " + fileName);
                        returnGameSession = tempSessionList[0];

                        continue;
                    }

                }
                else
                {
                    //WE SHOULD NEVER GET THERE PORCODIO!!!
                    UnityEngine.Debug.LogWarning("Error while loading file: File '" + fileName + "' not found.");
                    break;
                }

            }
            this._file.Close();
        }
        else
        {
            UnityEngine.Debug.LogWarning("Error while searching files: Directory '" + _savesPath + "' not found.");
        }

        return returnGameSession;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private List<GameSession> LoadAllSessions()
    {
        DirectoryInfo dir = new DirectoryInfo(_savesPath);
        FileInfo[] fi = dir.GetFiles();

        List<GameSession> tempSessionList;

        int count = fi.Length;

        _savedSessions.Clear();

        if (Directory.Exists(_savesPath))
        {
            UnityEngine.Debug.Log("Updating save files list...");

            for (int i = 0; i < count; i++)
            {
                string fileName = fi[i].FullName;

                if (File.Exists(fileName))
                {
                    this._file = File.Open(fileName, FileMode.Open);

                    tempSessionList = ((List<GameSession>)this._binFormatter.Deserialize(this._file));
                    _savedSessions.Add(tempSessionList[0]);
                    tempSessionList.Clear();

                    this._file.Close();
                    UnityEngine.Debug.Log("Found: " + fileName);
                }
                else
                {
                    //WE SHOULD NEVER GET THERE PORCODIO!!!
                    UnityEngine.Debug.LogWarning("Error while loading file: File '" + fileName + "' not found.");
                }

            }

            UnityEngine.Debug.Log("Done updating list!");
        }
        else
        {
            UnityEngine.Debug.LogWarning("Error while searching files: Directory '" + _savesPath + "' not found.");
        }

        return _savedSessions;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameSession"></param>
    private void UpdateSession(GameSession gameSession)
    {
        this._tempChar = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();

        //Update player stats.
        ClassMerger.MergeClassProperties(gameSession.character, this._tempChar);

        //Update player position.
        this._tempChar.transform.position = gameSession.character.GetPosition();
        this._tempChar.transform.rotation = Quaternion.Euler(gameSession.character.GetRotation());

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameSession"></param>
    private void PrepareSessionForSaving(GameSession gameSession)
    {
        this._tempChar = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        ClassMerger.MergeClassProperties(this._tempChar, gameSession.character);

        gameSession.lastSaveDate = DateTime.Now.ToString();
        gameSession.ID = this.GetSavesNumber();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settings"></param>
    private void SaveSettings(GameSettings settings)
    {
        this._tempSettings = new SavedSettings();

        ClassMerger.MergeClassProperties(settings, this._tempSettings);

        this._file = File.Create(_settingsPath);

        this._binFormatter.Serialize(this._file, this._tempSettings);

        this._file.Close();

        UnityEngine.Debug.Log("Data settings to path: " + _settingsPath);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private GameSettings LoadSettings()
    {
        GameSettings retSettings = new GameSettings();

        this._tempSettings = null;

        if (File.Exists(_settingsPath))
        {
            this._file = File.Open(_settingsPath, FileMode.Open);
            this._tempSettings = (SavedSettings)this._binFormatter.Deserialize(this._file);
            this._file.Close();

        }
        else
        {
            this._tempSettings = new SavedSettings();
            UnityEngine.Debug.LogWarning("No settings found, default ones loaded");
        }

        ClassMerger.MergeClassProperties(this._tempSettings, retSettings);

        return retSettings;
    }

    /// <summary>
    /// Load scene in background with loading screen.
    /// </summary>
    /// <example>
    /// This could be the possible usage of <see cref="LoadScene(int, LoadingScreen, bool)"/> method.
    /// <code>
    /// StartCoroutine(GameSceneManager.LoadSceneAysnc(0, loadingScreen, false));
    /// </code>
    /// </example>
    /// <param name="sceneIndex">Index of scene to load</param>
    /// <param name="loadingScreen">Loading screen instance</param>
    /// <param name="showProgress">If true, shows the loading text and logo. This should be set to false when loading fast scenes such as house rooms.</param>
    /// <returns></returns>
    private IEnumerator LoadScene(int sceneIndex, LoadingScreen loadingScreen, bool showProgress)
    {
        AsyncOperation asyncOp = null;

        //Wait for fade in effect before loading the scene.
        yield return new WaitForSeconds(0.5f);

        //Start loading scene.
        asyncOp = SceneManager.LoadSceneAsync(sceneIndex);

        //Update progress.
        if (showProgress) //If we are not showing progress we don't need to update it.
        {
            StartCoroutine(UpdateLoadingProgress(loadingScreen, asyncOp));
        }

        //Busy wait.
        while (!asyncOp.isDone)
        {
            yield return new WaitForSeconds(0.1f); 
        }

        yield return new WaitForSeconds(0.5f); //Just to make sure we loaded everything

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <param name="gameSession"></param>
    /// <param name="loadingScreen"></param>
    /// <param name="showProgress"></param>
    /// <returns></returns>
    private IEnumerator LoadSceneWithPlayerProgress(GameSession gameSession, LoadingScreen loadingScreen, bool showProgress)
    {
        //We have to lock player movement/all types of input while loading.
        //player.LockInputs() or maybe player.LockMovement()

        //Disable GUI so that the player understands that inputs are temporarly disabled.
        mainGUI.gameObject.SetActive(false);

        Camera.main.GetComponent<CameraController>().enabled = false;

        loadingScreen.showProgress = showProgress;
        loadingScreen.enabled = true;

        if (gameSession.SceneIndex > -1)
        {
            currentSession = LoadSession(gameSession.SceneIndex);
            this.UpdateSession(currentSession);

        }
        else
        {
            //WE SHOULD NEVER GET HERE!
            UnityEngine.Debug.LogError("GameManager.cs: LoadGame() function error!");
        }


        //Wait for scene to be loaded.
        yield return LoadScene(gameSession.SceneIndex, loadingScreen, showProgress);

        //
        //Now we can update everything
        Transform tempTransform;

        tempTransform = GetPlayerSpawnPosInCurrentScene();

        gameSession.character.SetPosition(tempTransform.position);
        gameSession.character.SetRotation(tempTransform.rotation.eulerAngles);

        UpdateSession(gameSession);

        Camera.main.GetComponent<CameraController>().enabled = true;
        //
        yield return new WaitForSeconds(0.5f);

        //When we finished updating, restore GUI and player movement.
        loadingScreen.enabled = false;

        //Enable GUI so that the player understands that inputs are re-enabled.
        mainGUI.gameObject.SetActive(true);

        //Now we can restore player inputs.
        //player.UnlockInputs() or maybe player.UnlockMovement()
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loadingScreen"></param>
    /// <param name="asyncOp"></param>
    /// <returns></returns>
    private IEnumerator UpdateLoadingProgress(LoadingScreen loadingScreen, AsyncOperation asyncOp)
    {
        while (loadingScreen.enabled)
        {
            if (asyncOp != null && loadingScreen != null)
            {
                loadingScreen.text.text = string.Format("Now Loading... {0}%", asyncOp.progress * 100);
            }
            else
            {
                UnityEngine.Debug.Log("GameManager.cs: Error while updating loading progress, variables 'loadingScreen' and 'asyncOp' can't be null.");
            }

            yield return new WaitForSecondsRealtime(0.05f);

        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private int GetSavesNumber()
    {
        DirectoryInfo dir = new DirectoryInfo(_savesPath);
        FileInfo[] fi = dir.GetFiles();

        return fi.Length;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    private void UpdateSessionToLoadIndex(int index)
    {
        this._sessionToLoadIndex = index;
    }

    /// <summary>
    /// 
    /// </summary>
    private void PopulateSaveList()
    {
        mainGUI.PopulateSaveList(this);

    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadAndUpdateSessions()
    {
        //Resize array lenght according to saves number.
        Array.Resize(ref allSessions, GetSavesNumber());

        //Load all sessions, user will chose wich one to load.
        allSessions = LoadAllSessions().ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Transform GetPlayerSpawnPosInCurrentScene()
    {
       return GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;
    }
}
