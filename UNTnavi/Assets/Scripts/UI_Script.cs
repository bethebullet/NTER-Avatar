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
    public GameObject scavMenu;
    public Image scavButton;
    public Color white => Color.white;
    public Color green => Color.green;

    public Toggle tog;
    public int screenState = 0;
    public bool scavengerHunt = false;
    private int scavState;

    public Camera ARcam;
    public Camera mapCam;

    public bool blink;

    void Start()
    {
        mapCam.enabled = true;
        ARcam.enabled = false;

        splashscreen.SetActive(true);
        mainMenuObj.SetActive(true);
        roomSearchObj.SetActive(false);
        scheduleObj.SetActive(false);
        scavState = 0; //remove this later when player prefs are added

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
        if (blink)
        {
            scavButton.color = Lerp(white, green, 2);
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

    public void scav()
    {
        if (scavengerHunt)
        {
            scavMenu.SetActive(false);
            scavengerHunt = false;
        }
        else
        {
            scavMenu.SetActive(true);
            scavMenu.GetComponent<scavUI>().updateMenu(scavState);
            blink = false;
            scavengerHunt = true;
        }
    }
    public void setScavMenu(int state)
    {
        scavState = state;
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
    public void blinkEffect()
    {
        blink = true;
    }

    public Color Lerp(Color firstColor, Color secondColor, float speed) => Color.Lerp(firstColor, secondColor, Mathf.Sin(Time.time * speed));
}
