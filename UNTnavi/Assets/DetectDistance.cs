using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using Mapbox.Utils;

public class DetectDistance : MonoBehaviour
{
    [SerializeField] LocationStatus lStatus;

    // Update is called once per frame
    void Update()
    {
        var playerPos = new GeoCoordinatePortable.GeoCoordinate(lStatus.currLoc.LatitudeLongitude.x, lStatus.currLoc.LatitudeLongitude.y);
        var mapPos = new GeoCoordinatePortable.GeoCoordinate(33.253689744936516f,  -97.15234794376056f);

        var dist = playerPos.GetDistanceTo(mapPos); //meters
        Debug.Log(dist);

        if(dist > 1000)
        {
            //error prompt, too far from DP
        }
    }
}
