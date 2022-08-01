﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drops List")]
//Scriptable Object to store all items enemies can drop for access elsewhere

public class DropsList : ScriptableObject
{
    public List<DropListEntry> dropsList;

    public GameObject GetDrop()
    {
        int totalWeight = 0;
        foreach(DropListEntry drop in dropsList)
        {
            totalWeight += drop.weighting;
        }
        int rand = Random.Range(1, totalWeight);
        int i = -1;
        while(rand > 0)
        {
            i++;
            rand -= dropsList[i].weighting;
            
        }
        if(i >= 8)
        {
            Debug.Log("8");
        }
        return (dropsList[i].drop);
    }
}
