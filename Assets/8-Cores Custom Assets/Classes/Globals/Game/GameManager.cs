using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Manca la parte di scelta sessione da parte dell'utente.
    //Il campo 'currentSessionIndex' deve essere riempito puntando all'ID della sessione scelta dall'utente.
    //Se si crea un nuovo gioco, abilitare la booleana newGame.

    //[HideInInspector]
    private GameSession[] allSessions = new GameSession[5];    //Tutti i salvataggi fatti fin'ora
    private GameSession tempSession;    //Sessione temporanea di appoggio per il salvataggio/caricamento dati.
    private GameSettings gameSettings = new GameSettings();

    private BaseCharacter tempChar;
    //private Inventory tempInventory;   

    public static DataManager dataManager;

    public int sessionToLoadIndex = 0;
    public int currentSessionIndex = 0;
    public bool newGame = false;

    public GameSession currentSession;

    // Use this for initialization
    void Start()
    {
        dataManager = new DataManager(this);
        dataManager.Initialize();

        allSessions = dataManager.LoadAll().ToArray();
        gameSettings = dataManager.LoadSettings();

        if (allSessions.Length > 0)
        {
            //SCELTA DELL'UTENTE

            //SELEZIONE SESSIONE
            currentSession = allSessions[currentSessionIndex];
            currentSession.Update(dataManager);
        }
        else
        {
            currentSession = new GameSession();
            currentSession.savedInventory = new InventoryData(20, 4);
            //NEW GAME NEEDED.
        }

        //tempSession = new GameSession();
        tempChar = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();

        //dataManager.MergeClassProperties(tempChar, tempSession.character);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            tempSession = currentSession;

            dataManager.Save(tempSession);

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
}
