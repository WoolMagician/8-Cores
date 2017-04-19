using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneDoorTrigger : MonoBehaviour {
    private float currentAmount = 0f;
    private float maxAmount = 5f;
    private bool flag2 = false;

    public float overlayAlpha = 200.0f;

    [HideInInspector]
    public bool flag = false;

    [HideInInspector]
    public Vector2 size;

    //public Object Player;
    [Tooltip("Index of scene to load")]
    public int sceneToLoad;

    [Tooltip("Index of scene to unload")]
    public int sceneToUnload;

    public Texture2D texture;
    public Texture2D overlay;
    public Texture2D round;

    public GameObject Sphere1;
    public GameObject Sphere2;
    public GameObject Sphere3;
    public GameObject Overlay;

    public GameObject[] Spheres;

    private int currentSphereID = 1;

    public float roundWidth = 150f;
    public float roundHeight = 150f;

    public string keyToPress = "F";
    public string text = "";
    public int textureWidth = 400;
    public int textureHeight = 100;
    public float texturePosX = 100;
    public float texturePosY = 100;

    [Tooltip("Overrides texturePosX")]
    public bool centerX = true;

    public GUIStyle style;

    private void Start()
    {

        text = string.Concat("Press [", keyToPress.Trim().ToUpper(), "] to enter");

        size = style.CalcSize(new GUIContent(text));

        if (centerX)
        {
            texturePosX = (Screen.width / 2) - (textureWidth / 2);
        }
    }

    private void Update()
    {

        if (Input.GetButtonDown("PS4_TRIANGLE"))
        {
            //flag2 = true;
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.01f;
            else

                Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.001f * Time.timeScale;
            Sphere1.SetActive(true);
            Sphere2.SetActive(true);
            Sphere3.SetActive(true);
            Overlay.SetActive(true);

        }

        if (Input.GetButtonUp("PS4_TRIANGLE"))
        {
            //flag2 = false;
            Time.timeScale = 1.0f;
            Time.fixedDeltaTime = 0.001f * Time.timeScale;
            Sphere1.SetActive(false);
            Sphere2.SetActive(false);
            Sphere3.SetActive(false);
            Overlay.SetActive(false);

        }
        currentSphereID = 0;
        if (Input.GetButton("PS4_TRIANGLE"))
        {
            if(Input.GetButtonDown("PS4_CIRCLE"))
            {
                currentSphereID += 1;

                Spheres[currentSphereID].transform.position = new Vector3(Spheres[currentSphereID].transform.position.x, Spheres[currentSphereID].transform.position.y, Spheres[currentSphereID].transform.position.z - 1f);

            }
            
        }
        if (Time.timeScale == 0.01f)
        {

            currentAmount += Time.deltaTime;
        }

        if (currentAmount > maxAmount)
        {

            currentAmount = 0f;
            Time.timeScale = 1.0f;

        }
    }

    void OnTriggerStay(Collider other)
    {
        flag = true;

        if (Input.GetKeyDown(keyToPress.Trim().ToLower()) && other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        flag = false;
    }

    private void OnGUI()
    {
        if (flag)
        {
            GUI.DrawTexture(new Rect(texturePosX, texturePosY, textureWidth, textureHeight), texture);
            GUI.Label(new Rect(((textureWidth / 2) - (size.x / 2)) + texturePosX, (textureHeight / 2) - (size.y / 2), size.x, size.y ), text, style);
        }
        if (flag2)
        {
            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, overlayAlpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), overlay);

            GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 255f);
            GUI.DrawTexture(new Rect((Screen.width / 2) - (roundWidth / 2), (Screen.height / 2) - (roundHeight / 2), roundWidth, roundHeight), round, ScaleMode.ScaleToFit, true, 0f);

            GUI.color = new Color(255f, 255f, 255f, 0.5f);
            GUI.DrawTexture(new Rect((Screen.width / 2) - (roundWidth * 0.7f / 2) + 150, (Screen.height / 2) - (roundHeight * 0.7f / 2), roundWidth * 0.7f, roundHeight * 0.7f), round, ScaleMode.ScaleToFit, true, 0f);

            GUI.color = new Color(255f, 255f, 255f, 0.5f);
            GUI.DrawTexture(new Rect((Screen.width / 2) - (roundWidth * 0.7f / 2) - 150, (Screen.height / 2) - (roundHeight * 0.7f / 2), roundWidth * 0.7f, roundHeight * 0.7f), round, ScaleMode.ScaleToFit, true, 0f);

        }
    }

}

