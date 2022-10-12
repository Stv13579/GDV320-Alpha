using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCuller : Room
{

    [SerializeField]
    GameObject north, south, east, west;

    private void Start()
    {
        base.Start();
        //Check each direction, disable them if nothing there, add them to active if not

       

        //Check each direction, disable them if nothing there, add them to active if not
        if (!levelGenerator.CheckEdgePosition(new Vector2(gridPos.x + 1, gridPos.y)))
        {
            east.SetActive(true);
        }

        if (!levelGenerator.CheckEdgePosition(new Vector2(gridPos.x - 1, gridPos.y)))
        {
            west.SetActive(true);
        }

        if (!levelGenerator.CheckEdgePosition(new Vector2(gridPos.x, gridPos.y + 1)))
        {
            north.SetActive(true);
        }


        if (!levelGenerator.CheckEdgePosition(new Vector2(gridPos.x, gridPos.y - 1)))
        {
            south.SetActive(true);
        }

    }

    
}
