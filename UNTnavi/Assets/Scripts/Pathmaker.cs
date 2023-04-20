using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathmaker : MonoBehaviour
{
    public Path path;
    public Seeker seeker;
    
    public Navi navi;

    public int currentWP;
    public float distWP;
    public float totalDist;
    public float distLeft;

    public Vector3 myLocation;
    public Vector3 target;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        myLocation = transform.position;
        target = myLocation + new Vector3(0,5,0);
        currentWP = 0;
        UpdatePath();
        navi.InitializeNavi(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(path == null)
            return;
        distWP = Vector3.Distance(myLocation, path.vectorPath[currentWP]);
        if (distWP < .75f && currentWP < path.vectorPath.Count)
            currentWP++;
        
        float sum = 0;
        for (int i = 0; i < path.vectorPath.Count - 1; i++)
        {
            sum += Vector3.Distance(path.vectorPath[i], path.vectorPath[i + 1]);
        }
        totalDist = sum;

        float sumL = 0;
        for (int i = 0; i < currentWP - 1; i++)
        {
            sumL += Vector3.Distance(path.vectorPath[i], path.vectorPath[i + 1]);
        }
        distLeft = sum - sumL;
        // Debug.Log(distWP);
        // Debug.Log(path.vectorPath.Count);
        // Debug.Log(currentWP);


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
