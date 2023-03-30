using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathmaker : MonoBehaviour
{
    Path path;
    Seeker seeker;

    GameObject room;

    int currentWP;

    public Vector3 myLocation;
    public Vector3 target;
    Vector3 path3d;

    Vector3 pathDir;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        myLocation = transform.position;
        target = myLocation;
        currentWP = 0;
        UpdatePath();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = myLocation;
        transform.rotation = Quaternion.LookRotation(pathDir); 

        float distWP = Vector3.Distance(myLocation, path.vectorPath[currentWP]);
        if (distWP < .75f && currentWP < path.vectorPath.Count)
            currentWP++;

        //path3d = new Vector3(path.vectorPath[currentWP].x, 0, path.vectorPath[currentWP].y);
        pathDir = (path.vectorPath[currentWP]-myLocation).normalized;
        pathDir.y = 0; 

        if(distWP > .2f)
            myLocation += pathDir * 0.2f;
       
        Debug.Log(distWP);
        Debug.Log(pathDir);
        Debug.Log(path.vectorPath.Count);
        Debug.Log(currentWP);
    
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWP = 0;
        }
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
        {
            seeker.StartPath(myLocation, target, OnPathComplete);
        }
    }

    public void findF173()
    {
        room = GameObject.Find("F173");
        target = room.transform.position;
        Debug.Log(room.transform.position);
        UpdatePath();
    }

    public void findK102()
    {
        room = GameObject.Find("K102");
        target = room.transform.position;
        Debug.Log(room.transform.position);
        UpdatePath();
    }

    public void findCafe()
    {
        room = GameObject.Find("Cafeteria");
        target = room.transform.position;
        Debug.Log(room.transform.position);
        UpdatePath();
    }
}
