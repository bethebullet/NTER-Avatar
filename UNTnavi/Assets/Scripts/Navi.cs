using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navi : MonoBehaviour
{
    Pathmaker pm;
    TrailRenderer tr;

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
        tr = GetComponentInChildren<TrailRenderer>();
    }

    // Start is called before the first frame update
    public void InitializeNavi(Pathmaker pathmaker)
    {
        pm = pathmaker;
        travelDist = 5;
        moving = false;
        travelDist = 15;
        speed = .3f;
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
                middle = pm.path.vectorPath[i] + new Vector3 (0,1,0);
                pastMid = true;
            }
            if(distSum > travelDist)
            {
                target = pm.path.vectorPath[i] + new Vector3 (0,1,0);
                i = pm.path.vectorPath.Count;
            } 

        }

        if (!moving)
            StartCoroutine(MoveNavi());

    }

    IEnumerator MoveNavi()
    {
        // Debug.Log("MOVING!");
        moving = true;
        

        // transform.position = pm.transform.position;
        var startPos = pm.transform.position;
        transform.position = startPos;
        //yield return new WaitForEndOfFrame();
        tr.Clear();
        tr.emitting = true;

        // Debug.Log(transform.position);
        float timer = 0;
        marker = 0;

        int i = 0;
        while(Vector3.Distance(transform.position, middle) > .1f && i < 100)
        {   
            if(speed <= 0 )
                speed = .01f;

            transform.position = Vector3.Lerp(startPos, middle, timer / speed);
            
            timer += Time.deltaTime;
            i++;
            yield return new WaitForSeconds(.01f);
            Debug.Log(Vector3.Distance(transform.position, middle));
        }
        // Debug.Log("Middle");
        i = 0;
        timer = 0;
        while(Vector3.Distance(transform.position, target) > .1f && i < 100)
        {
            if(speed <= 0 )
                speed = .01f;
            
            transform.position = Vector3.Lerp(middle, target, timer / speed);

            timer += Time.deltaTime;
            i++;
            yield return new WaitForSeconds(.01f);
        }
        
        yield return new WaitForSeconds(.5f);
        tr.emitting = false;
        //yield return new WaitForEndOfFrame();
        moving = false;
    }
}
