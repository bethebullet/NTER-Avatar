using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Script : MonoBehaviour
{
    public GameObject mainMenuObj;
    public GameObject roomSearchObj;
    public int screenState = 0;

    void Start()
    {
        mainMenuObj.SetActive(true);
        roomSearchObj.SetActive(false);
    }

    public void update(int state)
    {
        screenState = state;

        if (screenState == 0)
        {
            mainMenuObj.SetActive(true);
            roomSearchObj.SetActive(false);
        }
        else if (screenState == 1)
        {
            mainMenuObj.SetActive(false);
            roomSearchObj.SetActive(true);
        }
    }
}
