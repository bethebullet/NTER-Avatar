using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    public Color startColor = Color.black;
    public Color endColor = Color.blue;

    [Range(0,10)]
    public float speed = 1;

    MeshRenderer ren;

    void Awake()
    {
        ren = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        ren.material.SetColor("_BaseColor", Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time*speed, 1)));
        //ren.material.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time*speed, 1));
    }
}
