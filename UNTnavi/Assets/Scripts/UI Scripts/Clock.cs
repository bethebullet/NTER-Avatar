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

            if (CheckInClass(nextClass))
            {
                timeTxt.text = "Class in Session\n";
            } else 
            {
                if (classtime.Subtract(minutes).TotalMinutes < 60)
                    timeTxt.text = "Next Class (in " + timeLeft + " min)\n";
                else
                    timeTxt.text = "Next Class (in " + timeLeft + " hours)\n";
            }
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


    bool CheckInClass(ClassScheduleItem tClass)
    {
        DateTime minutes = DateTime.Now;
        DateTime classtime = DateTime.Parse(tClass.startTime);
        DateTime endtime = DateTime.Parse(tClass.endTime);
        double start = classtime.Subtract(minutes).TotalMinutes;
        double end = endtime.Subtract(minutes).TotalMinutes;

        if (start < 0 && end > 0)
            return true;
        else
            return false;    
    }

    public int GetDayofWeek()
    {
        DateTime today= DateTime.Now;
        if(today.DayOfWeek.ToString() == "Monday")
            return 0;
        if(today.DayOfWeek.ToString() == "Tuesday")
            return 1;
        if(today.DayOfWeek.ToString() == "Wednesday")
            return 2;
        if(today.DayOfWeek.ToString() == "Thursday")
            return 3;
        if(today.DayOfWeek.ToString() == "Friday")
            return 4;
        if(today.DayOfWeek.ToString() == "Saturday")
            return 5;
        return -1;
    }

    bool CheckClassToday()
    {
        for(int i = 0; i < sHandler.classPartitions.Count; i++)
        {
            var tClass = sHandler.classPartitions[i];
            if(GetDayofWeek() < 0)
                return false;
            if (tClass.weekDays[GetDayofWeek()])
                if (GetTimeLeft(tClass) > 0 || CheckInClass(tClass))
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
            if(GetDayofWeek() < 0)
                return nClass;
            if (nClass.weekDays[GetDayofWeek()])
                if(GetTimeLeft(nClass) < 0 || CheckInClass(nClass))
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
