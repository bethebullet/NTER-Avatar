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
    public Color yellow => Color.yellow;

    public Toggle tog;
    public int screenState = 0;
    public bool scavengerHunt = false;
    private int scavState;

    public Camera ARcam;
    public Camera mapCam;
    public GameObject player;

    public GameObject marker;
    public Pathmaker pm;

    public bool blink;

    public int xpad;
    public int ypad;

    void Start()
    {
        mapCam.enabled = true;
        ARcam.enabled = false;

        splashscreen.SetActive(true);
        mainMenuObj.SetActive(true);
        roomSearchObj.SetActive(false);
        scheduleObj.SetActive(false);
        scavState = 0; //remove this later when player prefs are added


        xpad = 120;
        ypad = 380;
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
            scavButton.color = Lerp(white, yellow, 2);
        }

        var markPos = marker.transform.position;
        mapCam.transform.position = player.transform.position + new Vector3(0,20,0);
        marker.transform.position = ARcam.WorldToScreenPoint(pm.target);
        if(marker.transform.position.x > Screen.width - xpad)
            marker.transform.position = new Vector3(Screen.width - xpad, markPos.y, markPos.z);
        if(marker.transform.position.x < 0 + xpad)
            marker.transform.position = new Vector3(xpad, markPos.y, markPos.z);
        if(marker.transform.position.y > Screen.height - ypad)
            marker.transform.position = new Vector3(markPos.x, Screen.height - ypad, markPos.z);
        if(marker.transform.position.y < 0 + ypad)
            marker.transform.position = new Vector3(markPos.x, ypad, markPos.z);


        
        marker.transform.right = new Vector3(Screen.width/2, Screen.height/2, 0) - marker.transform.position;
        marker.transform.rotation *= Quaternion.Euler(0,0,90);
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
            scavButton.color = white;
            blink = false;
        }
        else
        {
            scavMenu.SetActive(true);
            scavMenu.GetComponent<scavUI>().updateMenu(scavState);
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
