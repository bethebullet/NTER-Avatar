using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public struct ClassScheduleItem{
    string roomNumber;
    string startTime;
    string endTime;
    string className;
    int order;
}

public class ScheduleClass : MonoBehaviour {
    //Have a button you press that adds a class
    //the class has entry boxes for: Class Name, Room #, Start Time, End Time
    //the tab has an edit button and delete button, when in editing mode, hit confirm the confirm changes
    //or just have it auto update every time a keystroke is entered or a text box is unfocused
    //sort classes by start time

    public void SortClasses(){}
    public void InsertNewClass(){}
    
}