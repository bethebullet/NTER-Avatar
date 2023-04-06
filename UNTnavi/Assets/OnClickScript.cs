using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickScript : MonoBehaviour
{
    public Pathmaker pm;
    public GameObject room;

    public void roomClick()
    {
        pm.FindTarget(room);
    }

}
