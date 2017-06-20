using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    //Manca la parte di scelta sessione da parte dell'utente.
    //Il campo 'currentSessionIndex' deve essere riempito puntando all'ID della sessione scelta dall'utente.
    //Se si crea un nuovo gioco, abilitare la booleana newGame.

    //[HideInInspector]
    private GameSession[] allSessions = new GameSession[5];    //Tutti i salvataggi fatti fin'ora
    private GameSession tempSession;    //Sessione temporanea di appoggio per il salvataggio/caricamento dati.
    private GameSettings gameSettings = new GameSettings();
    public Texture2D tex;

    private BaseCharacter tempChar;
    public static DataManager dataManager;

    public int sessionToLoadIndex = 0;

    public GameSession currentSession;

    // Use this for initialization
    void Start()
    {
        dataManager = new DataManager(this);

        //Creates Saves directory and base settings file.
        dataManager.Initialize();

        //Load all sessions, user will chose wich one to load.
        allSessions = dataManager.LoadAll().ToArray();

        //Load settings.
        gameSettings = dataManager.LoadSettings();

        //Check if there are any save files and let user chose.
        if (allSessions.Length > 0)
        {
            //SCELTA DELL'UTENTE

            //SELEZIONE SESSIONE
            currentSession = allSessions[sessionToLoadIndex];
            currentSession.Update(dataManager);
        }
        else
        {
            //NEW GAME NEEDED.
            NewGame();

        }

        tempChar = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();

        Debug.Log("Current loaded session: SAV_" + currentSession.creationDate.ToString("ddMMyyyyHHmmss") + ".ecd");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            //Use tempSession for saving.
            tempSession = currentSession;

            //Save session with index increased.
            dataManager.Save(tempSession);

            //Update sessions list.
            allSessions = dataManager.LoadAll().ToArray();

        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            currentSession = dataManager.Load(sessionToLoadIndex);
            currentSession.Update(dataManager);

        }

        if(Input.GetKeyDown(KeyCode.F11))
        {
            dataManager.SaveSettings(gameSettings);

        }
        else if(Input.GetKeyDown(KeyCode.F12))
        {
            gameSettings = dataManager.LoadSettings();

        }

    }

    private void NewGame()
    {
        currentSession = new GameSession();
        currentSession.ID = 0;
        currentSession.creationDate = DateTime.Now;
        currentSession.inventory = new InventoryData(20, 4);

        tempSession = currentSession;
        dataManager.Save(tempSession);

        Debug.Log("New game created!");
    }
}
