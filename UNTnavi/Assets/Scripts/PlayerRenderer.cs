using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRenderer : MonoBehaviour
{
    MeshRenderer[] player;
    public Toggle toggle;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(toggle.isOn)
        {
            foreach (MeshRenderer mr in player)
                mr.enabled = false;
        } else
        {
            foreach (MeshRenderer mr in player)
                mr.enabled = true;
        }
    }
}
