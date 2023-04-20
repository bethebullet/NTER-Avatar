using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMap : MonoBehaviour
{
    public UI_Script ui;

    public GameObject tempM1;
    public GameObject tempM2;

    void Start()
    {
        ui = GameObject.Find("UI Canvas").GetComponent<UI_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ui == null)
            return;
            
        if (ui.mapCam.enabled)
        {
            tempM1.SetActive(true);
            tempM2.SetActive(true);
        } else
        {
            tempM1.SetActive(false);
            tempM2.SetActive(false);
        }
       
    }
}
