using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navi : MonoBehaviour
{
    Pathmaker pm;

    public float travelDist;
    float distSum;
    bool pastMid;

    Vector3 middle;
    Vector3 target;

    bool moving;
    int marker;
    public float speed;

    void Awake()
    {
        pm = null;
    }

    // Start is called before the first frame update
    public void InitializeNavi(Pathmaker pathmaker)
    {
        pm = pathmaker;
        travelDist = 5;
        moving = false;
        travelDist = 10;
        speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(pm == null || pm.path == null)
            return;

        // middle = pm.path.vectorPath[pm.currentWP];
        // if (pm.currentWP == pm.path.vectorPath.Count - 1)
        //     target = middle;
        // else
        //     target = pm.path.vectorPath[pm.currentWP + 1];
        
        // if(pm.distLeft < 5)
        // {
        //     target = pm.path.vectorPath[pm.path.vectorPath.Count - 1];
        //     middle = target;
        // }

        distSum = Vector3.Distance(pm.myLocation, pm.path.vectorPath[pm.currentWP]);
        pastMid = false;
        for (int i = pm.currentWP; i < pm.path.vectorPath.Count - 1; i++)
        {
            distSum += Vector3.Distance(pm.path.vectorPath[i], pm.path.vectorPath[i + 1]);
            if(distSum > travelDist/2 && !pastMid)
            {
                middle = pm.path.vectorPath[i];
                pastMid = true;
            }
            if(distSum > travelDist)
            {
                target = pm.path.vectorPath[i];
                i = pm.path.vectorPath.Count;
            } 

        }

        if (!moving)
            StartCoroutine(MoveNavi());

    }

    IEnumerator MoveNavi()
    {
        Debug.Log("MOVING!");
        moving = true;

        transform.position = pm.myLocation;
        marker = 0;

        while(Vector3.Distance(transform.position, target) > 2)
        {
            if(Vector3.Distance(transform.position, middle) < 1)
                marker++;
            if (marker == 0)
            {
                transform.position = Vector3.Lerp(transform.position, middle, Time.deltaTime * speed);
            } else
            {
                transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            }
        }
        yield return new WaitForSeconds(.5f);
        moving = false;
    }
}
