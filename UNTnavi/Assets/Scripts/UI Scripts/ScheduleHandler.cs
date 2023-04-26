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
    public IDictionary<string,int> classDays;
    // public GameObject label;
}

public struct ClassObject{ // Unity Object
    public int index;
    public GameObject label;
    public bool valid;
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
    private List<ClassObject> classObjects;
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
    }
    void Update(){}

    public void SortClasses(){
        //classes = classes.OrderBy(c=> c.startTime).ToList();
    } // sort classes by schedule start time and set their order index

    public void NewClassTemplate(){
        if(classCount >= MAX_CLASSES_ALLOWED){return;}//dont add any more if they have 8
        ClassScheduleItem item = new ClassScheduleItem();
        ClassObject classObj = new ClassObject();
        var templateCopy = Instantiate(scheduleTemplate);
        RectTransform rt = templateCopy.GetComponent<RectTransform>();
        templateCopy.transform.SetParent(TemplateContainer,false);
        rt.localScale = new Vector3(1,1,1);

        item.index = classCount;
        classObj.index = classCount;
        classObj.label = templateCopy;

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
                // Destroy(item.label);
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
                Destroy(classObj.label);
                classObjects.RemoveAt(i);
            }else{
                if(classObj.index > idx){
                    classObj.index -= 1;
                }
            }
        }
        SortClasses();
    }

    public void BackCheck()
    {
        // for (int i = 0; i<classPartitions.Count;i++){
        //     ClassObject thisClass = classObjects[i];
        //     if(!thisClass.valid)
        //         DeleteClassTemplate(i);
        // }
        // SaveSchedule();
    }

    public void SaveAll(int idx, int editMode){
        for (int i = 0; i<classPartitions.Count;i++){
            ClassObject thisClass = classObjects[idx];
            if(idx == i){ // && thisClass.valid == true
                ClassScheduleItem item = classPartitions[idx];
                ClassScheduleItem updatedClass = new ClassScheduleItem();
                updatedClass.index = idx;
                
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

                classPartitions[idx] = updatedClass;
                Debug.Log(classPartitions[idx].className);
                Debug.Log(thisClass.valid);
                if(thisClass.valid)
                    Debug.Log("valid class saved");
                    SaveSchedule();
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
        if(hasLoaded == 0)
        {
            Debug.Log("Loading schedule.");
            string path = Application.persistentDataPath + "/schedule.bin";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                classPartitions = (List<ClassScheduleItem>) formatter.Deserialize(stream);
                stream.Close();

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
            hasLoaded++;
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
          room.onEndEdit.AddListener(delegate{SaveAll(item.index, 4);});
          room.onValueChanged.AddListener(delegate{SearchRoom(item.index,room.text);});
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
    }
}
