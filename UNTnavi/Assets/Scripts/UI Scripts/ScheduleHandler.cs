using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public struct ClassScheduleItem{ // Can be saved to a file
    public string roomNumber;
    public string startTime;
    public string endTime;
    public string className;
    public int index;
    public List<bool> weekDays;

    public void SetIndex(int x)
    {
        index = x;
    }
}

public struct ClassObject{ // Unity Object
    public int index;
    public GameObject label;
    public bool valid;

    public void SetIndex(int x)
    {
        index = x;
    }

    public void SetLabel(GameObject x)
    {
        label = x;
    }
}

public class ScheduleHandler : MonoBehaviour
{
    //Have a button you press that adds a class
    //the class has entry boxes for: Class Name, Room #, Start Time, End Time
    //the tab has an edit button and delete button, when in editing mode, hit confirm the confirm changes
    //or just have it auto update every time a keystroke is entered or a text box is unfocused
    //sort classes by start time
    [SerializeField] private GameObject scheduleTemplate;
    [SerializeField] private Transform TemplateContainer;
    public List<ClassScheduleItem> classPartitions;
    public List<ClassObject> classObjects;
    private int MAX_CLASSES_ALLOWED = 8;
    private int classCount = 0;
    private int hasLoaded = 0;

    private DateTime parsedTime;

    void Start(){
        /*float width = container.sizeDelta.x;
        float height = container.sizeDelta.y;
        GridLayoutGroup grid = TemplateContainer.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(width*.95f,height*.95f/8);
        print(grid.cellSize);
        grid.spacing = new Vector2(0.0f,height*.05f/8);*/
        //grid.Spacing.Y = height*.05/8;

        // Debug.Log(Application.persistentDataPath);
        // Debug.Log(File.Exists(Application.persistentDataPath + "/schedule.bin"));
        classPartitions = new List<ClassScheduleItem>();
        classObjects = new List<ClassObject>();

        LoadSchedule();
        // BackCheck();
    }

    public void SortClasses(){
        //classObjects = classObjects.OrderBy(classObjects=> int.TryParse(classObjects.index.startTime)).ToList();
        int newListLength = 0;
        int i = 0;
        bool notUsed = true;
        int minTime = 200000;
        int minIndex = 200000;
        int loopCount = 0;
        List<int> newClass = new List<int>();
        List<ClassScheduleItem> newClassPartitions = new List<ClassScheduleItem>();
        List<ClassObject> newClassObjects = new List<ClassObject>();
        if (classPartitions.Count != 0)
        {
            while (newListLength != classCount)
            {
                if (newClass.Count != 0)
                {
                    foreach (int j in newClass)
                    {
                        if ( i == j)
                        {
                            notUsed = false;
                        }
                    }
                }
                if (notUsed)
                {
                    
                    int hourTime = 0; 
                    int minuteTime = 0;
                    int tempTime = 100000;
                    int colon =  classPartitions[i].startTime.IndexOf(":");
                    int amPM = classPartitions[i].startTime.IndexOf("M") - 2;
                    bool success1 = int.TryParse(classPartitions[i].startTime.Substring(0, colon), out hourTime);
                    bool success2 = int.TryParse(classPartitions[i].startTime.Substring(colon+1, amPM - colon-1), out minuteTime);
                    if (success1 && success2)
                    {
                        string tod = classPartitions[i].startTime.Substring(amPM+1, 2);
                        if (tod.Equals("PM") && hourTime != 12)
                        {
                        minuteTime = minuteTime + 720;
                        }
                        if (tod.Equals("AM") && hourTime == 12)
                        {    
                        hourTime = 0;
                        }
                        hourTime = hourTime * 60;
                        tempTime = hourTime + minuteTime;

                        if (tempTime < minTime)
                        {
                            minTime = tempTime;
                            minIndex = i;
                        }
                    }
                }
                i++;
                notUsed = true;
                if (i == classCount)
                {
                    classPartitions[minIndex].SetIndex(newClassPartitions.Count);
                    classObjects[minIndex].SetIndex(newClassObjects.Count);
                    newClassPartitions.Add(classPartitions[minIndex]);
                    newClassObjects.Add(classObjects[minIndex]);
                    newClass.Add(minIndex);
                    i = 0;
                    loopCount++;
                    newListLength++;
                    Debug.Log(minIndex);
                    minIndex = 10000;
                    minTime = 1000000;
                }
            }
            i = 0;
            foreach(ClassScheduleItem item in newClassPartitions)
            {
                classPartitions[i] = item;
                i++;
            }
            i = 0;
            foreach(ClassObject item in newClassObjects)
            {
                classObjects[i] = item;
                i++;
            }
            for (int j = 0; j < classPartitions.Count; j++)
            {
                SaveAll(j, 0);
            }
        }
    } // sort classes by schedule start time and set their order index
    public void sortByDays()
    {
        // go through each day
        // if not on new lists add
        // at end of the week, recombine and refresh

        List<int> newClass = new List<int>();
        List<ClassScheduleItem> newClassPartitions = new List<ClassScheduleItem>();
        List<ClassObject> newClassObjects = new List<ClassObject>();

        if (classPartitions.Count != 0)
        {
            for (int i =0; i < 6; i++)
            {
                for(int j = 0; j < classPartitions.Count; j++)
                {
                    if (classPartitions[j].weekDays[i])
                    {
                        if (notUsed(newClass, j))
                        {
                            classPartitions[j].SetIndex(newClassPartitions.Count);
                            classObjects[j].SetIndex(newClassObjects.Count);
                            newClassPartitions.Add(classPartitions[j]);
                            newClassObjects.Add(classObjects[j]);
                            newClass.Add(j);
                        }
                    }
                }
            }
            int k = 0;
            foreach(ClassScheduleItem item in newClassPartitions)
            {
                classPartitions[k] = item;
                k++;
            }
            k = 0;
            foreach(ClassObject item in newClassObjects)
            {
                classObjects[k] = item;
                k++;
            }
            for (int j = 0; j < classPartitions.Count; j++)
            {
                SaveAll(j, 0);
            }
            
            Refresh();
        }
    }
    bool notUsed(List<int> newClass, int idx)
    {
        bool notUsed = true;

        if (newClass.Count != 0)
        {
            foreach (int j in newClass)
            {
                if ( idx == j)
                {
                    notUsed = false;
                }
            }
        }
        return notUsed;
    }

    public void NewClassTemplate(){
        if(classCount >= MAX_CLASSES_ALLOWED){return;}//dont add any more if they have 8
        ClassScheduleItem item = new ClassScheduleItem();
        ClassObject classObj = new ClassObject();
        var templateCopy = Instantiate(scheduleTemplate);
        RectTransform rt = templateCopy.GetComponent<RectTransform>();
        templateCopy.transform.SetParent(TemplateContainer,false);
        rt.localScale = new Vector3(1,1,1);

        item.index = classCount;
        item.weekDays = new List<bool>();
        for (int i = 0; i < 6; i++)
            item.weekDays.Add(false);
        classObj.index = classCount;
        classObj.label = templateCopy;
        classObj.valid = false;


        classPartitions.Add(item);
        classObjects.Add(classObj);

        Transform deleteButton = templateCopy.transform.Find("DeleteButton");
        Button btn = deleteButton.GetComponent<Button>();
        btn.onClick.AddListener(delegate{DeleteClassTemplate(item.index);});

        Transform nameInput = templateCopy.transform.Find("ClassNameInput");
        TMP_InputField name = nameInput.GetComponent<TMP_InputField>();
        name.onEndEdit.AddListener(delegate{SaveAll(item.index, 1);});

        Transform startInput = templateCopy.transform.Find("StartInput");
        TMP_InputField start = startInput.GetComponent<TMP_InputField>();
        start.onEndEdit.AddListener(delegate{SaveAll(item.index, 2);});

        Transform endInput = templateCopy.transform.Find("EndInput");
        TMP_InputField end = endInput.GetComponent<TMP_InputField>();
        end.onEndEdit.AddListener(delegate{SaveAll(item.index, 3);});

        Transform roomInput = templateCopy.transform.Find("RoomInput");
        TMP_InputField room = roomInput.GetComponent<TMP_InputField>();
        
        room.onEndEdit.AddListener(delegate{SaveAll(item.index, 4);});
        room.onValueChanged.AddListener(delegate{SearchRoom(item.index,room.text);});


        Toggle[] weekTog = new Toggle[6]; 
        weekTog[0] = templateCopy.transform.Find("Mtoggle").GetComponent<Toggle>();
        weekTog[1] = templateCopy.transform.Find("Ttoggle").GetComponent<Toggle>();
        weekTog[2] = templateCopy.transform.Find("Wtoggle").GetComponent<Toggle>();
        weekTog[3] = templateCopy.transform.Find("Thtoggle").GetComponent<Toggle>();
        weekTog[4] = templateCopy.transform.Find("Ftoggle").GetComponent<Toggle>();
        weekTog[5] = templateCopy.transform.Find("Stoggle").GetComponent<Toggle>();
        for(int j = 0; j < weekTog.Length; j++)
            weekTog[j].onValueChanged.AddListener(delegate{SaveAll(item.index, 0);}); 

        classCount++;
    }
    public void DeleteClassTemplate(int idx){
        //add an "Are you sure you want to delete this class" popup
        //find the template in the classPartitions list and remove it
        //delete the associated data with it
        //reset the order of all the elements afterwards
        for (int i = 0; i<classPartitions.Count;i++){
            ClassScheduleItem item = classPartitions[i];
            if(item.index == idx){
                // Destroy(item);
                classPartitions.RemoveAt(i);
                classCount--;
            }else{
                if(item.index > idx){
                    item.index -= 1;
                }
            }
        }
        for (int i = 0; i<classObjects.Count;i++){
            ClassObject classObj = classObjects[i];
            if(classObj.index == idx){
                Destroy(classObjects[i].label);
                classObjects.RemoveAt(i);
            }else{
                if(classObj.index > idx){
                    classObjects[i].SetIndex(classObjects[i].index - 1);
                }
            }
        }
        //SortClasses();
    }

    public void BackCheck()
    {
        for (int i = 0; i<classPartitions.Count;i++){
            ClassObject thisClass = classObjects[i];
            if(!thisClass.valid)
                DeleteClassTemplate(i);
        }
        for (int j = 0; j < classPartitions.Count; j++)
        {
            SaveAll(j, 0);
        }
        if (classPartitions.Count == 0)
            SaveSchedule();
        SortClasses();
        sortByDays();
    }

    public void DeleteAll()
    {
        int tempClasses = classPartitions.Count;
        for (int i = 0; i < tempClasses; i++){
            Destroy(classObjects[i].label);
        }

        classPartitions.Clear();
        classObjects.Clear();
    }

    public void Refresh()
    {

        LoadSchedule();
        // GameObject[] classObjArr = GameObject.FindGameObjectsWithTag("Class");
        // List<GameObject> classObjs = new List<GameObject>();
        // foreach(GameObject classO in classObjArr)
        //     classObjs.Add(classO);
        
        // classObjs = classObjs.OrderBy(classObjs=> DateTime.Parse(classObjs.transform.Find("ClassNameInput").GetComponent<TMP_InputField>().text)).ToList();

        // Debug.Log(classObjs.Count);
        // for (int i = 0; i < classObjs.Count;i++){
        //     ClassObject thisClass = classObjects[i];
        //     ClassScheduleItem item = classPartitions[i];

        //     GameObject templateCopy = thisClass.label;

        //     Transform nameInput = templateCopy.transform.Find("ClassNameInput");
        //     TMP_InputField name = nameInput.GetComponent<TMP_InputField>();
        //     name.text = item.className;

        //     Transform startInput = templateCopy.transform.Find("StartInput");
        //     TMP_InputField start = startInput.GetComponent<TMP_InputField>();
        //     start.text = item.startTime;

        //     Transform endInput = templateCopy.transform.Find("EndInput");
        //     TMP_InputField end = endInput.GetComponent<TMP_InputField>();
        //     end.text = item.endTime;

        //     Transform roomInput = templateCopy.transform.Find("RoomInput");
        //     TMP_InputField room = roomInput.GetComponent<TMP_InputField>();
        //     room.text = item.roomNumber; 

        //     classObjs[i] = templateCopy;
        // }
    }

    public void SaveAll(int idx, int editMode){
        for (int i = 0; i<classPartitions.Count;i++){
            ClassObject thisClass = classObjects[idx];
            if(idx == i){ // && thisClass.valid == true
                ClassScheduleItem item = classPartitions[idx];
                ClassScheduleItem updatedClass = new ClassScheduleItem();
                updatedClass.index = idx;
                updatedClass.weekDays = new List<bool>();
                for (int j = 0; j < 6; j++)
                    updatedClass.weekDays.Add(false);
                
                GameObject templateCopy = thisClass.label;
                Transform nameInput = templateCopy.transform.Find("ClassNameInput");
                TMP_InputField name = nameInput.GetComponent<TMP_InputField>();
                Outline outlineName = thisClass.label.transform.Find("ClassNameInput").GetComponent<Outline>();
                if(editMode == 1)
                {
                    if (name.text != string.Empty)
                    {
                        outlineName.effectColor = new Color(0.0f,0.0f,0.0f,0.0f);
                        thisClass.valid = true;
                    }
                    else
                    {
                        outlineName.effectColor = new Color(1.0f,0.0f,0.0f,1.0f);
                        thisClass.valid = false;
                    }
                }
                updatedClass.className = name.text;

                Transform startInput = templateCopy.transform.Find("StartInput");
                TMP_InputField start = startInput.GetComponent<TMP_InputField>();
                Outline outlineStart = thisClass.label.transform.Find("StartInput").GetComponent<Outline>();
                if(editMode == 2)
                {
                    if (DateTime.TryParse(start.text, out parsedTime))
                    {
                        outlineStart.effectColor = new Color(0.0f,0.0f,0.0f,0.0f);
                        start.text = parsedTime.ToString("h:mm tt");
                        thisClass.valid = true;
                    }
                    else
                    {
                        outlineStart.effectColor = new Color(1.0f,0.0f,0.0f,1.0f);
                        start.text = string.Empty;
                        thisClass.valid = false;
                    }
                }
                updatedClass.startTime = start.text;

                Transform endInput = templateCopy.transform.Find("EndInput");
                TMP_InputField end = endInput.GetComponent<TMP_InputField>();
                Outline outlineEnd = thisClass.label.transform.Find("EndInput").GetComponent<Outline>();
                if(editMode == 3)
                {
                    if (DateTime.TryParse(end.text, out parsedTime))
                    {
                        outlineEnd.effectColor = new Color(0.0f,0.0f,0.0f,0.0f);
                        end.text = parsedTime.ToString("h:mm tt");
                        thisClass.valid = true;
                    }
                    else
                    {
                        outlineEnd.effectColor = new Color(1.0f,0.0f,0.0f,1.0f);
                        end.text = string.Empty;
                        thisClass.valid = false;
                    }
                }
                updatedClass.endTime = end.text;

                Transform roomInput = templateCopy.transform.Find("RoomInput");
                TMP_InputField room = roomInput.GetComponent<TMP_InputField>();
                updatedClass.roomNumber = room.text;

                Toggle[] weekTog = new Toggle[6]; 
                weekTog[0] = templateCopy.transform.Find("Mtoggle").GetComponent<Toggle>();
                weekTog[1] = templateCopy.transform.Find("Ttoggle").GetComponent<Toggle>();
                weekTog[2] = templateCopy.transform.Find("Wtoggle").GetComponent<Toggle>();
                weekTog[3] = templateCopy.transform.Find("Thtoggle").GetComponent<Toggle>();
                weekTog[4] = templateCopy.transform.Find("Ftoggle").GetComponent<Toggle>();
                weekTog[5] = templateCopy.transform.Find("Stoggle").GetComponent<Toggle>();

                for(int j = 0; j < weekTog.Length; j++)
                {
                    updatedClass.weekDays[j] = weekTog[j].isOn;
                }

                classPartitions[idx] = updatedClass;
                Debug.Log(classPartitions[idx].className);
                Debug.Log(thisClass.valid);
                if(thisClass.valid)
                {
                    Debug.Log("valid class saved");
                    
                    SaveSchedule();
                }
                break;
            }
        }
    }


    public void SaveSchedule () {
      BinaryFormatter formatter = new BinaryFormatter();
      string path = Application.persistentDataPath + "/schedule.bin";
      FileStream stream = new FileStream(path, FileMode.Create);

      formatter.Serialize(stream, classPartitions);

      Debug.Log("Schedule has been saved.");

      stream.Close();
    }

    public void LoadSchedule() {
        DeleteAll();
        Debug.Log("Loading schedule.");
        string path = Application.persistentDataPath + "/schedule.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            classPartitions = (List<ClassScheduleItem>) formatter.Deserialize(stream);
            stream.Close();

            foreach(ClassScheduleItem tClass in classPartitions)
                if(tClass.weekDays == null){
                    Debug.Log("Old schedule obselete. Create new classes.");
                    classPartitions.Clear();
                    break;
                }

            classCount = classPartitions.Count;

            //Test
            Debug.Log("Number of classes in the schedule: " + classCount);
            for (int i = 0; i<classPartitions.Count;i++){
                ClassScheduleItem item = classPartitions[i];
                Debug.Log("Class name: " + item.className);
            }

            classObjects = new List<ClassObject>();
            // Populate list of unity objects
            PopulateObjects();
        }

        
    }

    public void PopulateObjects() {
        for (int i = 0; i<classPartitions.Count;i++){
            ClassScheduleItem item = classPartitions[i];
            ClassObject classObj = new ClassObject();
            var templateCopy = Instantiate(scheduleTemplate);
            RectTransform rt = templateCopy.GetComponent<RectTransform>();
            templateCopy.transform.SetParent(TemplateContainer,false);
            rt.localScale = new Vector3(1,1,1);

            classObj.index = item.index;
            classObj.label = templateCopy;
            classObj.valid = true;
            classObjects.Add(classObj);

            // Debug.Log(item.className);

            Transform deleteButton = templateCopy.transform.Find("DeleteButton");
            Button btn = deleteButton.GetComponent<Button>();
            btn.onClick.AddListener(delegate{DeleteClassTemplate(item.index);});

            Transform nameInput = templateCopy.transform.Find("ClassNameInput");
            TMP_InputField name = nameInput.GetComponent<TMP_InputField>();
            name.text = item.className;
            // name.text = "Cybersecurity";
            name.onEndEdit.AddListener(delegate{SaveAll(item.index, 1);});

            Transform startInput = templateCopy.transform.Find("StartInput");
            TMP_InputField start = startInput.GetComponent<TMP_InputField>();
            start.text = item.startTime;
            // start.text = "9:30";
            start.onEndEdit.AddListener(delegate{SaveAll(item.index, 2);});

            Transform endInput = templateCopy.transform.Find("EndInput");
            TMP_InputField end = endInput.GetComponent<TMP_InputField>();
            end.text = item.endTime;
            // end.text = "10:50";
            end.onEndEdit.AddListener(delegate{SaveAll(item.index, 3);});

            Transform roomInput = templateCopy.transform.Find("RoomInput");
            TMP_InputField room = roomInput.GetComponent<TMP_InputField>();
            room.text =  item.roomNumber;
            // room.text = "B190";
            room.onEndEdit.AddListener(delegate{SearchRoom(item.index,room.text); SaveAll(item.index, 4);});
            room.onValueChanged.AddListener(delegate{SearchRoom(item.index,room.text);});

            Toggle[] weekTog = new Toggle[6]; 
            weekTog[0] = templateCopy.transform.Find("Mtoggle").GetComponent<Toggle>();
            weekTog[1] = templateCopy.transform.Find("Ttoggle").GetComponent<Toggle>();
            weekTog[2] = templateCopy.transform.Find("Wtoggle").GetComponent<Toggle>();
            weekTog[3] = templateCopy.transform.Find("Thtoggle").GetComponent<Toggle>();
            weekTog[4] = templateCopy.transform.Find("Ftoggle").GetComponent<Toggle>();
            weekTog[5] = templateCopy.transform.Find("Stoggle").GetComponent<Toggle>();

            for(int j = 0; j < weekTog.Length; j++)
            {
                weekTog[j].isOn = item.weekDays[j];
                weekTog[j].onValueChanged.AddListener(delegate{SaveAll(item.index, 0);});
            }
        }
    }


    public void SearchRoom(int index,string input){
        SearchScript sn = GameObject.Find("UI Canvas").GetComponent<SearchScript>();
        ClassObject obj = classObjects[index];
        Outline outline = obj.label.transform.Find("RoomInput").GetComponent<Outline>();
        int match = 0;
        foreach(GameObject ele in sn.GetRoomList())
        {
            TextMeshProUGUI t = ele.transform.GetComponentInChildren<TextMeshProUGUI>(true);
            if(t != null){
                if(input.ToLower() == t.text.ToLower()){
                    outline.effectColor = new Color(0.0f,0.0f,0.0f,0.0f);
                    match = 1;
                    obj.valid = true;
                    break;
                }
            }
        }
        if (match == 0){
            outline.effectColor = new Color(1.0f,0.0f,0.0f,1.0f);
            Debug.Log("why");
            obj.valid = false;
        }
        classObjects[index] = obj;
    }

}
