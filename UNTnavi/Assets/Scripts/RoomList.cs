using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomList : MonoBehaviour
{
    public string searchTag;

    private List<GameObject> actors = new List<GameObject>();
    public List<GameObject> doors;
    public List<GameObject> stairs;
    public List<GameObject> elevators;



    // Start is called before the first frame update
    void Start()
    {
        searchTag = "Door";
        if(searchTag != null)
            FindObjectwithTag(searchTag);
        doors = new List<GameObject>(actors);
        doors = doors.OrderBy(d=> d.name).ToList();

        searchTag = "Stairs";
        if(searchTag != null)
            FindObjectwithTag(searchTag);
        stairs = new List<GameObject>(actors);

        searchTag = "Elevator";
        if(searchTag != null)
            FindObjectwithTag(searchTag);
        elevators = new List<GameObject>(actors);


        GameObject.FindGameObjectWithTag("UI").GetComponent<SearchScript>().InitializeSearch(this);
    }

    public void FindObjectwithTag(string tag)
    {
        actors.Clear();
        Transform parent = transform;
        GetChildObject(parent, tag);
    }

    public void GetChildObject(Transform parent, string _tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == _tag)
            {
                actors.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                GetChildObject(child, _tag);
            }
        }
    }
}
