using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    public GameObject playerT;
    public int height;

    public CompassBehaviour cb;
    float northRotation;

    // Start is called before the first frame update
    void Start()
    {
        height = 6;
    }

    // Update is called once per frame
    void Update()
    {
        if(cb.startTracking)    
            northRotation = cb.northRotation;
        transform.position = playerT.transform.position + new Vector3(0,height,0);
        transform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, northRotation, 0), Time.deltaTime * .2f);
    }
}
