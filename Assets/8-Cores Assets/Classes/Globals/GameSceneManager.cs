using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Diagnostics;

//public struct GameSceneManager {
//    //private float currentAmount = 0f;
//    //private float maxAmount = 5f;
//    //private bool flag2 = false;

//    //public float overlayAlpha = 200.0f;

//    //[HideInInspector]
//    //public bool flag = false;

//    [HideInInspector]
//    public Vector2 size;

//    //public Object Player;
//    [Tooltip("Index of scene to load")]
//    public int sceneToLoad;

//    [Tooltip("Index of scene to unload")]
//    public int sceneToUnload;

//    public Texture2D texture;
//    public Texture2D overlay;
//    public Texture2D round;

//    //public float roundWidth = 150f;
//    //public float roundHeight = 150f;

//    //public string keyToPress = "F";
//    //public string text = "";
//    //public int textureWidth = 400;
//    //public int textureHeight = 100;
//    //public float texturePosX = 100;
//    //public float texturePosY = 100;

//    //[Tooltip("Overrides texturePosX")]
//    //public bool centerX = true;

//    public GUIStyle style;

//    //private void Start()
//    //{

//    //    text = string.Concat("Press [", keyToPress.Trim().ToUpper(), "] to enter");

//    //    size = style.CalcSize(new GUIContent(text));

//    //    if (centerX)
//    //    {
//    //        texturePosX = (Screen.width / 2) - (textureWidth / 2);
//    //    }
//    //}

//    //private void Update()
//    //{
//    //}

//    //void OnTriggerStay(Collider other)
//    //{
//    //    flag = true;

//    //    if (Input.GetKeyDown(keyToPress.Trim().ToLower()) && other.gameObject.tag == "Player")
//    //    {

//    //    }
//    //}
//    private static float _loadingProgress;
//    private static bool _doneLoading;

//    public static IEnumerator CoroutineLoadAsync(int sceneIndex)
//    {
//        AsyncOperation asyncOp;
//        Stopwatch stopWatch;

//        stopWatch = new Stopwatch();
//        stopWatch.Start();

//        asyncOp = SceneManager.LoadSceneAsync(sceneIndex);

//        _loadingProgress = 0f;

//        while (!asyncOp.isDone)
//        {
//            _loadingProgress = asyncOp.progress;
//        }

//        _doneLoading = asyncOp.isDone;

//        stopWatch.Stop();

//        UnityEngine.Debug.Log(string.Format("Loaded scene in {0} ms", stopWatch.ElapsedMilliseconds));

//        yield return null;
//    }

//    public static float progress
//    {
//        get
//        {
//            return _loadingProgress;
//        }
//    }

//    public static bool doneLoading
//    {
//        get
//        {
//            return _doneLoading;
//        }
//    }

//}

