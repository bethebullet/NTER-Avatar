using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OnClickScript : MonoBehaviour
{
    public UI_Script ui;
    public Pathmaker pm;
    public GameObject room;
    public SearchScript search;
    public ScheduleHandler sh;
    public Clock c;
    public GameObject inceptionRoom; 

    public void roomClick()
    {
        //Debug.Log(" Room: " + room.transform.position);
        pm.FindTarget(room);
        ui.updateMenu(0);
    }

    public void TrackClick() // track button on main screen
    {
        GameObject tempRoom = FindRoomString(search.elementKid);
        Debug.Log(" Room: " + tempRoom.transform.position);
        pm.FindTarget(tempRoom);
    }

    public void StairClick()
    {
        pm.FindTarget(FindClosestPlace(search.lists.stairs));
        ui.updateMenu(0);
    }

    public void ElevatorClick()
    {
        pm.FindTarget(FindClosestPlace(search.lists.elevators));
        ui.updateMenu(0);
    }

    public void BathroomClick()
    {
        ui.updateMenu(0);
    }

    GameObject FindRoomString(List<GameObject> rooms)
    {

        for(int i = 0; i < rooms.Count; i++)
        {
            room = rooms[i];
            if(string.Equals(rooms[i].name, c.nextClass.roomNumber, StringComparison.OrdinalIgnoreCase))
                inceptionRoom = rooms[i].GetComponent<OnClickScript>().room;
        }
        return inceptionRoom;
    }

    GameObject FindClosestPlace(List<GameObject> places)
    {
        float closestDist = 999;
        GameObject place = places[0];

        for(int i = 0; i < places.Count; i ++)
        {
            // float dist = pm.CheckPath(places[i].transform.position);
            float dist = Vector3.Distance(pm.myLocation, places[i].transform.position);
            if(dist < closestDist)
            {
                closestDist = dist;
                place = places[i];
            }
        }

        // Debug.Log(place.name);
        return place;
    }

}
