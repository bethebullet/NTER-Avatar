using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconToggle : MonoBehaviour
{
    public Toggle tog;
    Image mapIcon;

    void Start()
    {
        mapIcon = this.gameObject.GetComponent<Image>();
    }

    void Update()
    {
        if (tog.isOn)
        {
            mapIcon.color = new Color(0,0,0, 0);
        } else
        {
            mapIcon.color = new Color(1,1,1, 1);
        }
    }

}
