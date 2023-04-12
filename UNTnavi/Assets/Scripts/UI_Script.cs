using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour
{
    public GameObject splashscreen;
    public GameObject mainMenuObj;
    public GameObject roomSearchObj;
    public GameObject scheduleObj;

    public Toggle tog;
    public int screenState = 0;

    public Camera ARcam;
    public Camera mapCam;

    void Start()
    {
        mapCam.enabled = true;
        ARcam.enabled = false;

        splashscreen.SetActive(true);
        mainMenuObj.SetActive(true);
        roomSearchObj.SetActive(false);
        scheduleObj.SetActive(false);

        StartCoroutine(SplashFade());
    }

    void Update()
    {
        if (tog.isOn)
        {
            mapCam.enabled = false;
            ARcam.enabled = true; 
        } else {
            mapCam.enabled = true;
            ARcam.enabled = false;
        }
    }

    public void update(int state)
    {
        screenState = state;

        if (screenState == 0)
        {
            mainMenuObj.SetActive(true);
            roomSearchObj.SetActive(false);
            scheduleObj.SetActive(false);
        }
        else if (screenState == 1)
        {
            mainMenuObj.SetActive(false);
            roomSearchObj.SetActive(true);
            scheduleObj.SetActive(false);
        }
        else if (screenState == 2)
        {
            mainMenuObj.SetActive(false);
            roomSearchObj.SetActive(false);
            scheduleObj.SetActive(true);
        }
    }

    IEnumerator SplashFade()
    {
        yield return new WaitForSeconds(.5f);
        var color = splashscreen.GetComponent<CanvasGroup>();
        for (float x = 1; x > 0; x += -.05f)
        {
            color.alpha = x;
            yield return new WaitForSeconds(.005f);
        }
        color.alpha = 0;
        splashscreen.SetActive(false);
    }
}
