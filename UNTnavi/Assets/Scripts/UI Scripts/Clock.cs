using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    public TMP_Text time;
    string oldSeconds;

    // Update is called once per frame
    void Update()
    {
        string tempTime = System.DateTime.UtcNow.ToString("ss");
        
        if (tempTime != oldSeconds){
            UpdateTimer();   
        }
        oldSeconds = tempTime;
    }

    void UpdateTimer()
    {
        string seconds = System.DateTime.UtcNow.ToString("ss");
        string minutes = System.DateTime.UtcNow.ToString("mm");
        string hours = System.DateTime.UtcNow.ToLocalTime().ToString("hh");

        string tempTime = hours + " : " + minutes + " : " + seconds;

        time.text = tempTime;
    }

}
