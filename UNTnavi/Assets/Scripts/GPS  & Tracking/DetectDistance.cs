using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using Mapbox.Utils;

public class DetectDistance : MonoBehaviour
{
    [SerializeField] LocationStatus lStatus;

    public double dist;

    public GeoCoordinatePortable.GeoCoordinate playerPos;

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = new GeoCoordinatePortable.GeoCoordinate(lStatus.currLoc.LatitudeLongitude.x, lStatus.currLoc.LatitudeLongitude.y);
        var mapPos = new GeoCoordinatePortable.GeoCoordinate(33.253689744936516f,  -97.15234794376056f);

        dist = playerPos.GetDistanceTo(mapPos); //meters
        // Debug.Log(dist);

        if(dist > 400)
        {
            //error prompt, too far from DP
        }
    }
}
