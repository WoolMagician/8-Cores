using UnityEngine;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    //Manca la parte di scelta sessione da parte dell'utente.
    //Il campo 'currentSessionIndex' deve essere riempito puntando all'ID della sessione scelta dall'utente.
    //Se si crea un nuovo gioco, abilitare la booleana newGame.

    [HideInInspector]
    public GameSession[] allSessions = new GameSession[5];    //Tutti i salvataggi fatti fin'ora
    public GameSession currentSession;
    public GameSession tempSession;    //Sessione temporanea di appoggio per il salvataggio/caricamento dati.
    public DataManager dataManager;
    public GameSettings gameSettings = new GameSettings();

    private BaseCharacter tempChar;
    public int sessionToLoadIndex = 0;
    public int currentSessionIndex = 0;
    public bool newGame = false;

    // Use this for initialization
    void Start()
    {
        dataManager = new DataManager(this);
        allSessions = dataManager.Initialize();

        if (allSessions.Length > 0)
        {
            //SCELTA DELL'UTENTE

            //SELEZIONE SESSIONE
            currentSession = allSessions[currentSessionIndex];
        }
        else
        {
            //NEW GAME NEEDED.
        }

        //tempSession = new GameSession();
        //tempChar = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();

        //dataManager.MergeClassProperties(tempChar, tempSession.character);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            dataManager.Save(tempSession);

        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            tempSession = dataManager.Load(sessionToLoadIndex);

            dataManager.MergeClassProperties(tempSession.character, tempChar);

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
