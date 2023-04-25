using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mapbox.Unity.Location;
using Mapbox.Utils;

public class DebugGPS : MonoBehaviour
{
    public Text location;
    public Text distance;

    Vector2d currentLocation;
    public DetectDistance dd;

    

    private AbstractLocationProvider _locationProvider = null;
    void Start()
    {
        if (null == _locationProvider)
        {
            _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentLocation = _locationProvider.CurrentLocation.LatitudeLongitude;
        location.text = "CL: " + currentLocation.x + ", " + currentLocation.y;

        distance.text = "Distance: " + dd.dist + " meters";
    }
}
