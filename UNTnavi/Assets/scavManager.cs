using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using Mapbox.Utils;

public class scavManager : MonoBehaviour
{
    // Start is called before the first frame update
    public DetectDistance player; // used for player.plaerPos

    public UI_Script menu; // used for popups

    public bool gameStart = false; // used to check if user has begun the scavenger hunt

    public double scavDist;

    [SerializeField] private int scavProgress = 0;

    bool complete;
    GeoCoordinatePortable.GeoCoordinate libPos;
    GeoCoordinatePortable.GeoCoordinate compPos;
    GeoCoordinatePortable.GeoCoordinate cafePos;

    void Start()
    {
        libPos = new GeoCoordinatePortable.GeoCoordinate(33.254128175827304, -97.15262459005834);
        compPos = new GeoCoordinatePortable.GeoCoordinate(33.25401266313583, -97.15235645101363);
        cafePos = new GeoCoordinatePortable.GeoCoordinate(33.25441064740028, -97.15262226850868);
    }

    // Update is called once per frame
    void Update()
    {
        if(scavProgress == 0)
        {
            scavDist = player.playerPos.GetDistanceTo(libPos);
        }
        if(scavProgress == 2)
        {
            scavDist = player.playerPos.GetDistanceTo(compPos);
        }
        if(scavProgress == 4)
        {
            scavDist = player.playerPos.GetDistanceTo(cafePos);
        }
        if (scavDist < 5)
        {
            if (!complete)
            {
                scavProgress++;
                complete = true; 
                // makes button blink if not open 
                // set scavMenu passing in scavProgress
            }
        }
    }
    public void nextPuzzle()
    {
        scavDist = 1000;
        complete = false;
        scavProgress++;
    }
}
