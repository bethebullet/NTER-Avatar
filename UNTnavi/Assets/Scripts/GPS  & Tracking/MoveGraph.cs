using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MoveGraph : MonoBehaviour
{
    public AstarData data;
    GridGraph grid;

    void Awake()
    {
        AstarPath.active.Scan(); 
        data = AstarPath.active.data;
        grid = data.gridGraph;
        StartCoroutine(InitializeGraph());
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        grid.RelocateNodes(transform.position, Quaternion.Euler(0f, 122f, 0f), 1f, 1, 0);
    }

    IEnumerator InitializeGraph()
    {
        yield return new WaitForSeconds(1);
        AstarPath.active.Scan(); 
    }
}
