using System.Collections;
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
        int rand = Random.Range(0, totalWeight);
        int i = 0;
        while(totalWeight > 0)
        {
            totalWeight -= dropsList[i].weighting;
            i++;
        }
        return (dropsList[i].drop);
    }
}
