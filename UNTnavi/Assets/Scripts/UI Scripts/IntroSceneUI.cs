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
    public GameObject Tutorial;

    void Start()
    {
        StartCoroutine(SplashFade());
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SplashFade()
    {
        yield return new WaitForSeconds(1.0f);
        var color = splashscreen.GetComponent<CanvasGroup>();
        for (float x = 1; x > 0; x += -.05f)
        {
            color.alpha = x;
            yield return new WaitForSeconds(.01f);
        }
        color.alpha = 0;
        splashscreen.SetActive(false);
        Tutorial.SetActive(true);
    }
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(8.0f);
        SceneManager.LoadScene (sceneName:"Brandon Map");
    }
}
