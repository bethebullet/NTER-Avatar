using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public struct ClassScheduleItem{
    public string roomNumber;
    public string startTime;
    public string endTime;
    public string className;
    public int index;
    public GameObject label;
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
    private List<ClassScheduleItem> classPartitions;
    private int MAX_CLASSES_ALLOWED = 8;
    private int classCount = 0;


    void Start(){
        /*float width = container.sizeDelta.x;
        float height = container.sizeDelta.y;
        GridLayoutGroup grid = TemplateContainer.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(width*.95f,height*.95f/8);
        print(grid.cellSize);
        grid.spacing = new Vector2(0.0f,height*.05f/8);*/
        //grid.Spacing.Y = height*.05/8;
        classPartitions = new List<ClassScheduleItem>();
    }
    void Update(){}
    public void SortClasses(){} // sort classes by schedule start time and set their order index
    public void NewClassTemplate(){
        if(classCount >= MAX_CLASSES_ALLOWED){return;}//dont add any more if they have 8
        ClassScheduleItem item = new ClassScheduleItem();
        var templateCopy = Instantiate(scheduleTemplate);
        RectTransform rt = templateCopy.GetComponent<RectTransform>();
        templateCopy.transform.SetParent(TemplateContainer,false);
        rt.localScale = new Vector3(1,1,1);
        item.label = templateCopy;
        item.index = classCount;
        classPartitions.Add(item);
        Transform deleteButton = templateCopy.transform.Find("DeleteButton");
        Button btn = deleteButton.GetComponent<Button>();
        btn.onClick.AddListener(delegate{DeleteClassTemplate(item.index);});
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
                Destroy(item.label);
                classPartitions.RemoveAt(i);
                classCount--;
            }else{
                if(item.index > idx){
                    item.index -= 1;
                }
            }
        }
        SortClasses();
    }
}

