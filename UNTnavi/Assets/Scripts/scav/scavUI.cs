using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scavUI : MonoBehaviour
{
    public scavManager sm;
    public GameObject menu1;
    public GameObject menu2;
    public GameObject menu3;
    public GameObject menu4;
    public GameObject menu5;
    public GameObject menu6;

    public void updateMenu(int state)
    {
        switch(state)
        {
            case 1:
                menu1.SetActive(false);
                menu2.SetActive(true);
                sm.nextPuzzle();
                break;
            case 2:
                menu2.SetActive(false);
                menu3.SetActive(true);
                break;
            case 3:
                menu3.SetActive(false);
                menu4.SetActive(true);
                sm.nextPuzzle();
                break;
            case 4:
                menu4.SetActive(false);
                menu5.SetActive(true);
                break;
            case 5:
                menu5.SetActive(false);
                menu6.SetActive(true);
                sm.nextPuzzle();
                break;
            default:
            menu1.SetActive(true);
            menu6.SetActive(false);
                break;
        }
        
    }
}
