using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathmaker : MonoBehaviour
{
    Path path;
    Seeker seeker;

    int currentWP;

    public Vector3 myLocation;
    public Vector3 target;


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
        float distWP = Vector3.Distance(myLocation, path.vectorPath[currentWP]);
        if (distWP < .75f && currentWP < path.vectorPath.Count)
            currentWP++;

        Debug.Log(distWP);
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

    public void FindTarget(GameObject room)
    {
        target = room.transform.position;
        Debug.Log(room.transform.position);
        UpdatePath();
    }
}
