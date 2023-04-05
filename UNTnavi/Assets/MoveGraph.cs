using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MoveGraph : MonoBehaviour
{
    public AstarData data;
    GridGraph grid;

    void Start()
    {
        data = AstarPath.active.data;
        grid = data.gridGraph;
    }

    // Update is called once per frame
    void Update()
    {   
        grid.RelocateNodes(transform.position, Quaternion.Euler(0f, 122f, 0f), 1f, 1, 0);
    }
}
