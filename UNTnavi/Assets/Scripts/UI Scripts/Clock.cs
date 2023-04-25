using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Clock : MonoBehaviour
{
    public TMP_Text timeTxt;
    string oldSeconds;

    public ScheduleHandler sHandler;
    public ClassScheduleItem nextClass;

    // Update is called once per frame
    void FixedUpdate()
    {
        // string tempTime = System.DateTime.UtcNow.ToString("ss");
        
        // if (tempTime != oldSeconds){
        //     GetTime();   
        // }
        // oldSeconds = tempTime;

        if(CheckClassToday())
        {
            nextClass = PullNextClass();
            // nextClass = new ClassScheduleItem();
            // nextClass.className = "CSCE4902";
            // nextClass.roomNumber = "K120";
            // nextClass.startTime = "2:30pm";

            string timeLeft = GetTimeLeft(nextClass).ToString("0");

            DateTime classtime = DateTime.Parse(nextClass.startTime);
            DateTime minutes = DateTime.Now;
            if (classtime.Subtract(minutes).TotalMinutes < 60)
                timeTxt.text = "Next Class (in " + timeLeft + " min)\n";
            else
                timeTxt.text = "Next Class (in " + timeLeft + " hours)\n";

            timeTxt.text += "> " + nextClass.className + " in " + nextClass.roomNumber;
        } else
        {
            timeTxt.text = "No Classes Today";
        }
    }

    double GetTimeLeft(ClassScheduleItem tClass)
    {
            DateTime classtime = DateTime.Parse(tClass.startTime);
            DateTime minutes = DateTime.Now;

            if (classtime.Subtract(minutes).TotalMinutes < 60)
                return classtime.Subtract(minutes).TotalMinutes;
            return classtime.Subtract(minutes).TotalHours;

    }

    bool CheckClassToday()
    {
        for(int i = 0; i < sHandler.classPartitions.Count; i++)
        {
            var tClass = sHandler.classPartitions[i];
            // if () date
                if (GetTimeLeft(tClass) > 0)
                            return true;
        }
        return false;
    }

    // assumes classes are sorted by time and date
    ClassScheduleItem PullNextClass()
    {
        var nClass = sHandler.classPartitions[0];
        for(int i = 0; i < sHandler.classPartitions.Count; i++)
        {
            nClass = sHandler.classPartitions[i];
            //if() date
                if(GetTimeLeft(nClass) < 0)
                    continue;
                else
                    break;
        }
        return nClass; 
    }

    void GetTime()
    {
        string seconds = System.DateTime.UtcNow.ToString("ss");
        string minutes = System.DateTime.UtcNow.ToString("mm");
        string hours = System.DateTime.UtcNow.ToLocalTime().ToString("hh");

        string tempTime = hours + " : " + minutes + " : " + seconds;

        // timeTxt.text = tempTime;


    }

}
