using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchScript : MonoBehaviour
{
    public List<GameObject> Element;
    public GameObject SearchBar;
    public int totalElements;
    public GameObject maps;
    RoomList lists;
    [SerializeField] private Transform m_ContentContainer;
    [SerializeField] private GameObject m_ItemPrefab;
    [SerializeField] private  Pathmaker player;

    void Start()
    {
        lists = null;
    }

    public void InitializeSearch(RoomList x)
    {

        //if maps.GetComponent<RoomList>() is null or < .count == 349, dont do anything
        // if (maps.GetComponentInChildren<RoomList>() == null)
        //     return;

        // lists = maps.GetComponentInChildren<RoomList>();

        lists = x;
        //initialize the total number of elements to the number of doors from RoomList.cs
        totalElements = lists.doors.Count;

        //use the door list from roomList.cs
        Element = lists.doors;

        //put the elements into the content child of SearchContent
        //each element should be its own object so that the names can be viewed
        //the elements will be inside of SearchContent and will contain UI text with the name of the door
        for (int i = 0; i < totalElements; i++)
        {
            //using the prefab, create a new object inside of SearchContent
            var item_go = Instantiate(m_ItemPrefab);
            item_go.GetComponent<OnClickScript>().pm = player;
            item_go.GetComponent<OnClickScript>().room = Element[i];
            //set the text of the object to the name of the door
            item_go.GetComponentInChildren<TextMeshProUGUI>().text = Element[i].name;
            item_go.GetComponent<RectTransform>().sizeDelta = new Vector2(307.9071f, 58.1125f);
            //set name of the object to the name of the door
            item_go.name = (Element[i].name);
            //set the parent of the object to the content container
            item_go.transform.SetParent(m_ContentContainer, false);
            //set the scale of the object to 1
            item_go.transform.localScale = Vector2.one;

        }
    }

    void Update()
    {
        //if maps.GetComponent<RoomList>() is null, dont do anything
        if (lists == null)
            return;

        //if Element is full, dont do anything
        if (Element.Count == totalElements)
            return;
            
        //lists = maps.GetComponent<RoomList>();

        //initialize the total number of elements to the number of doors from RoomList.cs
        totalElements = lists.doors.Count;
        //use the door list from roomList.cs
        Element = lists.doors;
        
        //put the elements into the content child of SearchContent
        //each element should be its own object so that the names can be viewed
        //the elements will be inside of SearchContent and will contain UI text with the name of the door
        for (int i = 0; i < totalElements; i++)
        {
            //using the prefab, create a new object inside of SearchContent
            var item_go = Instantiate(m_ItemPrefab);
            //set the text of the object to the name of the door
            item_go.GetComponentInChildren<TextMeshProUGUI>().text = Element[i].name;
            item_go.GetComponent<RectTransform>().sizeDelta = new Vector2(307.9071f, 58.1125f);
            //set name of the object to the name of the door
            item_go.name = (Element[i].name);
            //set the parent of the object to the content container
            item_go.transform.SetParent(m_ContentContainer, false);
            //set the scale of the object to 1
            item_go.transform.localScale = Vector2.one;

        }
    }

    public void Search()
    {
        //loop through each element in the list
        for(int i = 0; i < Element.Count; i++)
        {
            //set the element value to the child of SearchContent
            Element[i] = m_ContentContainer.GetChild(i).gameObject;
        }

        string SearchText = SearchBar.GetComponent<TMP_InputField>().text;
        int searchTxtlength = SearchText.Length;

        int searchedElements = 0;

        foreach(GameObject ele in Element)
        {
            searchedElements += 1;

            if(ele.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length >= searchTxtlength)
            {
                if(SearchText.ToLower() == ele.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Substring(0, searchTxtlength).ToLower())
                {
                    ele.SetActive(true);
                }
                else
                {
                    ele.SetActive(false);
                }
            }
        }
    }
}
