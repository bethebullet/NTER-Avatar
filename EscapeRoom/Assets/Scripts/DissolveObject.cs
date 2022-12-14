using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The dissolve effect using the shader in dissolve material
public class DissolveObject : MonoBehaviour
{
    MeshRenderer rend;
    //Material disMaterial = (Material)Resources.Load("Dissolve Material", typeof(Material));

    //local variables corresponding to shader variables
    int dissolving; //if false(0), stop animation and stay solid, true(1), let animation play
    float dissolved; //the state at which the object is dissolved, 0 - full, 1 - empty

    public float inRate; // rate at which object undissolves
    public float outRate; // rate at which object dissolves
    public float fadeTime; // time between dissolving change, dissolve speed, lower is faster
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        //rend.material.shader// = Shader.Find("Dissolve");
        dissolving = 1;

        inRate = 0.08f;
        outRate = 0.08f;
        fadeTime = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        //set shader variables to script declared variables
        rend.material.SetInteger("_Dissolving", dissolving);
        rend.material.SetFloat("_Dissolved", dissolved);


    }

    //when enabled, play undissolve animation
    void OnEnable()
    {
        dissolved = 1.0f; //fully dissolved before fading in animation
        StartCoroutine(FadeIn());
    }

    /*
    void OnDisable()
    {
        dissolved = 0.0f;
        StartCoroutine(FadeOut());
    }
    */

    //undissolve animation call
    IEnumerator FadeIn()
    {
        while(dissolved > 0) // while object dissolved at all
        {
            dissolved -= inRate; //change dissolved (alpha) by inRate
            yield return new WaitForSeconds(fadeTime); //wait fadeTime seconds between changes
        }
    }

    //dissolve animation call
    IEnumerator FadeOut()
    {
        while(dissolved < 1)
        {
            dissolved += outRate;
            yield return new WaitForSeconds(fadeTime);
        }
    }
}
