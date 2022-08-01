using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drops List")]
//Scriptable Object to store all items enemies can drop for access elsewhere

public class DropsList : ScriptableObject
{
    public List<DropListEntry> dropsList;

    public int minSpawn = 1;
    public int maxSpawn = 6;

    public GameObject GetDrop()
    {
        int totalWeight = 0;
        foreach(DropListEntry drop in dropsList)
        {
            totalWeight += drop.weighting;
        }
        int rand = Random.Range(0, totalWeight);
        int i = -1;
        while(rand > 0)
        {
            i++;
            rand -= dropsList[i].weighting;
            
        }
        return (dropsList[i].drop);
    }
}
