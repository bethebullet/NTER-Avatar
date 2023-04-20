using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CompassBehaviour : MonoBehaviour
{
    public TextMeshProUGUI headingText;

    public float northRotation;
    private float headingVelocity = 0f;

    public bool startTracking = false;

    void Start()
    {
        headingVelocity = 0f;

        Input.compass.enabled = true;
        Input.location.Start();
        StartCoroutine(InitializeCompass());
    }

    void Update()
    {
        if (startTracking)
        {
            northRotation = Mathf.SmoothDampAngle(northRotation, Input.compass.trueHeading, ref headingVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, 0, northRotation);
            headingText.text = ((int)northRotation).ToString() + "° " + DegreesToCardinalDetailed(northRotation);
        }

        if(northRotation > 359.9999f)
            northRotation = 0;
        if(northRotation < 0f)
            northRotation = 359.9999f;
    }

    IEnumerator InitializeCompass()
    {
        yield return new WaitForSeconds(1f);
        startTracking |= Input.compass.enabled;
    }

    private static string DegreesToCardinalDetailed(double degrees)
    {
        string[] cardinals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
        return cardinals[(int)Math.Round(((double)degrees * 10 % 3600) / 225)];
    } // Start is called before the first frame update
}
