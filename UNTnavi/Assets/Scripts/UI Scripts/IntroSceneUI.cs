using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroSceneUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject splashscreen;
    // public GameObject Tutorial;

    void Start()
    {
        StartCoroutine(SplashFade());
        
    }

    IEnumerator SplashFade()
    {
        yield return new WaitForSeconds(4.0f);
        var color = splashscreen.GetComponent<CanvasGroup>();
        for (float x = 1; x > 0; x += -.05f)
        {
            color.alpha = x;
            yield return new WaitForSeconds(.01f);
        }
        color.alpha = 0;
        splashscreen.SetActive(false);
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(0.0f);
        SceneManager.LoadScene (sceneName:"Brandon Map");
    }
}
