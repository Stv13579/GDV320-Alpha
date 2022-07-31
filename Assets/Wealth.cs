using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wealth : Prophecy
{
    [SerializeField]
    DropsList drops;
    DropListEntry drop;
    private void Start()
    {
        drops.dropsList[0].weighting *= 2;

    }
}
