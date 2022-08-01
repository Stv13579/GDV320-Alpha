using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drops List")]
//Scriptable Object to store all items enemies can drop for access elsewhere

public class DropsList : ScriptableObject
{
    public List<DropListEntry> currencyList;
    public List<DropListEntry> ammoList;
    public List<DropListEntry> healthList;

    public int minCurrencySpawn = 1;
    public int maxCurrencySpawn = 2;

    public int minAmmoSpawn = 2;
    public int maxAmmoSpawn = 4;

    public int minHealthSpawn = 1;
    public int maxHealthSpawn = 1;

    public GameObject GetDrop(List<DropListEntry> dropsList)
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
        return (dropsList[i].drop);
    }


}
