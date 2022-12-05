using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    //local variables to edit shader blinking colors with
    public Color startColor = Color.black;
    public Color endColor = Color.blue;

    //set a starting speed for the animation change
    [Range(0,10)]
    public float speed = 1;

    // renderer that is manipulated to show the animations
    MeshRenderer ren;

    void Awake()
    {
        //get shader component of object to edit
        ren = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        //set shader color fading to start or end color based on time
        ren.material.SetColor("_BaseColor", Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time*speed, 1)));
        //ren.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time*speed, 1));
    }
}
