using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The dissolve effect using the shader in dissolve material
public class DissolveObject : MonoBehaviour
{
    MeshRenderer rend;
    //Material disMaterial = (Material)Resources.Load("Dissolve Material", typeof(Material));

    int dissolving; //if false(0), stop animation and stay solid, true(1), let animation play
    float dissolved; //the state at which the object is dissolved, 0 - full, 1 - empty

    public float inRate;
    public float outRate;
    public float fadeTime;
    
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
        dissolved = 1.0f;
        StartCoroutine(FadeIn());
    }

    /*
    void OnDisable()
    {
        dissolved = 0.0f;
        StartCoroutine(FadeOut());
    }
    */

    //undissolve animator call
    IEnumerator FadeIn()
    {
        while(dissolved > 0)
        {
            dissolved -= inRate;
            yield return new WaitForSeconds(fadeTime);
        }
    }

    IEnumerator FadeOut()
    {
        while(dissolved < 1)
        {
            dissolved += outRate;
            yield return new WaitForSeconds(fadeTime);
        }
    }
}
