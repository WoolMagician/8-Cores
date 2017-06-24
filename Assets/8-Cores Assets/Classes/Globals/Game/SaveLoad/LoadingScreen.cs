using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    
    public Image background;
    public RawImage logo;
    public Text text;

    // Use this for initialization
    void Start()
    {
 
    }

    private void Update()
    {
    
    }

    private void OnEnable()
    {
        background.gameObject.SetActive(true);
        background.canvasRenderer.SetAlpha(0.0f);
        background.CrossFadeAlpha(1.0f, 0.4f, false);
        StartCoroutine(WaitForFadeIn(0.8f));
    }

    private void OnDisable()
    {
        background.CrossFadeAlpha(0.0f, 0.4f, false);
        logo.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        StartCoroutine(WaitForFadeOut(0.45f));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="secondsToWait"></param>
    /// <returns></returns>
    private IEnumerator WaitForFadeIn(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        logo.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="secondsToWait"></param>
    /// <returns></returns>
    private IEnumerator WaitForFadeOut(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        background.gameObject.SetActive(false);
    }


}
